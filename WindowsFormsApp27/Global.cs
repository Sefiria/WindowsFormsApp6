using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp27
{
    internal class Global
    {
        public class UserSettings
        {
            public bool BOOL_PLAYER_AVOID_PROBLEMS = true;
        }

        public static float SPEED_SCALE = 1F;
        public static int MAX_FRUITS = 10;
        public static float SPEED_GROW = 1F;
        public static int FRUIT_TAKE_COOLDOWN_TICKS = 50;
        public static UserSettings Settings = new UserSettings();
    }
}
