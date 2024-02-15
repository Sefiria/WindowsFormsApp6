using System;
using System.Drawing;
using Tooling;

namespace WindowsFormsApp17
{
    internal class Core
    {
        public static int TSZ = 16;
        public static int rw, rh;
        public static float res = 0.1F;
        public static int w => (int)(rw * res);
        public static int h => (int)(rh * res);
        public static float hw => w / 2F;
        public static float hh => h / 2F;
        public static int mw => (rw - w) / 2;
        public static int mh => (rh - h) / 2;
        public static Graphics render_g, g;
        public static Random RND = new Random((int)DateTime.UtcNow.Ticks);
        public static PointF MouseLocation => MouseStatesV1.Position;
        public static vecf MouseVec => MouseLocation.vecf();
        public static vecf MouseTile => MouseVec.tile(TSZ);
        public static vecf MouseSnap => MouseVec.snap(TSZ);
        public static vecf MouseCamTile => (Data.Instance.cam - new vecf(w / 2F, h / 2F) - Data.Instance.cam % TSZ).tile(TSZ) + MouseTile;
        public static bool MouseHolding = false;
        public static PointF CenterPoint => (w/2F,h/2F).P();
        public static RectangleF RenderBounds = new RectangleF(0, 0, w, h);
        public static Rectangle VisibleBounds => new Rectangle((int)Data.Instance.cam.x - (int)hw, (int)Data.Instance.cam.y - (int)hh, w, h);
        public static Rectangle VisibleBoundsExt => new Rectangle((int)Data.Instance.cam.x - (int)hw * 2, (int)Data.Instance.cam.y - (int)hh * 2, w * 2, h * 2);
        public static Font Font = new Font("Segoe UI", 12);
        public static Font SmallFont = new Font("Segoe UI", 8);


        public static void Init()
        {
        }
    }
}
