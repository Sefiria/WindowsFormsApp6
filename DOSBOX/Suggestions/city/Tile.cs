using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.city
{
    public class Tile
    {
        public static int TSZ => 8;
        public enum TYPE
        {
            GROUND = 0,
            SOLID,
        }

        public byte[,] Pixels;
        public TYPE Type;

        public byte this[int x, int y] => x < 0 || y < 0 || x > 7 || y > 7 ? (byte)0 : Pixels[x, y];
    }
}
