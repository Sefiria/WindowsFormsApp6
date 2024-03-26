using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core
{
    public class Vec
    {
        public int x, y;

        public Vec(Vec vec) { this.x = vec.x; this.y = vec.y; }
        public Vec(int x, int y) { this.x = x; this.y = y; }

        public static Vec Empty => new Vec(0, 0);

        public static Vec operator +(Vec A, Vec B) => new Vec(A.x + B.x, A.y + B.y);
        public static bool operator ==(Vec A, Vec B) => A.x == B.x && A.y == B.y;
        public static bool operator !=(Vec A, Vec B) => A.x != B.x || A.y != B.y;
        public override string ToString()
        {
            return $"({x}, {y})";
        }
    };
}
