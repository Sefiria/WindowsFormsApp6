﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp14
{
    internal class Core
    {
        public static float RW, RH;
        public static Map Map;
        public static Cam Cam;
        public static Graphics g;

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

        public static void Initialize()
        {
            Map = new Map();
            Cam = new Cam();
        }
    }
}
