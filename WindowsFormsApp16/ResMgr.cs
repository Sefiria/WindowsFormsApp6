using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WindowsFormsApp16.Properties;
using WindowsFormsApp16.utilities;
using WindowsFormsApp16.Utilities;

namespace WindowsFormsApp16
{
    internal class ResMgr
    {
        public static List<Bitmap> tiles, bg_tiles;

        public static Bitmap GetBGTile(int id) => id < 0 || id >= bg_tiles.Count ? Resources.missigno : bg_tiles[id];
        public static Bitmap GetTile(int id) => id < 0 || id >= tiles.Count ? Resources.missigno : tiles[id];

        public static void Init()
        {
            Bitmap tileset = Resources.tileset_static;
            tileset.MakeTransparent(Color.Black);
            int tsz = 8;
            int w = tileset.Width;
            int h = tileset.Height;
            int wc = w / tsz;
            int hc = h / tsz;

            tiles = new List<Bitmap>();
            bg_tiles = new List<Bitmap>();

            for (int y = 0; y < hc; y++)
            {
                for (int x = 0; x < wc; x++)
                {
                    tiles.Add(Resize(tileset.Clone(new Rectangle(x * tsz, y * tsz, tsz, tsz), tileset.PixelFormat), Core.TSZ / (float)tsz));
                    Bitmap bmp = new Bitmap(tiles.Last());
                    using (Graphics g = Graphics.FromImage(bmp))
                        using (Brush darkcloud_brush = new SolidBrush(Color.FromArgb(96, Color.Black)))
                            g.FillRectangle(darkcloud_brush, 0, 0, Core.TSZ, Core.TSZ);
                    bg_tiles.Add(bmp);
                }
            }
        }

        private static Bitmap Resize(Bitmap img, float factor)
        {
            factor = (float)Math.Round(factor, 2);
            
            if (factor == 1F)
                return img;

            int w = (int)(img.Width * factor);
            int h = (int)(img.Height * factor);
            Bitmap next = new Bitmap(w, h);

            if(factor < 1F)
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
