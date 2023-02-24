using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCardsApp15.Utilities
{
    public class vec
    {
        public int x, y;
        public vec() { x = y = 0; }
        public vec(int x, int y) { this.x = x; this.y = y; }
        public vecf vecf => new vecf(x, y);

        public static vec Zero => new vec(0, 0);

        public static bool operator ==(vec a, vec b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(vec a, vec b) => a.x != b.x || a.y != b.y;
        public static vec operator +(vec a, vec b) => new vec(a.x + b.x, a.y + b.y);
        public static vec operator -(vec a, vec b) => new vec(a.x - b.x, a.y - b.y);
        public static vec operator *(vec a, int b) => new vec(a.x * b, a.y * b);
        public static vec operator /(vec a, vec b) => new vec(b.x == 0 ? 0 : a.x / b.x, b.y == 0 ? 0 : a.y / b.y);
        public static vec operator /(vec a, int b) => new vec(b == 0 ? 0 : a.x / b, b == 0 ? 0 : a.y / b);

        public override string ToString()
        {
            return $"(x:{x}, y:{y})";
        }
    }
}
