using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using Tooling;

namespace WindowsFormsApp28_px
{
    internal class Matter : Circle, IMatter
    {
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }

        private float m_A;

        public float A
        {
            get => m_A;
            set
            {
                m_A = value;
                while (m_A < 0F) m_A += 360F;
                while (m_A >= 360F) m_A -= 360F;
            }
        }
        public int C;

        public float SM = 1F, SA = 4F;
        public List<Item> Inventory { get; set; } = new List<Item>();

        public PointF Point => new PointF(x, y);

        public Matter() : base(0, 0, 0)
        {
            C = 0;
        }
        public Matter(Point position, int diameter, int color) : base(position.X, position.Y, diameter / 2)
        {
            C = color;
        }
        public Matter(float X, float Y, int diameter, int color) : base(X, Y, diameter / 2)
        {
            C = color;
        }

        public virtual void Update()
        {
        }

        public virtual void Action(IMatter triggerer)
        {
        }

        public virtual void Draw(Graphics g)
        {
            g.DrawEllipse(new Pen(Color.FromArgb(C)), x - r, y - r, diameter, diameter);
        }
        public virtual void DrawUI(Graphics g)
        {
        }
    }
}
