using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Utilities
{
    public class vec
    {
        public int x, y;
        public vec() { x = y = 0; }
        public vec(int x, int y) { this.x = x; this.y = y; }
        public vecf vecf => new vecf(x, y);
    }
}
