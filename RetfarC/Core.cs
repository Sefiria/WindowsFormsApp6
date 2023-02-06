using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetfarC
{
    public static class Core
    {
        public static Graphics g;
        public static int W, H, Wh, Hh, TW, TH;
        public static bool FormFocused = true;
        public static Point FormPosition;
        public static Stopwatch GlobalTime;
        public static Font Font = new Font("Segoe UI", 12F);
        private static float m_TSZ = 32F;
        public static int TSZ { get => (int)m_TSZ; set => m_TSZ = value; }
        public static Func<Point, Point> PointToClient { get; internal set; }
        public static Point MousePos => PointToClient(Cursor.Position);
        public static Random RND = new Random((int)DateTime.Now.Ticks);
        public static bool ShowInventory = false;
    }
}
