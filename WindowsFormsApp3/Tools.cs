using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Text;
using WindowsFormsApp3.Entities;

namespace WindowsFormsApp3
{
    public static class Tools
    {
        public static Random RND = new Random((int)DateTime.Now.Ticks);

        public static Bitmap MadeTransparent(this Bitmap img)
        {
            Bitmap result = new Bitmap(img);
            result.MakeTransparent();
            return result;
        }
        public static List<Bitmap> SplitImage(Bitmap b, int w, int h)
        {
            List<Bitmap> result = new List<Bitmap>();

            for(int x=0; x<b.Width; x += w)
            {
                Bitmap curimg = new Bitmap(w, h);
                var g = Graphics.FromImage(curimg);
                g.DrawImage(b, new Rectangle(0, 0, w, h), new Rectangle(x, 0, w, h), GraphicsUnit.Pixel);
                g.Dispose();
                curimg.MakeTransparent(Color.White);
                result.Add(curimg);
            }

            return result;
        }
        public static Brush SetAlpha(this Color brush, int alpha) => new SolidBrush(Color.FromArgb(alpha, brush.R, brush.G, brush.B));
        public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (var item in items)
                stack.Push(item);
        }

        static ColorMatrix matrix1 = new ColorMatrix();
        static ColorMatrix matrix2 = new ColorMatrix();
        static ImageAttributes attributes1 = new ImageAttributes();
        static ImageAttributes attributes2 = new ImageAttributes();
        public static Bitmap GetFaded(Bitmap B, Bitmap A, float t)
        {
            if (A == null || B == null)
                throw new ArgumentException("One of the bitmap input isn't initialized.");

            int Width = A.Width;
            int Height = A.Height;

            Bitmap img = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                matrix1.Matrix33 = 1 - t;
                attributes1.SetColorMatrix(matrix1, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                matrix2.Matrix33 = t;
                attributes2.SetColorMatrix(matrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(A, new Rectangle(0, 0, Width, Height), 0, 0, Width,
                                        Height, GraphicsUnit.Pixel, attributes1);
                g.DrawImage(B, new Rectangle(0, 0, Width, Height), 0, 0, Width,
                                        Height, GraphicsUnit.Pixel, attributes2);
            }
            return img;
        }

        public static Bitmap RotatedAlphaResizedFaded(Bitmap A, Bitmap B, float t, float angle, float opacity, float scale)
        {
            Bitmap bmp = GetFaded(A, B, t);
            int w = (int)(bmp.Width * scale);
            int h = (int)(bmp.Height * scale);
            if (w < 1) w = 1;
            if (h < 1) h = 1;
            Bitmap rotatedImage = new Bitmap(w, h);
            rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
                // Rotate
                g.RotateTransform(angle);
                // Restore rotation point in the matrix
                g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);
                // Draw the image on the bitmap

                //create a color matrix object  
                ColorMatrix matrix = new ColorMatrix();
                //set the opacity  
                matrix.Matrix33 = opacity;
                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();
                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                // draw the image  
                g.DrawImage(bmp, new Rectangle(0, 0, w, h), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            }

            return rotatedImage;
        }
        public static Bitmap Rotated(this Bitmap bmp, float angle)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            Bitmap rotatedImage = new Bitmap(w, h);
            rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(w / 2F, h / 2F);
                g.RotateTransform(angle);
                g.TranslateTransform(-w / 2F, -h / 2F);
                g.DrawImage(bmp, new Rectangle(0, 0, w, h), 0, 0, w, h, GraphicsUnit.Pixel, new ImageAttributes());
            }
            return rotatedImage;
        }
        public static Bitmap RotatedAlphaResized(Bitmap bmp, float angle, float opacity, float scale)
        {
            int w = (int)(bmp.Width * scale);
            int h = (int)(bmp.Height * scale);
            if (w < 1) w = 1;
            if (h < 1) h = 1;
            Bitmap rotatedImage = new Bitmap(w, h);
            rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform(bmp.Width / 2F, bmp.Height / 2F);
                // Rotate
                g.RotateTransform(angle);
                // Restore rotation point in the matrix
                g.TranslateTransform(-bmp.Width / 2F, -bmp.Height / 2F);
                // Draw the image on the bitmap

                //create a color matrix object  
                ColorMatrix matrix = new ColorMatrix();
                //set the opacity  
                matrix.Matrix33 = opacity;
                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();
                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                // draw the image  
                g.DrawImage(bmp, new Rectangle(0, 0, w, h), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            }

            return rotatedImage;
        }

        public static byte[] ImgToBArray(Bitmap image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            byte[] arr = ms.ToArray();
            ms.Dispose();
            return arr;
        }
        public static string ImgToString(Bitmap image) => Encoding.Default.GetString(ImgToBArray(image));
        internal static Bitmap StringToImg(string szimg)
        {
            if (string.IsNullOrWhiteSpace(szimg))
                return new Bitmap(16, 16);
            byte[] bytes = Encoding.Default.GetBytes(szimg);
            MemoryStream ms = new MemoryStream(bytes);
            Bitmap img = new Bitmap(ms);
            ms.Dispose();
            return img;
        }
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return Convert.ToBase64String(mso.ToArray());
            }
        }

        public static string Unzip(string bytes)
        {
            try
            {
                using (var msi = new MemoryStream(Convert.FromBase64String(bytes)))
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        //gs.CopyTo(mso);
                        CopyTo(gs, mso);
                    }

                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }
            catch (Exception) { return null; }
        }

        public static bool IsCloserToPlayerThan(this DrawableEntity cur, DrawableEntity collider) => Maths.SQ(cur.X - SharedData.Player.X) + Maths.SQ(cur.Y - SharedData.Player.Y) < Maths.SQ(collider.X - SharedData.Player.X) + Maths.SQ(collider.Y - SharedData.Player.Y);
    }
}