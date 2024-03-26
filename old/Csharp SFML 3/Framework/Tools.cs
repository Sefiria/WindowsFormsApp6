using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sfColor = SFML.Graphics.Color;
using sfImage = SFML.Graphics.Image;

namespace Framework
{
    public class Tools
    {
        public static int TileSize = 16;
        public static int ChunkSize = 16;
        public static int ImageLength = 0;
        public static int MapWidth = 16 * ChunkSize;
        public static int MapHeight = 8 * ChunkSize;
        public static int MapLayers = 3;
        public static int PlayRenderWidth = 32;
        public static int PlayRenderHeight = 16;
        public static int EditorRenderWidth = 672 / TileSize;
        public static int EditorRenderHeight = 256 / TileSize;

        private static float t_lerpColor = 0F;
        private static Stopwatch watchColorDeltaIncr = new Stopwatch();
        private static List<EntityProperties> m_allEntities = null;
        private static List<EntityProperties> AllEntities { get { if (m_allEntities == null) m_allEntities = GetAllEntitiesAnUninstantiables(); return m_allEntities; } }

        static public void Initialize(int tileSize)
        {
            TileSize = tileSize;
            ImageLength = TileSize * TileSize * 4;
            watchColorDeltaIncr.Start();
        }
        static public float Lerp(float firstFloat, float secondFloat, float by)
        {
            return (1F - by) * firstFloat + by * secondFloat;
        }
        static public Vector2f Lerp(Vector2f firstVector, Vector2f secondVector, float by)
        {
            float retX = Lerp(firstVector.X, secondVector.X, by);
            float retY = Lerp(firstVector.Y, secondVector.Y, by);
            return new Vector2f(retX, retY);
        }
        static public Bitmap DecodeBitmap(string bytes)
        {
            string[] arr = bytes.Split('-');
            byte[] array = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i], 16);
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            return (Bitmap)tc.ConvertFrom(array);
        }
        static public sfColor ColorToSfColor(Color col)
        {
            return new sfColor(col.R, col.G, col.B, col.A);
        }
        static public Color SfColorToColor(sfColor col)
        {
            return Color.FromArgb(col.A, col.R, col.G, col.B);
        }
        static public sfImage BitmapToSfImage(Bitmap img)
        {
            sfImage sfimg = new sfImage((uint)img.Width, (uint)img.Height);

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    sfimg.SetPixel((uint)i, (uint)j, ColorToSfColor(img.GetPixel(i, j)));
                }
            }

            return sfimg;
        }
        static public List<sfImage> ListBitmapToListSfImage(List<Bitmap> list)
        {
            var result = new List<sfImage>();
            foreach (var raw in list)
                result.Add(BitmapToSfImage(raw));
            return result;
        }
        static public List<Bitmap> ListSfImageToListBitmap(List<sfImage> list)
        {
            var result = new List<Bitmap>();
            foreach (var raw in list)
                result.Add(SfImageToBitmap(raw));
            return result;
        }
        static public sfImage BitmapToSfImage_Normalized(Bitmap img)
        {
            return BitmapToSfImage(BitmapNormalize(img));
        }
        static public Bitmap SfImageToBitmap_Normalized(sfImage img)
        {
            return SfImageToBitmap(SfImageNormalize(img));
        }
        static public sfImage BitmapToSfImage_Scaled(Bitmap img)
        {
            return BitmapToSfImage(BitmapScale(img));
        }
        static public Bitmap SfImageToBitmap_Scaled(sfImage img)
        {
            return SfImageToBitmap(SfImageScale(img));
        }
        static public Bitmap SfImageToBitmap(sfImage img)
        {
            Bitmap bmp = new Bitmap((int)img.Size.X, (int)img.Size.Y);

            for (uint i = 0; i < img.Size.X; i++)
            {
                for (uint j = 0; j < img.Size.Y; j++)
                {
                    bmp.SetPixel((int)i, (int)j, SfColorToColor(img.GetPixel(i, j)));
                }
            }

            return bmp;
        }
        static public Bitmap BitmapNormalize(Bitmap img)
        {
            Bitmap result = new Bitmap(TileSize, TileSize);

            for (int i = 0; i < TileSize; i++)
                for (int j = 0; j < TileSize; j++)
                    result.SetPixel(i, j, img.GetPixel(i * 8, j * 8));

            return result;
        }
        static public sfImage SfImageNormalize(sfImage img)
        {
            sfImage result = new sfImage((uint)TileSize, (uint)TileSize);

            for (uint i = 0; i < TileSize; i++)
                for (uint j = 0; j < TileSize; j++)
                    result.SetPixel(i, j, img.GetPixel(i * 8, j * 8));

            return result;
        }
        static public Bitmap BitmapScale(Bitmap img)
        {
            Bitmap result = new Bitmap(TileSize * 8, TileSize * 8);

            Color color;
            for (int i = 0; i < TileSize; i++)
                for (int j = 0; j < TileSize; j++)
                {
                    color = img.GetPixel(i, j);
                    for (int x = 0; x < 8; x++)
                        for (int y = 0; y < 8; y++)
                            result.SetPixel(i * 8 + x, j * 8 + y, color);
                }

            return result;
        }
        static public sfImage SfImageScale(sfImage img)
        {
            sfImage result = new sfImage((uint)TileSize * 8, (uint)TileSize * 8);

            sfColor color;
            for (uint i = 0; i < TileSize; i++)
                for (uint j = 0; j < TileSize; j++)
                {
                    color = img.GetPixel(i, j);
                    for (uint x = 0; x < 8; x++)
                        for (uint y = 0; y < 8; y++)
                            result.SetPixel(i * 8 + x, j * 8 + y, color);
                }

            return result;
        }
        static public int Snap(int coord)
        {
            return (coord / TileSize) * TileSize;
        }
        static public Point Snap(Point pos)
        {
            return new Point((pos.X / TileSize) * TileSize, (pos.Y / TileSize) * TileSize);
        }
        static public bool CompareSfColors(sfColor a, sfColor b)
        {
            return a.A == b.A && a.R == b.R && a.G == b.G && a.B == b.B;
        }
        static public bool CompareColors(Color a, Color b)
        {
            return a.ToArgb() == b.ToArgb();
        }
        static public bool CompareSfColorToColor(sfColor a, Color b)
        {
            return a.A == b.A && a.R == b.R && a.G == b.G && a.B == b.B;
        }
        static public byte[] SfImageToBytes(sfImage img)
        {
            var bytes = new byte[ImageLength];
            int i = 0;
            for (uint x = 0; x < TileSize; x++)
            {
                for (uint y = 0; y < TileSize; y++)
                {
                    var color = img.GetPixel(x, y);
                    bytes[i + 0] = color.R;
                    bytes[i + 1] = color.G;
                    bytes[i + 2] = color.B;
                    bytes[i + 3] = color.A;
                    i += 4;
                }
            }
            return bytes;
        }
        static public sfColor[,] GetPixelsColors16x16FromByteArray1024(byte[] array)
        {
            var result = new sfColor[16, 16];

            byte R, G, B, A;
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    R = array[x * y + x + 0];
                    G = array[x * y + x + 1];
                    B = array[x * y + x + 2];
                    A = array[x * y + x + 3];
                    result[x, y] = new sfColor(R, G, B, A);
                }
            }

            return result;
        }
        static public string GetBitmapEncoded(Bitmap bmp)
        {
            return BitConverter.ToString((byte[])new ImageConverter().ConvertTo(bmp, typeof(byte[])));
        }
        static public Bitmap GetBitmapDecoded(string bytes)
        {
            if(string.IsNullOrWhiteSpace(bytes))
            {
                var img = new Bitmap(TileSize, TileSize);
                using (var g = Graphics.FromImage(img))
                {
                    g.FillRectangle(new SolidBrush(Color.PaleVioletRed), new RectangleF(0F, 0F, TileSize, TileSize));
                    g.DrawString("X", new Font("Arial", 10F), new SolidBrush(Color.Red), new RectangleF(4F, 2F, TileSize - 4, TileSize - 2));
                    g.DrawString("X", new Font("Arial", 10F), new SolidBrush(Color.Black), new RectangleF(0F, 0F, TileSize, TileSize));
                }
                return img;
            }

            string[] arr = bytes.Split('-');
            byte[] array = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i], 16);
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            return (Bitmap)tc.ConvertFrom(array);
        }
        static public string GetSfImageEncoded(sfImage sfimg)
        {
            return GetBitmapEncoded(SfImageToBitmap(sfimg));
        }
        static public sfImage GetSfImageDecoded(string bytes)
        {
            return BitmapToSfImage(GetBitmapDecoded(bytes));
        }
        static public List<EntityProperties> GetAllEntities()
        {
            var result = new List<EntityProperties>();

            var array = Enum.GetValues(typeof(GlobalVariables.Instantiable));
            foreach (var enumValue in array)
                result.AddRange(GetSubFolderEntities(enumValue.ToString()));

            return result;
        }
        static public List<EntityProperties> GetAllEntitiesAnUninstantiables()
        {
            var result = new List<EntityProperties>();

            var Instantiables = Enum.GetValues(typeof(GlobalVariables.Instantiable));
            var Uninstantiables = Enum.GetValues(typeof(GlobalVariables.Uninstantiable));
            var array = new object[Instantiables.Length + Uninstantiables.Length];
            Instantiables.CopyTo(array, 0);
            Uninstantiables.CopyTo(array, Instantiables.Length);

            foreach (var enumValue in array)
                result.AddRange(GetSubFolderEntities(enumValue.ToString()));

            return result;
        }
        static public List<EntityProperties> GetSubFolderEntities(string subfolder)
        {
            var result = new List<EntityProperties>();

            string path = Directory.GetDirectories(Directory.GetCurrentDirectory() + "/Entities/", subfolder, SearchOption.AllDirectories).FirstOrDefault();
            var files = Directory.EnumerateFiles(path);
            foreach (var file in files)
                result.Add(EntityProperties.Load(file));

            return result;
        }
        static public List<(Bitmap, string, EntityProperties)> GetListBitmapAndNameInEntityPropertiesInEntitySubFolder(string subfolder)
        {
            var result = new List<(Bitmap, string, EntityProperties)>();
            var entities = GetSubFolderEntities(subfolder);

            foreach (var entity in entities)
                result.Add((SfImageToBitmap(entity.image), entity.Name, entity));

            return result;
        }
        static public Color GetColorRainbow(byte alpha = 255)
        {
            var RGBColor = Color.FromArgb(Tools.ColorHLSToRGB((int)Tools.Lerp(0F, 240F, t_lerpColor), 120, 120));
            var result = Color.FromArgb(alpha, RGBColor.R, RGBColor.G, RGBColor.B);
            return result;
        }
        static public void Increment_t_lerpColor()
        {
            t_lerpColor += 0.1F * watchColorDeltaIncr.ElapsedMilliseconds / 100F;
            watchColorDeltaIncr.Restart();
            if (t_lerpColor > 1F) t_lerpColor = 0F;
        }
        static public Point ClientToWorld(Point point)
        {
            return new Point(point.X / TileSize, point.Y / TileSize);
        }
        static public EntityProperties GetEntityPropertyFromID(byte ID)
        {
            foreach(var entity in AllEntities)
            {
                if (entity.ID == ID)
                    return entity;
            }
            return null;
        }
        static public sfImage[] Split4SfImage(sfImage img)
        {
            sfImage topleft, topright, bottomleft, bottomright;

            topleft     = new sfImage(img.Size.X / 2, img.Size.Y / 2, sfColor.Transparent);
            topright    = new sfImage(img.Size.X / 2, img.Size.Y / 2, sfColor.Transparent);
            bottomleft  = new sfImage(img.Size.X / 2, img.Size.Y / 2, sfColor.Transparent);
            bottomright = new sfImage(img.Size.X / 2, img.Size.Y / 2, sfColor.Transparent);

            for (uint x = 0; x < img.Size.X / 2; x++)
                for (uint y = 0; y < img.Size.Y / 2; y++)
                    topleft.SetPixel(x, y, img.GetPixel(x, y));

            for (uint x = img.Size.X / 2; x < img.Size.X; x++)
                for (uint y = 0; y < img.Size.Y / 2; y++)
                    topright.SetPixel(x - img.Size.X / 2, y, img.GetPixel(x, y));

            for (uint x = 0; x < img.Size.X / 2; x++)
                for (uint y = img.Size.Y / 2; y < img.Size.Y; y++)
                    bottomleft.SetPixel(x, y - img.Size.Y / 2, img.GetPixel(x, y));

            for (uint x = img.Size.X / 2; x < img.Size.X; x++)
                for (uint y = img.Size.Y / 2; y < img.Size.Y; y++)
                    bottomright.SetPixel(x - img.Size.X / 2, y - img.Size.Y / 2, img.GetPixel(x, y));

            return new sfImage[]{ topleft, topright, bottomleft, bottomright };
        }
        static public sfImage UnSplit4SfImage(sfImage topleft, sfImage topright, sfImage bottomleft, sfImage bottomright)
        {
            sfImage img = new sfImage(topleft.Size.X * 2, topleft.Size.Y * 2, sfColor.Transparent);

            for (uint x = 0; x < img.Size.X / 2; x++)
                for (uint y = 0; y < img.Size.Y / 2; y++)
                {
                    img.SetPixel(x,                     y,                      topleft.GetPixel     (x, y));
                    img.SetPixel(x + img.Size.X / 2,    y,                      topright.GetPixel    (x, y));
                    img.SetPixel(x,                     y + img.Size.Y / 2,     bottomleft.GetPixel  (x, y));
                    img.SetPixel(x + img.Size.X / 2,    y + img.Size.Y / 2,     bottomright.GetPixel (x, y));
                }

            return img;
        }
        static public List<Bitmap> SplitBitmap(Bitmap img, int size)
        {
            List<Bitmap> result = new List<Bitmap>();
            for (int y = 0; y < img.Size.Height / size; y++)
            {
                for (int x = 0; x < img.Size.Width / size; x++)
                {
                    var splitted = new Bitmap(size, size);
                    using (var g = Graphics.FromImage(splitted))
                        g.DrawImage(img, new Rectangle(0, 0, size, size), new Rectangle(x * size, y * size, size, size), GraphicsUnit.Pixel);
                    splitted.MakeTransparent();
                    result.Add(splitted);
                }
            }
            return result;
        }

        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);

        // Convert an object to a byte array
        static public byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // Convert a byte array to an Object
        static public Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
