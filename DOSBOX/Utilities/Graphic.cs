using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DOSBOX.Utilities
{
    public class Graphic
    {
        public static void Clear(byte color, int layer)
        {
            int w = 64, h = 64;
            byte[,] px = new byte[w, h];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    px[i, j] = color;
            new DispClass(px, 0, 0).Display(layer);
        }
        public static void DisplayRect(int x, int y, int w, int h, byte color, int layer) => DisplayRectAndBounds(x, y, w, h, color, 0, 0, layer);
        public static void DisplayRectAndBounds(int x, int y, int w, int h, byte color, byte boundscolor, int thickness, int layer)
        {
            byte[,] px = new byte[w, h];

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    px[i, j] = color;

            if (thickness > 0)
            {
                if (thickness > Math.Min(w, h) / 2)
                    thickness = Math.Min(w, h) / 2;
                for (int i = 0; i < w; i++)
                    for (int j = 0; j < thickness; j++)
                    {
                        px[i, j] = boundscolor;
                        px[i, h - j - 1] = boundscolor;
                    }
                for (int j = 0; j < h; j++)
                    for (int i = 0; i < thickness; i++)
                    {
                        px[i, j] = boundscolor;
                        px[w - i - 1, j] = boundscolor;
                    }
            }

            new DispClass(px, x, y).Display(layer);
        }
        public static void DisplayHorizontalLine(int Ax, int Bx, int y, byte color, int thickness, int layer)
        {
            int xmax = Math.Max(Ax, Bx);
            int xmin = Math.Min(Ax, Bx);
            int w = xmax - xmin;
            int h = thickness;

            byte[,] px = new byte[w, h];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    px[i, j] = color;

            new DispClass(px, xmin, y).Display(layer);
        }
        public static void DisplayVerticalLine(int x, int Ay, int By, byte color, int thickness, int layer)
        {
            int ymax = Math.Max(Ay, By);
            int ymin = Math.Min(Ay, By);
            int w = thickness;
            int h = ymax - ymin;

            byte[,] px = new byte[w, h];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    px[i, j] = color;

            new DispClass(px, x, ymin).Display(layer);
        }
        public static void DisplayLine(vecf start, vecf end, byte color, int layer) => DisplayLine(start.i, end.i, color, layer);
        public static void DisplayLine(vec start, vec end, byte color, int layer)
        {
            int minx = Math.Min(start.x, end.x);
            int maxx = Math.Max(start.x, end.x);
            int miny = Math.Min(start.y, end.y);
            int maxy = Math.Max(start.y, end.y);
            var fstart = new vecf(minx, miny);
            var fend = new vecf(maxx, maxy);

            int w = maxx - minx + 1;
            int h = maxy - miny + 1;
            int length = Math.Max(w, h);

            vec origin = new vec(start.x, start.y);
            if (start.x > end.x) origin.x -= w;
            if (start.y > end.y) origin.y -= h;

            byte[,] px = new byte[w, h];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    px[i, j] = (byte)(layer == 0 ? 4 : 0);

            vec pt;
            for (float t = 0F; t <= 1F; t += 1F / length)
            {
                pt = Maths.Lerp(fstart, fend, t).i - fstart.i;
                px[pt.x, pt.y] = color;
            }

            if(!(end.x < start.x || end.y < start.y) || (end.x < start.x && end.y < start.y))
                new DispClass(px, origin.x, origin.y).DisplayExact(layer);
            else
                new DispClass(px, origin.x, origin.y).Display(layer);
        }

        public static byte[,] GetGradientHorizontal(int w, int h, List<byte> ordered_colors)
        {
            var g = new byte[w, h];
            for (int c = 0; c < ordered_colors.Count; c++)
                for (int x = c; x < c + w / ordered_colors.Count; x++)
                    for (int y = 0; y < h; y++)
                        g[x, y] = ordered_colors[c];
            return g;
        }
        public static byte[,] GetGradientVertical(int w, int h, List<byte> ordered_colors)
        {
            var g = new byte[w, h];
            for (int c = 0; c < ordered_colors.Count; c++)
                for (int x = 0; x < w; x++)
                    for (int y = c; y < c + h / ordered_colors.Count; y++)
                        g[x, y] = ordered_colors[c];
            return g;
        }

        internal static byte InvertOf(byte b, int layer)
        {
            if (layer == 0)
            {
                if (b == 4) return 4;
                return (byte)(3 - b);
            }
            else
            {
                var r = (byte)(b == 4 ? 0 : 3 - b);
                if (r == 0)
                    return 4;
                return r;
            }
        }

        public class DispClass : Disp
        {
            public DispClass(byte[,] px, int x, int y)
            {
                g = px;
                vec = new vec(x, y);
            }
        }
    }
}
