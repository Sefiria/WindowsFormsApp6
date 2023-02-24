using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCardsApp15.Utilities
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

        public static bool operator ==(vecf a, vecf b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(vecf a, vecf b) => a.x != b.x || a.y != b.y;
        public static vecf operator +(vecf a, vecf b) => new vecf(a.x + b.x, a.y + b.y);
        public static vecf operator -(vecf a, vecf b) => new vecf(a.x - b.x, a.y - b.y);
        public static vecf operator *(vecf a, float b) => new vecf(a.x * b, a.y * b);
        public static vecf operator /(vecf a, vecf b) => new vecf(Math.Round(b.x, 8) == 0 ? 0 : a.x / b.x, Math.Round(b.y, 8) == 0 ? 0 : a.y / b.y);
        public static vecf operator /(vecf a, float b) => new vecf(Math.Round(b, 8) == 0 ? 0 : a.x / b, Math.Round(b, 8) == 0 ? 0 : a.y / b);

        public override string ToString()
        {
            return $"(x:{x}, y:{y})";
        }
    }
}
