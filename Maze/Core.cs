using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public static class Core
    {
        public static Graphics g;
        public static int X, Y, W, H;
        public static Point Renderize(this Point pos) => new Point(pos.X - X, pos.Y - Y);
        public static bool FormFocused = true;
        public static Point FormPosition;
        public static Stopwatch GlobalTime, MatchTime;
        public static Font Font = new Font("Segoe UI", 12F);
        private static float m_TSZ = 32F;
        public static int TSZ { get => (int)m_TSZ; set => m_TSZ = value; }
    }
}
