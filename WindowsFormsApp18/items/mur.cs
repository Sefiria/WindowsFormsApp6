using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp18.Utilities;

namespace WindowsFormsApp18.items
{
    internal class mur : Box
    {
        public mur(vecf loc, vecf sz) : base(loc.x, loc.y, sz.x, sz.y)
        {
        }
        public mur(vec loc, vec sz) : base(loc.x, loc.y, sz.x, sz.y)
        {
        }
        public mur(float x, float y, float w, float h) : base(x, y, w, h)
        {
        }
    }
}
