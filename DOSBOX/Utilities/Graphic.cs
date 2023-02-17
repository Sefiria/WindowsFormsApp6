using System;
using System.Collections.Generic;
using System.Linq;
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
