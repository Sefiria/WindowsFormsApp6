using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsFormsApp15.utilities;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;

namespace WindowsFormsApp15
{
    internal class Core
    {
        public static int TSZ => 32;
        public static vecf cam;
        public static int rw, rh;
        public static Graphics g;
        public static Random RND = new Random((int)DateTime.UtcNow.Ticks);
        public static Point MouseLocation;
        public static vecf MouseVec => MouseLocation.vecf();
        public static vecf MouseTile => MouseVec.tile(TSZ);
        public static vecf MouseSnap => MouseVec.snap(TSZ);
        public static bool MouseHolding = false;


        public static void Init()
        {
        }
    }
}
