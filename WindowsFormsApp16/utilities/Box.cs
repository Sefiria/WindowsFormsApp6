using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp16.Utilities
{
    public class Box
    {
        public float x, y, w, h;
        public Box(Circle c)
        {
            x = c.r == 1 ? c.x : c.x - c.r;
            y = c.r == 1 ? c.y : c.y - c.r;
            w = c.r * 2;
            h = c.r * 2;
        }
    }
}
