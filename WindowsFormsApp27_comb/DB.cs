using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp27_comb.Properties;

namespace WindowsFormsApp27_comb
{
    internal class DB
    {
        public static void Initialize()
        {
            Textures = Resources.textures.Split2D(32);
        }

        public enum Tex
        {
            Emerald = 0x4A
        }

        public static Bitmap[,] Textures;
    }
}
