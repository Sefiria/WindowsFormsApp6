using System.Drawing;
using Tooling;
using WindowsFormsApp28_px.Properties;

namespace WindowsFormsApp28_px
{
    internal class DB
    {
        public static readonly int ItemSize = 32;

        public static void Initialize()
        {
            Textures = Resources.textures.Split2D(ItemSize);
        }

        public enum Tex
        {
            LaserGun = 0x46
        }

        public static Bitmap[,] Textures;
    }
}
