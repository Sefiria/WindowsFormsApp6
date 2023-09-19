using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PishConverter
{
    internal class Global
    {
        public static Graphics g;
        public static int W, H;
        public static Random RND = new Random((int)DateTime.UtcNow.Ticks);
        public static Vector2 SpaceParticlesLook = new Vector2(-1, 0);
    }
}
