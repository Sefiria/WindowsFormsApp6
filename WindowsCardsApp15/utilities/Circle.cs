using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCardsApp15.Utilities
{
    public class Circle
    {
        public float x, y, r;
        public Circle(float x, float y, float r)
        {
            this.r = r;
            this.x = (float)Math.Ceiling(x);
            this.y = (float)Math.Ceiling(y);
        }
    }
}
