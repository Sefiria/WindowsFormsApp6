using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2
{
    public static class MapResManager
    {
        static Bitmap ErrorImg;
        static Dictionary<int, Bitmap> TileImages = new Dictionary<int, Bitmap>();
        public static void Load(string palette_name)
        {
            if(ErrorImg == null)
                ErrorImg = Resources.error_img;

            TileImages.Clear();
            LoadPalette(palette_name);
        }

        public static void LoadPalette(string palette_name)
        {
            Bitmap palette = (Bitmap)Resources.ResourceManager.GetObject(palette_name);
            if (palette == null)
                throw new ArgumentException($"Resource name '{palette_name}' does not exist in the Resources");

            List<Bitmap> split_images = Tools.SplitImage(palette, SharedCore.TileSize, SharedCore.TileSize);
            for (int y = 0; y < split_images.Count; y++)
                TileImages[y] = split_images[y];
        }

        public static Bitmap GetTileImgFromValue(int v)
        {
            if (!TileImages.ContainsKey(v))
                return ErrorImg;
            return TileImages[v];
        }
    }
}
