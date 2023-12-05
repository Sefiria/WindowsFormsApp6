using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cast
{
    public abstract class Structural
    {
        public PointF _A, _B;
        public Color C;
        public virtual PointF A => _A;
        public virtual PointF B => _B;
        public Structural()
        {
        }
    }
}
