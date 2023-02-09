using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WindowsFormsApp8
{
    public class Tile : Autotile.Tile
    {
        public string FileName;
        public string Name;
        public byte[,] Pixels;
        public int W, H;
        public Bitmap Image;
        private int m_GradientId = 0;
        private int IterationsCount;
        public int GradientId
        {
            get => m_GradientId;
            set
            {
                m_GradientId = value;
                SecureId();
            }
        }
        public double t = 0D;
        public List<Point> ModifiedPixels = new List<Point>();
        public void SecureId()
        {
            while (m_GradientId >= IterationsCount) m_GradientId -= IterationsCount;
            while (m_GradientId < 0) m_GradientId += IterationsCount;
        }
        public void Increase()
        {
            t += Core.Palette.TickValue / 100D;
            if (t > 1D)
            {
                GradientId++;
                t = 0D;
            }
        }
        public void Decrease() { GradientId--; t = 0F; }
        public List<Point> GetAllPixelsPoints(bool notOnlyTileType = false)
        {
            var all = new List<Point>();
            for (int x = 0; x < W; x++)
            {
                for (int y = 0; y < H; y++)
                {
                    if (notOnlyTileType || Core.ListTilesTile(x, y) != null)
                        all.Add(new Point(x, y));
                }
            }
            return all;
        }

        public Tile(string fileName, string name, int w, int h, int itec)
        {
            FileName = fileName;
            Name = name;
            W = w;
            H = h;
            IterationsCount = itec;

            Pixels = new byte[W, H];
        }
        public Color GetGradient(byte px)
        {
            if (px >= Core.Palette.Pixels.Count)
                return Color.Transparent;
            int id = GradientId;
            int count = Core.Palette.Pixels[px].Gradient.Count;
            while (id >= count) id -= count;
            while (id < 0) id += count;

            if (Core.Palette.Pixels[px].IsLerp)
            {
                Color s, e, r;
                s = Core.Palette.Pixels[px].Gradient[id];
                e = id == Core.Palette.Pixels[px].Gradient.Count - 1 ? Core.Palette.Pixels[px].Gradient[0] : Core.Palette.Pixels[px].Gradient[id + 1];
                r = Color.FromArgb((byte)Maths.Lerp(s.R, e.R, t), (byte)Maths.Lerp(s.G, e.G, t), (byte)Maths.Lerp(s.B, e.B, t));
                return r;
            }
            else
            {
                return Core.Palette.Pixels[px].Gradient[id];
            }
        }
        public void SetImage()
        {
            Image = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(Image);

            for(int x=0; x<Pixels.GetLength(0); x++)
            {
                for (int y = 0; y < Pixels.GetLength(1); y++)
                {
                    g.FillRectangle(new SolidBrush(GetGradient(Pixels[x, y])), x, y, 1, 1);
                }
            }

            //var list = new List<Point>(ModifiedPixels);
            //foreach (var pt in list)
            //{
            //    g.FillRectangle(new SolidBrush(GetGradient(Pixels[pt.X, pt.Y])), pt.X, pt.Y, 1, 1);
            //    //g.DrawLine(new Pen(GetGradient(Pixels[pt.X, pt.Y])), pt.X, pt.Y, pt.X+1, pt.Y);
            //    ModifiedPixels.Remove(pt);
            //}

            g.Dispose();

            Image = Image.Resized(Core.TileSz);
        }
        public override string ToString() => Name;
    }
}
