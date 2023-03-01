using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Utilities
{
    public class vec
    {
        public int x, y;
        public vec() { x = y = 0; }
        public vec(vec v) { x = v.x; y = v.y; }
        public vec(int x, int y) { this.x = x; this.y = y; }
        public vecf vecf => new vecf(x, y);

        public static bool operator ==(vec a, vec b) => a?.x == b?.x && a?.y == b?.y;
        public static bool operator !=(vec a, vec b) => a?.x != b?.x || a?.y != b?.y;
        public static vec operator +(vec a, vec b) => new vec(a.x + b.x, a.y + b.y);
        public static vec operator -(vec a, vec b) => new vec(a.x - b.x, a.y - b.y);
        public static vec operator *(vec a, float b) => new vec((int)(a.x * b), (int)(a.y * b));
    }
}
