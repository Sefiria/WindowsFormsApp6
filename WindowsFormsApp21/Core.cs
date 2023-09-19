using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp21.Animations;

namespace WindowsFormsApp21
{
    internal class Core
    {
        public static float RW, RH;
        public static Graphics g;
        public static Bitmap Image;
        public static readonly int Cube = 12;

        public static Map Map = new Map();

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

        public static void Initialize()
        {
        }
    }
}
