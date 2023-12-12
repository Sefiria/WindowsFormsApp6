using System;
using System.Drawing;
using Tooling;

namespace WindowsFormsApp17
{
    internal class Core
    {
        public static int TSZ => 16;
        public static int rw, rh;
        public static Graphics g;
        public static Random RND = new Random((int)DateTime.UtcNow.Ticks);
        public static Point MouseLocation;
        public static vecf MouseVec => MouseLocation.vecf();
        public static vecf MouseTile => MouseVec.tile(TSZ);
        public static vecf MouseSnap => MouseVec.snap(TSZ);
        public static vecf MouseCamTile => (Data.Instance.cam - new vecf(rw / 2F, rh / 2F)).tile(TSZ) + MouseTile;
        public static bool MouseHolding = false;


        public static void Init()
        {
        }
    }
}
