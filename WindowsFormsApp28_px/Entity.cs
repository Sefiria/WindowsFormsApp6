using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Tooling;

namespace WindowsFormsApp28_px
{
    internal class Entity : Box, IMatter
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

        public float SM = 2F, SA = 4F;
        public List<Item> Inventory { get; set; } = new List<Item>();

        public PointF Point => new PointF(x, y);

        public Entity() : base(0,0,0,0)
        {
        }
        public Entity(Point position, Size size, float angle, int color) : base(position.X, position.Y, size.Width, size.Height)
        {
            A = angle;
            C = color;

        }
        public Entity(float X, float Y, int W, int H, float angle, int color) : base(X, Y, W, H)
        {
            A = angle;
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
            var w = this.w / 2F;
            var h = this.h / 2F;
            PointF TopLeft = (-w, -h).P();
            PointF TopRight = (w, -h).P();
            PointF BottomRight = (w, h).P();
            PointF BottomLeft = (-w, h).P();

            var path = new GraphicsPath();
            path.StartFigure();
            path.AddLine(TopLeft, TopRight);
            path.AddLine(TopRight, BottomRight);
            path.AddLine(BottomRight, BottomLeft);
            path.AddLine(BottomLeft, TopLeft);
            path.CloseFigure();

            Matrix matrix = new Matrix();
            matrix.Translate(x, y);
            matrix.Rotate(A);
            path.Transform(matrix);

            var c = Color.FromArgb(C);
            var b = new HatchBrush(HatchStyle.WideDownwardDiagonal, c, Color.FromArgb(c.R / 2, c.G / 2, c.B / 2));

            g.FillPath(b, path);
        }

        public virtual void DrawUI(Graphics g)
        {
        }
    }
}
