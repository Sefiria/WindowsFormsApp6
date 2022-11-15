using nQuant;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp7
{
    public class ImageIndexing
    {
        public static void Process(Bitmap img)
        {
            img = img.Resized(Core.RW, Core.RH);
            img = new Bitmap(new WuQuantizer().QuantizeImage(img));

            int[,] argbArray = img.AsRGBArray();
            int w = argbArray.GetLength(0);
            int h = argbArray.GetLength(1);
            RenderPalClass.Pixels.Clear();
            Color c;

            for (int x = 0; x < Core.RWT; x++)
            {
                for (int y = 0; y < Core.RHT; y++)
                {
                    c = Color.FromArgb(argbArray[x * Core.TileSz, y * Core.TileSz]);
                    if (RenderPalClass.Pixels.Any(px => px.Gradient.First() == c))
                    {
                        RenderClass.Pixels[x, y] = (byte)RenderPalClass.Pixels.IndexOf(RenderPalClass.Pixels.First(px => px.Gradient.First() == c));
                    }
                    else
                    {
                        RenderPalClass.Pixels.Add(new Pixel() { Gradient = new List<Color>() { c } });
                        RenderClass.Pixels[x, y] = (byte) (RenderPalClass.Pixels.Count - 1);
                    }
                }
            }
            RenderClass.ModifiedPixels.AddRange(RenderClass.GetAllPixelsPoints());
        }
    }
}
