using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp8
{
    public class Core
    {
        public static int W, H;
        public static int TileSz = 16;
        public static Point MousePosition;
        public static Graphics g, gui;
        public static bool IsMouseDown = false;

        public static int WT => W / TileSz;
        public static int HT => H / TileSz;
        public static Point MouseTile => new Point(MousePosition.X / TileSz, MousePosition.Y / TileSz);
        public static Point MouseSnap => new Point(MouseTile.X * TileSz, MouseTile.Y * TileSz);
    }
}
