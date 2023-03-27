using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp15.Utilities
{
    public class Circle
    {
        public float x, y, r;
        public Circle(Dispf d)
        {
            r = Math.Max(d._w, d._h) / 2F;
            x = (float)Math.Ceiling(d.vec.x);
            y = (float)Math.Ceiling(d.vec.y);
        }
        public Circle(float x, float y, float r)
        {
            this.r = r;
            this.x = (float)Math.Ceiling(x);
            this.y = (float)Math.Ceiling(y);
        }
        public Circle(float x, float y, int r)
        {
            this.r = r;
            this.x = (float)Math.Ceiling(x);
            this.y = (float)Math.Ceiling(y);
        }
    }
}
