using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public class Core
    {
        public static int RW, RH;
        public static int TileSz = 32;
        public static Bitmap Image;
        public static Point MousePosition;
        public static Graphics g, gui;
        public static bool IsMouseDown = false;
        public static bool IsRightMouseDown = false;
        public static Palette Palette;
        public static ListBox ListTiles;

        public static int WT = 16;
        public static int HT = 16;
        public static Point MouseTile => new Point(MousePosition.X / TileSz, MousePosition.Y / TileSz);
        public static Point MouseSnap => new Point(MouseTile.X * TileSz, MouseTile.Y * TileSz);
    }
}
