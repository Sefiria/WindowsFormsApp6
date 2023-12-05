using System.Diagnostics;
using System.Drawing;

namespace Cast
{
    internal class Core
    {
        public static float RW, RH;
        public static Graphics g;
        public static Bitmap Image;
        public static Cam Cam = new Cam();
        public static PointF MS = PointF.Empty;
        public static Font Font = new Font("Segoe UI", 12);
        public static Font SmallFont = new Font("Segoe UI", 8);
        public static int StackSize = 99;

        public static Map Map = new Map();
        public static Entities Entities = new Entities();

        public static int iRW => (int)RW;
        public static int iRH => (int) RH;
        /// <summary>
        /// Half Render Width
        /// </summary>
        public static float hw => RW / 2F;
        /// <summary>
        /// Half Render Height
        /// </summary>
        public static float hh => RH / 2F;
        public static PointF CenterPoint => new PointF((int)hw, (int)hh);

        public static byte Ticks = 0;
    }
}
