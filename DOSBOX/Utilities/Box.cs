using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Utilities
{
    public class Box
    {
        public float x, y, w, h;
        public Box(Disp d)
        {
            x = d.vec.x;
            y = d.vec.y;
            w = d._w;
            h = d._h;
        }
        public Box(Dispf d)
        {
            x = (int)d.vec.x;
            y = (int)d.vec.y;
            w = d._w;
            h = d._h;
        }
        public Box(Circle c)
        {
            x = c.r == 1 ? c.x : c.x - c.r;
            y = c.r == 1 ? c.y : c.y - c.r;
            w = c.r * 2;
            h = c.r * 2;
        }
    }
}
