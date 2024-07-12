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
        public float previous_look_x = 0F;
        public vec From = vec.Null;

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
            previous_look_x = copy.previous_look_x;
            From = copy.From;
        }
        public Fluid(float q)
        {
            Q = q;
        }
        public Fluid()
        {
            Q = 0F;
        }
        public void Reset()
        {
            Q = 0F;
            previous_look_x = 0F;
            From = vec.Null;
        }
    }
}
