using System;
using System.Drawing;
using System.Drawing.Imaging;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;

namespace Tooling
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
        public static Bitmap ChangeColor(this Bitmap image, Color from, Color to)
        {
            Bitmap Bitmap = new Bitmap(image);
            if (Image.GetPixelFormatSize(Bitmap.PixelFormat) > 8)
            {
                ChangeColorHiColoredBitmap(Bitmap, from, to);
                return Bitmap;
            }

            int indexfrom = Array.IndexOf(Bitmap.Palette.Entries, from);
            if (indexfrom < 0)
                return Bitmap; // nothing to change

            // we could replace the Color in the palette but we want to see an example for manipulating the pixels
            int indexto = Array.IndexOf(Bitmap.Palette.Entries, to);
            if (indexto < 0)
                return Bitmap; // destination Color not found - you can search for the nearest Color if you want

            ChangeColorIndexedBitmap(Bitmap, indexfrom, indexto);

            return Bitmap;
        }
        private static unsafe void ChangeColorHiColoredBitmap(Bitmap Bitmap, Color from, Color to)
        {
            int rawfrom = from.ToArgb();
            int rawto = to.ToArgb();

            BitmapData data = Bitmap.LockBits(new Rectangle(Point.Empty, Bitmap.Size), ImageLockMode.ReadWrite, Bitmap.PixelFormat);
            byte* line = (byte*)data.Scan0;
            for (int y = 0; y < data.Height; y++)
            {
                for (int x = 0; x < data.Width; x++)
                {
                    switch (data.PixelFormat)
                    {
                        case PixelFormat.Format24bppRgb:
                            byte* pos = line + x * 3;
                            int c24 = Color.FromArgb(pos[0], pos[1], pos[2]).ToArgb();
                            if (c24 == rawfrom)
                            {
                                pos[0] = (byte)(rawto & 0xff);
                                pos[1] = (byte)((rawto >> 8) & 0xff);
                                pos[2] = (byte)((rawto >> 16) & 0xff);
                            }
                            break;
                        case PixelFormat.Format32bppRgb:
                        case PixelFormat.Format32bppArgb:
                            int c32 = *((int*)line + x);
                            if (c32 == rawfrom)
                                *((int*)line + x) = rawto;
                            break;
                        default:
                            throw new NotSupportedException(); // of course, you can do the same for other pixelformats, too
                    }
                }

                line += data.Stride;
            }

            Bitmap.UnlockBits(data);
        }
        private static unsafe void ChangeColorIndexedBitmap(Bitmap image, int from, int to)
        {
            int bpp = Bitmap.GetPixelFormatSize(image.PixelFormat);
            if (from < 0 || to < 0 || from >= (1 << bpp) || to >= (1 << bpp))
                throw new ArgumentOutOfRangeException();
            if (from == to)
                return;

            BitmapData data = image.LockBits(
                new Rectangle(Point.Empty, image.Size),
                ImageLockMode.ReadWrite,
                image.PixelFormat);

            byte* line = (byte*)data.Scan0;

            // scanning through the lines
            for (int y = 0; y < data.Height; y++)
            {
                // scanning through the pixels within the line
                for (int x = 0; x < data.Width; x++)
                {
                    switch (bpp)
                    {
                        case 8:
                            if (line[x] == from)
                                line[x] = (byte)to;
                            break;
                        case 4:
                            // first pixel is the high nibble. from and to indices are 0..16
                            byte nibbles = line[x / 2];
                            if ((x & 1) == 0 ? nibbles >> 4 == from : (nibbles & 0x0f) == from)
                            {
                                if ((x & 1) == 0)
                                {
                                    nibbles &= 0x0f;
                                    nibbles |= (byte)(to << 4);
                                }
                                else
                                {
                                    nibbles &= 0xf0;
                                    nibbles |= (byte)to;
                                }

                                line[x / 2] = nibbles;
                            }
                            break;
                        case 1:
                            // first pixel is msb. from and to are 0 or 1.
                            int pos = x / 8;
                            byte mask = (byte)(128 >> (x & 7));
                            if (to == 0)
                                line[pos] &= (byte)~mask;
                            else
                                line[pos] |= mask;
                            break;
                    }
                }

                line += data.Stride;
            }

            image.UnlockBits(data);
        }
        public static Color ShadeWith(this Color self, Color other, float t, bool include_alpha = false)
        {
            byte a = self.A;
            if (include_alpha)
                a = (byte)Maths.Range(byte.MinValue, byte.MaxValue, Maths.Lerp(self.A, other.A, t));
            byte r = (byte)Maths.Range(byte.MinValue, byte.MaxValue, Maths.Lerp(self.R, other.R, t));
            byte g = (byte)Maths.Range(byte.MinValue, byte.MaxValue, Maths.Lerp(self.G, other.G, t));
            byte b = (byte)Maths.Range(byte.MinValue, byte.MaxValue, Maths.Lerp(self.B, other.B, t));
            return Color.FromArgb(a, r, g, b);
        }
    }
}