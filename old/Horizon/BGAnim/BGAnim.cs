using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGAnim
{
    public class BGAnim
    {
        private float IncrTimerResultMax = 320;
        private float IncrTimerResult = 0;
        private Bitmap Result;

        public enum AnimationDirection { Left, Right, Up, Down }

        public Color TransparentColor = Color.White;
        public Bitmap BG, MG, FG;
        public AnimationDirection DirectionBG = AnimationDirection.Left;
        public AnimationDirection DirectionFG = AnimationDirection.Right;
        public float Speed = 1F;

        static public readonly int Size = 320;

        public BGAnim()
        {
            BG = new Bitmap(Size, Size);
            MG = new Bitmap(Size, Size);
            FG = new Bitmap(Size, Size);
            Result = new Bitmap(Size, Size);
        }
        public BGAnim(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            if (Path.GetExtension(filename) != ".ban")
                throw new ArgumentException("The file should be in BAN format.");

            LoadFromBAN(filename);
        }

        private void LoadFromBAN(string filename)
        {
            byte[] bytesBG;
            byte[] bytesMid;
            byte[] bytesFG;

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = File.OpenRead(filename))
            {
                // background color (transparent)
                TransparentColor = Color.FromArgb((int)bf.Deserialize(stream));
                Speed = (float)bf.Deserialize(stream);

                // Images
                bytesBG = Convert.FromBase64String((string)bf.Deserialize(stream));
                bytesMid = Convert.FromBase64String((string)bf.Deserialize(stream));
                bytesFG = Convert.FromBase64String((string)bf.Deserialize(stream));
            }

            // No dispose of stream before disposing the bitmap
            BG = new Bitmap(new MemoryStream(bytesBG));
            MG = new Bitmap(new MemoryStream(bytesMid));
            FG = new Bitmap(new MemoryStream(bytesFG));
        }
        public void SaveAsBAN(string filename)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = File.Create(filename))
            {
                // background color (transparent)
                bf.Serialize(stream, TransparentColor.ToArgb());
                bf.Serialize(stream, Speed);
                
                // Images
                byte[] bytesBG;
                byte[] bytesMG;
                byte[] bytesFG;
                using (MemoryStream temp = new MemoryStream())
                {
                    BG.Save(temp, ImageFormat.Bmp);
                    bytesBG = temp.ToArray();
                    MG.Save(temp, ImageFormat.Bmp);
                    bytesMG = temp.ToArray();
                    FG.Save(temp, ImageFormat.Bmp);
                    bytesFG = temp.ToArray();
                }
                bf.Serialize(stream, Convert.ToBase64String(bytesBG));
                bf.Serialize(stream, Convert.ToBase64String(bytesMG));
                bf.Serialize(stream, Convert.ToBase64String(bytesFG));
            }

            MessageBox.Show("Saved as BAN");
        }
        public void SaveAsPBA(string filename)
        {
            var imageArray = GenerateFrames();

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = File.Create(filename))
            {
                // background color (transparent)
                bf.Serialize(stream, TransparentColor.ToArgb());
                bf.Serialize(stream, imageArray.Count);

                // Images
                foreach (Image img in imageArray)
                {
                    byte[] bytes;
                    using (MemoryStream temp = new MemoryStream())
                    {
                        img.Save(temp, ImageFormat.Png);
                        bytes = temp.ToArray();
                    }
                    bf.Serialize(stream, Convert.ToBase64String(bytes));
                }
            }

            MessageBox.Show("Saved as PBA");
        }
        public void SaveAsGIF(string filename)
        {
            var imageArray = GenerateFrames();

            using (var stream = new MemoryStream())
            {
                using (var encoder = new BumpKit.GifEncoder(stream))
                {
                    for (int i = 0; i < imageArray.Count; i++)
                    {
                        encoder.AddFrame(imageArray[i], 0, 0, TimeSpan.FromSeconds(0));
                    }

                    stream.Position = 0;
                    using (var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        stream.WriteTo(fileStream);
                    }
                }
            }

            MessageBox.Show("Saved as GIF");
        }
        private List<Image> GenerateFrames()
        {
            List<Image> frames = new List<Image>();
            for (int i = 0; i < IncrTimerResultMax; i += (int)Speed)
            {
                Bitmap img = new Bitmap(Animate(i));
                //ReplaceColor(img, TransparentColor, BGAnimPBA.TransparentColor);
                frames.Add(img);
            }
            return frames;
        }
        private void ReplaceColor(Bitmap img, Color prev, Color next)
        {
            using (Graphics g = Graphics.FromImage(img))
            {
                // Set the image attribute's color mappings
                ColorMap[] colorMap = new ColorMap[1];
                colorMap[0] = new ColorMap();
                colorMap[0].OldColor = prev;
                colorMap[0].NewColor = next;
                ImageAttributes attr = new ImageAttributes();
                attr.SetRemapTable(colorMap);
                // Draw using the color map
                Rectangle RenderBounds = new Rectangle(0, 0, Size, Size);
                g.DrawImage(img, RenderBounds, RenderBounds.X, RenderBounds.Y, RenderBounds.Width, RenderBounds.Height, GraphicsUnit.Pixel, attr);
            }
        }

        public Bitmap Animate(float frame = -1F)
        {
            bool customFrame = frame != -1F;
            if (!customFrame)
                frame = IncrTimerResult;

            Bitmap Result = new Bitmap(Size, Size);

            Bitmap TBG = new Bitmap(BG);
            Bitmap TMG = new Bitmap(MG);
            Bitmap TFG = new Bitmap(FG);
            TBG.MakeTransparent(TransparentColor);
            TMG.MakeTransparent(TransparentColor);
            TFG.MakeTransparent(TransparentColor);

            int BGMODX = DirectionBG == AnimationDirection.Left ? -1 : (DirectionBG == AnimationDirection.Right ? 1 : 0);
            int BGMODY = DirectionBG == AnimationDirection.Up ? -1 : (DirectionBG == AnimationDirection.Down ? 1 : 0);
            int FGMODX = DirectionFG == AnimationDirection.Left ? -1 : (DirectionFG == AnimationDirection.Right ? 1 : 0);
            int FGMODY = DirectionFG == AnimationDirection.Up ? -1 : (DirectionFG == AnimationDirection.Down ? 1 : 0);
            Point PositionBG = new Point((int)(BGMODX * frame), (int)(BGMODY * frame));
            Point PositionFG = new Point((int)(FGMODX * frame), (int)(FGMODY * frame));

            using (Graphics g = Graphics.FromImage(Result))
            {
                g.FillRectangle(new SolidBrush(TransparentColor), 0, 0, Size, Size);

                g.DrawImage(TBG, PositionBG);
                switch (DirectionBG)
                {
                    case AnimationDirection.Left: g.DrawImage(TBG, PositionBG.X + Size, PositionBG.Y); break;
                    case AnimationDirection.Right: g.DrawImage(TBG, PositionBG.X - Size, PositionBG.Y); break;
                    case AnimationDirection.Up: g.DrawImage(TBG, PositionBG.X, PositionBG.Y - Size); break;
                    case AnimationDirection.Down: g.DrawImage(TBG, PositionBG.X, PositionBG.Y + Size); break;
                }

                g.DrawImage(TMG, Point.Empty);

                g.DrawImage(TFG, PositionFG);
                switch (DirectionFG)
                {
                    case AnimationDirection.Left: g.DrawImage(TFG, PositionFG.X + Size, PositionFG.Y); break;
                    case AnimationDirection.Right: g.DrawImage(TFG, PositionFG.X - Size, PositionFG.Y); break;
                    case AnimationDirection.Up: g.DrawImage(TFG, PositionFG.X, PositionFG.Y - Size); break;
                    case AnimationDirection.Down: g.DrawImage(TFG, PositionFG.X, PositionFG.Y + Size); break;
                }
            }

            if (!customFrame)
            {
                IncrTimerResult += Speed;
                if (IncrTimerResult >= IncrTimerResultMax)
                    IncrTimerResult -= IncrTimerResultMax;
            }

            return Result;
        }


        static public Image[] LoadGIFFrames(string filename)
        {
            Image originalImg = Image.FromFile(filename);
            int numberOfFrames = originalImg.GetFrameCount(FrameDimension.Time);
            Image[] frames = new Image[numberOfFrames];

            for (int i = 0; i < numberOfFrames; i++)
            {
                originalImg.SelectActiveFrame(FrameDimension.Time, i);
                frames[i] = ((Image)originalImg.Clone());
            }

            return frames;
        }
    }

    public class BGAnimPBA
    {
        private List<Bitmap> frames = new List<Bitmap>();
        private float IncrTimerResultMax = 0;
        private float IncrTimerResult = 0;

        public Color TransparentColor;

        static public int Size => BGAnim.Size;

        public BGAnimPBA(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            if (Path.GetExtension(filename) != ".pba")
                throw new ArgumentException("The file should be in PBA format.");


            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = File.OpenRead(filename))
            {
                // background color (transparent)
                TransparentColor = Color.FromArgb((int)bf.Deserialize(stream));
                IncrTimerResultMax = (int)bf.Deserialize(stream);

                // Images
                for(int i=0; i< IncrTimerResultMax; i++)
                {
                    byte[] bytes = Convert.FromBase64String((string)bf.Deserialize(stream));

                    // No dispose of stream before disposing the bitmap
                    frames.Add(new Bitmap(new MemoryStream(bytes)));
                }
            }
        }

        public Bitmap Animate(float frame = -1F)
        {
            bool customFrame = frame != -1F;
            if (!customFrame)
                frame = IncrTimerResult;

            Bitmap Result = (Bitmap) frames[(int)frame];
            Result.MakeTransparent(TransparentColor);
            
            if (!customFrame)
            {
                IncrTimerResult++;
                if (IncrTimerResult >= IncrTimerResultMax)
                    IncrTimerResult -= IncrTimerResultMax;
            }

            return Result;
        }
    }
}
