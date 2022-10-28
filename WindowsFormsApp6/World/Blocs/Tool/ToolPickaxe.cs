using System.Drawing;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.World.Ores;

namespace WindowsFormsApp6.World.Blocs.Tool
{
    public class ToolPickaxe : ToolBase
    {
        public ToolPickaxe(OreType quality, int radius = 1)
        {
            //BaseQuality = 1;//devient Enchant
            Quality = quality;
            Radius = radius;
            Image = GetImageFromQuality(Quality);
        }

        public Bitmap GetImageFromQuality(OreType quality)
        {
            switch(quality)
            {
                default:
                case OreType.None: return Resources.hand.Transparent();
                case OreType.Bronze: return Colorize(Resources.pickaxe.Transparent(), Color.FromArgb(155, 108, 0));
                case OreType.Silver: return Colorize(Resources.pickaxe.Transparent(), Color.FromArgb(136, 136, 136));
                case OreType.Gold: return Colorize(Resources.pickaxe.Transparent(), Color.FromArgb(255, 249, 9));
                case OreType.Titanium: return Colorize(Resources.pickaxe.Transparent(), Color.FromArgb(218, 218, 218));
                case OreType.Diamond: return Colorize(Resources.pickaxe.Transparent(), Color.FromArgb(0, 230, 249));
            }
        }
        private Bitmap Colorize(Bitmap img, Color color)
        {
            Bitmap b = img.FloodFilled(new Point(6, 6), Color.FromArgb(1, 1, 1), color);
            return b;
        }
    }
}
