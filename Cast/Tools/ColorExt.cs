using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cast.Tools
{
    public static class ColorExt
    {
        public static Color ChangeSaturation(this Color c, int amount)
        {
            float s = SetToMaxOrMin(amount * 0.01f + c.GetSaturation());
            return ColorFromAhsb(c.A, c.GetHue(), s, c.GetBrightness());
        }
        public static Color ColorFromAhsb(int a, float h, float s, float b)
        {
            if (0 > a || (int)byte.MaxValue < a)
                throw new ArgumentOutOfRangeException("a", a, "Invalid Alpha");
            if (0.0 > (double)h || 360.0 < (double)h)
                throw new ArgumentOutOfRangeException("h", h, "Invalid Hue");
            if (0.0 > (double)s || 1.0 < (double)s)
                throw new ArgumentOutOfRangeException("s", s, "Invalid Saturation");
            if (0.0 > (double)b || 1.0 < (double)b)
                throw new ArgumentOutOfRangeException("b", b, "Invalid Brightness");
            if (0.0 == (double)s)
                return Color.FromArgb(a, Convert.ToInt32(b * (float)byte.MaxValue), Convert.ToInt32(b * (float)byte.MaxValue), Convert.ToInt32(b * (float)byte.MaxValue));
            float num1;
            float num2;
            if (0.5 < (double)b)
            {
                num1 = b - b * s + s;
                num2 = b + b * s - s;
            }
            else
            {
                num1 = b + b * s;
                num2 = b - b * s;
            }
            int num3 = (int)Math.Floor((double)h / 60.0);
            if (300.0 <= (double)h)
                h -= 360f;
            h /= 60f;
            h -= 2f * (float)Math.Floor((num3 + 1.0) % 6.0 / 2.0);
            float num4 = 0 != num3 % 2 ? num2 - h * (num1 - num2) : h * (num1 - num2) + num2;
            int num5 = Convert.ToInt32(num1 * byte.MaxValue);
            int num6 = Convert.ToInt32(num4 * byte.MaxValue);
            int num7 = Convert.ToInt32(num2 * byte.MaxValue);
            switch (num3)
            {
                case 1:
                    return Color.FromArgb(a, num6, num5, num7);
                case 2:
                    return Color.FromArgb(a, num7, num5, num6);
                case 3:
                    return Color.FromArgb(a, num7, num6, num5);
                case 4:
                    return Color.FromArgb(a, num6, num7, num5);
                case 5:
                    return Color.FromArgb(a, num5, num7, num6);
                default:
                    return Color.FromArgb(a, num5, num6, num7);
            }
        }
        private static float SetToMaxOrMin(float s)
        {
            if ((double)s > 1.0)
                s = 1f;
            if ((double)s < 0.0)
                s = 0.0f;
            return s;
        }
    }
}