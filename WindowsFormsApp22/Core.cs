using System.CodeDom;
using System.Drawing;
using Tooling;
using WindowsFormsApp22.Entities;

namespace WindowsFormsApp22
{
    internal class Core
    {
        public static float RW, RH;
        public static Graphics g;
        public static Bitmap Image;
        public const int Cube = 24;
        public static Player Player;
        public static vecf cam_ofs = vecf.Zero;
        public static PointF Cam => Player?.Pos.PlusF(cam_ofs) ?? Point.Empty;
        public static PointF MS => MouseStatesV1.Position;
        public static PointF GetTargetPoint(vecf pt) => pt.pt.PlusF(Cam.Mod(Cube)).Snap(Cube).MinusF(Cam.Mod(Cube));
        public static PointF TargetPoint => MS.PlusF(Cam.Mod(Cube)).Snap(Cube).MinusF(Cam.Mod(Cube));
        public static PointF GetTargetCube(vecf pt) => pt.pt.PlusF(Cam.Mod(Cube)).Snap(Cube).MinusF(Cam.Mod(Cube)).MinusF(Cam).Div(Cube);
        public static PointF TargetCube => MS.PlusF(Cam.Mod(Cube)).Snap(Cube).MinusF(Cam.Mod(Cube)).MinusF(Cam).Div(Cube);
        public static Font Font = new Font("Segoe UI", 12);
        public static Font SmallFont = new Font("Segoe UI", 8);
        public static int StackSize = 99;
        public static bool AltMode = true, Auto = false;
        public static Rectangle RenderBounds => new Rectangle(0, 0, iRW, iRH);
        public static Rectangle VisibleBounds => new Rectangle((int)Cam.X - (int)hw, (int)Cam.Y - (int)hh, (int)RW, (int)RH);
        public static Rectangle VisibleBoundsExt => new Rectangle((int)Cam.X - (int)hw * 2, (int)Cam.Y - (int)hh * 2, (int)RW * 2, (int)RH * 2);

        public static Map Map = new Map();
        public static EvtMgr EvtMgr = new EvtMgr();
        //public static Inventory Inventory;

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

        public static long Ticks = 0;
    }
}
