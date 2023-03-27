using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15.utilities.tiles
{
    internal class Conveyor
    {
        public static Dictionary<Way, anim> conveyors = null;
        public static void Init()
        {
            conveyors = AnimRes.conveyors;
        }
        public Conveyor()
        {
            if (conveyors == null)
                Init();
        }
        public void Display(vecf where)
        {

        }
    }
}
