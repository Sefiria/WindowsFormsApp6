using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp17.Utilities;

namespace WindowsFormsApp17.utilities
{
    internal static class Common
    {
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
        public static vec Normalized(this vec vec) => new vec(Maths.Normalize(vec.x), Maths.Normalize(vec.y));
        public static vecf Normalized(this vecf vec) => new vecf(Maths.Normalize(vec.x), Maths.Normalize(vec.y));
    }
}
