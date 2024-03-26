using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Script.StructuresAndCells;

namespace Script
{
    public static class ImagesManager
    {
        const int ResourcesDefaultTile = 8;
        static int MapTileSize = ResourcesDefaultTile;
        static bool Initialized = false;

        public static void Initialize(int TileSize)
        {
            Initialized = true;
            MapTileSize = TileSize;
        }

        public static Image GetResizedImageIfNeeded(Image img)
        {
            if (MapTileSize != ResourcesDefaultTile)
                return ResizeImage(img, img.Width / ResourcesDefaultTile * MapTileSize, img.Height / ResourcesDefaultTile * MapTileSize);
            return img;
        }
        public static Image GetImageFromTool(_Main.Tools tool)
        {
            if (!Initialized)
                throw new Exception("ImagesManager is not initialized.");

            try
            {
                return GetResizedImageIfNeeded((Image)Properties.Resources.ResourceManager.GetObject(tool.ToString()));
            }
            catch
            {
                return GetResizedImageIfNeeded(Properties.Resources.Missing);
            }
        }
        public static Image GetImageFromStructureType(Structure structure)
        {
            if (!Initialized)
                throw new Exception("ImagesManager is not initialized.");

            switch (structure)
            {
                default: return Properties.Resources.Missing;
                case StructureGenerator s: return GetImageFromTool(_Main.Tools.Generator);
                case StructureAndGate s: return GetImageFromTool(_Main.Tools.AndGate);
                case StructureXorGate s: return GetImageFromTool(_Main.Tools.XorGate);
                case StructureSwitch s: return GetImageFromTool(_Main.Tools.Switch);
                case StructureMergeTool s: return GetImageFromTool(_Main.Tools.MergeTool);
                case StructurePotentiometer s: return GetImageFromTool(_Main.Tools.Potentiometer);
                case StructureExternalOutput_1x1 s: return GetImageFromTool(_Main.Tools.ExternalOutput_1x1);
                case StructureExternalOutput_3x3 s: return GetImageFromTool(_Main.Tools.ExternalOutput_3x3);
                case StructureExternalInput s: return GetImageFromTool(_Main.Tools.ExternalInput);
            }
        }


        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
