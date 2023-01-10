using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp10
{
    public static class Tools
    {
        public static Random RND = new Random((int)DateTime.Now.Ticks);
        public static int GetRnd(int max) => RND.Next(max);
        public static int GetRnd(int min, int max) => RND.Next(min, max);
    }
}
