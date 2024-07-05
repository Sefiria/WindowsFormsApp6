using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp28
{
    public class Fluid
    {
        public static float GraphicalDeathSpeed = 0.05F;
        protected float _Q, _LF;
        public vecf Look;
        public float Q
        {
            get => _Q;
            set
            {
                _Q = value;
                if (Q > 0F) LF = 1F;
            }
        }
        public float LF
        {
            get => _LF;
            set
            {
                _LF = value;
                if (_LF < 0F) _LF = 0F;
            }
        }
        public Fluid(Fluid copy)
        {
            Q = copy.Q;
        }
        public Fluid(float q)
        {
            Q = q;
        }
        public Fluid()
        {
            Q = 0F;
        }
    }
}
