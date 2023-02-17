using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Utilities
{
    public class vecf
    {
        public float x, y;
        public vecf() { x = y = 0F; }
        public vecf(int x, int y) { this.x = x; this.y = y; }
        public vecf(float x, float y) { this.x = x; this.y = y; }
        public vec vec => new vec((int)x, (int)y);

        public static vecf Zero => new vecf(0F, 0F);

        public static bool operator ==(vecf a, vecf b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(vecf a, vecf b) => a.x != b.x || a.y != b.y;
    }
}
