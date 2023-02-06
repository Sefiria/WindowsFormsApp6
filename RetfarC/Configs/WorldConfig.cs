using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC
{
    public class WorldConfig
    {
        public static int MaxResourceCount => 10;
        public static int MaxTicks => 16;
        public static int MaxTimerResource => 200;
        public static int TileWidth = 16, TileHeight = 16;

        // total should be 100
        public static int RatioGrass = 80;
        public static int RatioStone = 10;
        public static int RatioSand = 10;
    }
}
