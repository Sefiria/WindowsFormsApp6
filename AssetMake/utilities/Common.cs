using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetMake.Utilities;

namespace AssetMake.utilities
{
    internal static class Common
    {
        public static List<Bitmap> SplitHorizontally(this Bitmap img, int width)
        {
            List<Bitmap> result = new List<Bitmap>();
            int w = img.Width;
            int h = img.Height;
            for (int i = 0; i < w / width; i++)
            {
                result.Add(img.Clone(new Rectangle(width * i, 0, width, h), img.PixelFormat));
            }
            return result;
        }
        public static List<Bitmap> SplitVertically(this Bitmap img, int height)
        {
            List<Bitmap> result = new List<Bitmap>();
            int w = img.Width;
            int h = img.Height;
            for (int j=0;j<h/height;j++)
            {
                result.Add(img.Clone(new Rectangle(0, height * j, w, height), img.PixelFormat));
            }
            return result;
        }
        public static Bitmap Resize(this Bitmap img, int w, int h) => new Bitmap(img, w, h);
        public static Bitmap Resize(this Bitmap img, int sz) => new Bitmap(img, sz, sz);
        public static List<Bitmap> Resize(this List<Bitmap> imgs, int w, int h) => imgs.Select(img => img.Resize(w, h)).ToList();
        public static List<Bitmap> Resize(this List<Bitmap> imgs, int sz) => imgs.Select(img => img.Resize(sz)).ToList();
        public static vecf vecf(this Point pt) => new vecf(pt.X, pt.Y);
        public static bool IsPositive(this float v) => v > 0F;
        public static bool IsNegative(this float v) => v < 0F;// 0 isn't positive or negative, no entry into the possible condition if 0
        public static vec Normalized(this vec vec) => Maths.Normalized(vec.f).i;
        public static vecf Normalized(this vecf vec) => Maths.Normalized(vec);
        public static Bitmap Resize(this Bitmap img, float factor)
        {
            factor = (float)Math.Round(factor, 2);

            if (factor == 1F)
                return img;

            int w = (int)(img.Width * factor);
            int h = (int)(img.Height * factor);
            Bitmap next = new Bitmap(w, h);

            if (factor < 1F)
            {
                for (float x = 0F; x < w; x += factor)
                {
                    for (float y = 0F; y < h; y += factor)
                    {
                        next.SetPixel((int)x, (int)y, img.GetPixel((int)(x / factor), (int)(y / factor)));
                    }
                }
                return next;
            }

            // factor > 1F
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    next.SetPixel(x, y, img.GetPixel((int)(x / factor), (int)(y / factor)));
                }
            }

            return next;
        }
    }
}
