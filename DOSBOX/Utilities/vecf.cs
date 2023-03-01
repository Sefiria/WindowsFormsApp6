using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Utilities
{
    public class vecf
    {
        public float x, y;
        public vecf() { x = y = 0F; }
        public vecf(vecf v) { x = v.x; y = v.y; }
        public vecf(int x, int y) { this.x = x; this.y = y; }
        public vecf(float x, float y) { this.x = x; this.y = y; }
        public vec vec => new vec((int)x, (int)y);
        public vec i => vec;
        public PointF pt => new PointF(x, y);

        public static vecf Zero => new vecf(0F, 0F);

        public static bool operator ==(vecf a, vecf b) => a?.x == b?.x && a?.y == b?.y;
        public static bool operator !=(vecf a, vecf b) => a.x != b.x || a.y != b.y;
        public static vecf operator +(vecf a, vecf b) => new vecf(a.x + b.x, a.y + b.y);
        public static vecf operator -(vecf a, vecf b) => new vecf(a.x - b.x, a.y - b.y);
        public static vecf operator *(vecf a, float b) => new vecf(a.x * b, a.y * b);
    }
}
