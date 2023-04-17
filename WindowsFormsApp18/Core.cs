using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsFormsApp18.utilities;
using WindowsFormsApp18.Utilities;

namespace WindowsFormsApp18
{
    internal class Core
    {
        public static int TSZ => 24;
        public static int rw, rh;
        public static Bitmap Image;
        public static Graphics g;
        public static Random RND = new Random((int)DateTime.UtcNow.Ticks);
        public static Point MouseLocation;
        public static vecf MouseVec => MouseLocation.vecf();
        public static vecf MouseTile => MouseVec.tile(TSZ);
        public static vecf MouseSnap => MouseVec.snap(TSZ);
        public static vec MouseCamTile => (Data.Instance.cam - new vecf(rw / 2, rh / 2)).tile(TSZ).i + MouseTile.i + 1;
        public static bool MouseHolding = false;


        public static void Init()
        {
        }
    }
}
