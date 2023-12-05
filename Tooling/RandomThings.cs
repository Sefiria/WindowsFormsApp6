using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling
{
    public static class RandomThings
    {
        public static int rndbyte_int => Common.Rnd.Next(byte.MinValue, byte.MaxValue);
        public static Color rndbyte_color => Color.FromArgb(rndbyte_int, rndbyte_int, rndbyte_int);
        public static int rnd(int min, int max) => Common.Rnd.Next(min, max);
        public static int rnd(int max) => rnd(0, max);
        public static int rnd(float int_max) => rnd(0, (int)int_max);
    }
}
