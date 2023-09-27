using System.Drawing;

namespace MiniKube
{
    internal class Core
    {
        public static float RW, RH;
        public static Graphics g;
        public static Bitmap Image;
        public static readonly int Cube = 24;
        public static PointF Cam = PointF.Empty;
        public static float CamSpeed = 4F;
        public static PointF MS = PointF.Empty;
        public static PointF TargetPoint => MS.PlusF(Cam.Mod(Cube)).Snap(Cube).Minus(Cam.Mod(Cube));
        public static PointF TargetCube => MS.PlusF(Cam.Mod(Cube)).Snap(Cube).Minus(Cam.Mod(Cube)).Minus(Cam).Div(Cube);
        public static Font Font = new Font("Segoe UI", 12);
        public static Font SmallFont = new Font("Segoe UI", 8);
        public static int StackSize = 99;

        public static Map Map = new Map();
        public static Inventory Inventory;

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
