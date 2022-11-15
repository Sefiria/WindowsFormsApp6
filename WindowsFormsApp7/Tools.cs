using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp7
{
    public static class Tools
    {
        public static Random RND = new Random((int)DateTime.Now.Ticks);
        //public static int Snap(this int v) => v / Data.Instance.State.TileSz * Data.Instance.State.TileSz;
        //public static int ToUnit(this int v) => v / Data.Instance.State.TileSz;
        //public static int ToCurWorld(this int v, int offset = 0) => v * Data.Instance.State.TileSz + offset;
        //public static int ToWorld(this int v, int offset = 0) => v * Data.Instance.World.TileSz + offset;
        public static Bitmap Resized(this Bitmap img, int w, int h) => new Bitmap(img, w, h);
        public static Bitmap Resized(this Bitmap img, int sz) => img.Resized(sz, sz);
        public static Bitmap Transparent(this Bitmap img) { var b = new Bitmap(img); b.MakeTransparent(); return b; }

        private static bool ColorMatch(Color a, Color b)
        {
            return (a.ToArgb() & 0xffffff) == (b.ToArgb() & 0xffffff);
        }

        public static void FloodFill(Bitmap bmp, Point pt, Color targetColor, Color replacementColor)
        {
            Queue<Point> q = new Queue<Point>();
            q.Enqueue(pt);
            while (q.Count > 0)
            {
                Point n = q.Dequeue();
                if (!ColorMatch(bmp.GetPixel(n.X, n.Y), targetColor))
                    continue;
                Point w = n, e = new Point(n.X + 1, n.Y);
                while ((w.X >= 0) && ColorMatch(bmp.GetPixel(w.X, w.Y), targetColor))
                {
                    bmp.SetPixel(w.X, w.Y, replacementColor);
                    if ((w.Y > 0) && ColorMatch(bmp.GetPixel(w.X, w.Y - 1), targetColor))
                        q.Enqueue(new Point(w.X, w.Y - 1));
                    if ((w.Y < bmp.Height - 1) && ColorMatch(bmp.GetPixel(w.X, w.Y + 1), targetColor))
                        q.Enqueue(new Point(w.X, w.Y + 1));
                    w.X--;
                }
                while ((e.X <= bmp.Width - 1) && ColorMatch(bmp.GetPixel(e.X, e.Y), targetColor))
                {
                    bmp.SetPixel(e.X, e.Y, replacementColor);
                    if ((e.Y > 0) && ColorMatch(bmp.GetPixel(e.X, e.Y - 1), targetColor))
                        q.Enqueue(new Point(e.X, e.Y - 1));
                    if ((e.Y < bmp.Height - 1) && ColorMatch(bmp.GetPixel(e.X, e.Y + 1), targetColor))
                        q.Enqueue(new Point(e.X, e.Y + 1));
                    e.X++;
                }
            }
        }
        public static void FloodFill(Point pt, byte replacementValue)
        {
            byte targetValue = RenderClass.Pixels[pt.X, pt.Y];
            if (targetValue == replacementValue)
                return;

            Queue<Point> q = new Queue<Point>();
            q.Enqueue(pt);
            while (q.Count > 0)
            {
                Point n = q.Dequeue();
                if (RenderClass.Pixels[n.X, n.Y] != targetValue)
                    continue;
                Point w = n, e = new Point(n.X + 1, n.Y);
                while ((w.X >= 0) && RenderClass.Pixels[w.X, w.Y] == targetValue)
                {
                    RenderClass.Pixels[w.X, w.Y] = replacementValue;
                    RenderClass.ModifiedPixels.Add(new Point(w.X, w.Y));
                    if ((w.Y > 0) && RenderClass.Pixels[w.X, w.Y - 1] == targetValue)
                        q.Enqueue(new Point(w.X, w.Y - 1));
                    if ((w.Y < Core.RHT - 1) && RenderClass.Pixels[w.X, w.Y + 1] == targetValue)
                        q.Enqueue(new Point(w.X, w.Y + 1));
                    w.X--;
                }
                while ((e.X <= Core.RWT - 1) && RenderClass.Pixels[e.X, e.Y] == targetValue)
                {
                    RenderClass.Pixels[e.X, e.Y] = replacementValue;
                    RenderClass.ModifiedPixels.Add(new Point(e.X, e.Y));
                    if ((e.Y > 0) && RenderClass.Pixels[e.X, e.Y - 1] == targetValue)
                        q.Enqueue(new Point(e.X, e.Y - 1));
                    if ((e.Y < Core.RHT - 1) && RenderClass.Pixels[e.X, e.Y + 1] == targetValue)
                        q.Enqueue(new Point(e.X, e.Y + 1));
                    e.X++;
                }
            }
        }
        public static Bitmap FloodFilled(this Bitmap bmp, Point pt, Color targetColor, Color replacementColor)
        {
            FloodFill(bmp, pt, targetColor, replacementColor);
            return bmp;
        }

        public static string AsString<T>(this T t) where T : struct, IConvertible
        {
            return Enum.GetName(typeof(T), t);
        }
        public static int[,] AsRGBArray(this Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;

            int[,] array2d = new int[w, h];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    array2d[x, y] = img.GetPixel(x, y).ToArgb();
                }
            }

            return array2d;
        }
    }
}
