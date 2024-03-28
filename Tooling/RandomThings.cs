using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public static int advanced_rnd(int min, int max, bool max_included = true) => min + Common.Rnd.Next((max + (max_included ? 1 : 0)) - min);
        public static int arnd(int min, int max, bool max_included = true) => advanced_rnd(min, max, max_included);

        public static float rnd(float min, float max_excluded) => rnd((int)(min * 1000), (int)(max_excluded * 1000)) / 1000f;
        public static float rnd1() => rnd(100001) / 100001f;
        public static float rnd1Around0() => rnd(200001) / 100001f - 1f;
    }
}