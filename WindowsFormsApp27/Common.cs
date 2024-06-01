using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp27
{
    internal class Common
    {
        public static long ticks = 0L, sim_ticks = 0L;
        public static bool IsSimRuning = false;
        public static Font minifont = new Font("Courrier New", 8F);
        public static Font font = new Font("Courrier New", 12F);
        public static int sim_births = 0, sim_deaths = 0;
    }
}
