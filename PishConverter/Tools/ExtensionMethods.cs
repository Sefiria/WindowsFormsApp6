using System.Drawing;
using System.Numerics;

namespace PishConverter.Tools
{
    internal static class ExtensionMethods
    {
        public static Point ToPoint(this Vector2 v) => new Point((int)v.X, (int)v.Y);

        public static Bitmap PartToBitmap(this byte[,] px)
        {
            var v = Global.RND.Next(20, 200);
            return px.PixelsToBitmap(Color.FromArgb(v, v, v));
        }
        public static Bitmap ToBitmap(this byte[,] px, int scale = 1) => px.PixelsToBitmap(Color.White, scale);
        public static Bitmap PixelsToBitmap(this byte[,] px, Color color, int scale = 1)
        {
            int w = px.GetLength(0);
            int h = px.GetLength(1);
            Bitmap img = new Bitmap(w * scale, h * scale);
            for (int y = 0; y < w; y++)
            {
                for (int x = 0; x < h; x++)
                {
                    if (px[y, x] == 0)
                        continue;

                    if (scale == 1)
                    {
                        img.SetPixel(x, y, color);
                    }
                    else
                    {
                        for (int j = 0; j < scale; j++)
                        {
                            for (int i = 0; i < scale; i++)
                            {
                                img.SetPixel(x * scale + i, y * scale + j, color);
                            }
                        }
                    }
                }
            }
            return img;
        }
    }
}
