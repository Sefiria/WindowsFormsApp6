using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling
{
    public class Segment
    {
        private PointF m_A, m_B;
        public PointF A
        {
            get => m_A;
            set
            {
                m_A = value;
                N = Maths.Normale(m_A, m_B);
                Angle = Maths.GetAngle(m_B.MinusF(m_A));
            }
        }
        public PointF B
        {
            get => m_B;
            set
            {
                m_B = value;
                N = Maths.Normale(m_A, m_B);
                Angle = Maths.GetAngle(m_B.MinusF(m_A));
            }
        }
        public PointF N;
        public float Angle;
        public Segment()
        {
        }
        public Segment(PointF A, PointF B)
        {
            this.A = A;
            this.B = B;
        }
        public Segment(float ax, float ay, float bx, float by)
        {
            A = new PointF(ax, ay);
            B = new PointF(bx, by);
        }

        public void Draw(Graphics g)
        {
            g.DrawLine(Pens.White, A, B);
        }
    }
}
