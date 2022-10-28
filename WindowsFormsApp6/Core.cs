using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    public static class Core
    {
        public static int W, H;
        public static Graphics g;
        public static int TileSz => Data.Instance.State?.TileSz ?? TileSzBase;
        public static readonly int TileSzBase = 24;
        public static Point MousePosition;
    }
}
