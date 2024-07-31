using DOSBOX.Suggestions.fusion.Triggerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.fusion
{
    public class Register
    {
        public static List<string> mobs_killed = new List<string>();
        public static List<string> open_doors = new List<string>();
        public static List<string> triggered_objects = new List<string>();

        public static void Reset()
        {
            mobs_killed.Clear();
            open_doors.Clear();
            triggered_objects.Clear();
        }

        public static void Write(object obj)
        {
            switch(obj)
            {
                case Mob m: mobs_killed.Add(m.BaseHash); break;
                case Door d: open_doors.Add(d.BaseHash); break;
                case ITriggerable o: triggered_objects.Add(o.BaseHash); break;
            }
        }
    }
}
