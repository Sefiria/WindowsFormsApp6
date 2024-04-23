using System;
using System.Drawing;
using System.Linq;
using Tooling;

namespace WindowsFormsApp28_px.matters.organic
{
    internal class Controllable : Matter
    {
        private float timer_display_look = 0F;

        public Controllable() : base()
        {
        }
        public Controllable(Point position, int diameter, int color) : base(position, diameter, color)
        {
        }
        public Controllable(float X, float Y, int diameter, int color) : base(X, Y, diameter, color)
        {
        }

        public override void Update()
        {
            Update_Movements();
            Update_Actions();
        }
        public void Update_Movements()
        {
            var (z, q, s, d) = KB.ZQSD();

            float x = this.x, y = this.y, a = A;

            if (q) a -= SA;
            if (d) a += SA;

            void move_forward(float speed)
            {
                var r = (1F, 0F).P().Rotate(a);
                x += r.X * speed;
                y += r.Y * speed;
            }

            if (z) move_forward(SM);
            if (s) move_forward(-SM);

            foreach (Segment segment in Common.Segments)
            {
                if (Maths.CollisionSegment(segment.A.X, segment.A.Y, segment.B.X, segment.B.Y, x, y, r))
                {
                    var n = segment.N;
                    x += n.X * SM;
                    y += n.Y * SM;
                }
            }

            var C1 = new Circle(this);
            C1.x += C1.diameter;
            foreach (Entity entity in Common.Matters.OfType<Entity>())
            {
                var box = entity;

                if (Maths.CollisionPointCercle(box.x, box.y, C1))
                    continue;
                if (Maths.CollisionPointCercle(box.x, box.y + box.h, C1))
                    continue;
                if (Maths.CollisionPointCercle(box.x + box.w, box.y, C1))
                    continue;
                if (Maths.CollisionPointCercle(box.x + box.w, box.y + box.h, C1))
                    continue;

                if (Maths.CollisionSegment(box.x + box.w, box.y, box.x, box.y, C1.x, C1.y + C1.r, C1.r))// segment top
                {
                    x += 0F;
                    y += -SM;
                }
                if (Maths.CollisionSegment(box.x + box.w, box.y + box.h, box.x + box.w, box.y, C1.x - C1.r, C1.y, C1.r))// segment right
                {
                    x += SM;
                    y += 0F;
                }
                if (Maths.CollisionSegment(box.x, box.y + box.h, box.x + box.w, box.y + box.h, C1.x, C1.y - C1.r, C1.r))// segment bottom
                {
                    x += 0F;
                    y += SM;
                }
                if (Maths.CollisionSegment(box.x, box.y, box.x, box.y + box.h, C1.x + C1.r, C1.y, C1.r))// segment left
                {
                    x += -SM;
                    y += 0F;
                }

                if (Maths.CollisionPointBox(C1.x, C1.y, box))// one inside the other
                    continue;
            }

            this.x = x;
            this.y = y;
            A = a;
        }
        public void Update_Actions()
        {
            if (KB.IsKeyDown(KB.Key.Space))
            {
                Common.Matters.Except(this).Where(e => e.Point.vecf().Distance(Point.vecf()) < diameter + SM * 4F).ToList().ForEach(e => e.Action(this));
            }
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);

            var anybt = KB.AnyZQSD() || KB.IsKeyDown(KB.Key.Space);
            if (timer_display_look > 0F || anybt)
            {
                timer_display_look -= 0.05F;
                float look_size = 24F;
                PointF look = PointF.Empty;
                var r = (1F, 0F).P().Rotate(A);
                look.X += r.X;
                look.Y += r.Y;

                var raycast = Maths.SimpleRaycastHit(Point, look, SM, look_size, Common.Segments);

                g.DrawLine(new Pen(Color.FromArgb((int)(byte.MaxValue * Math.Max(0F, timer_display_look)), Color.FromArgb(C)), 2F), Point, raycast.LastPoint);
                if (anybt)
                    timer_display_look = 1F;
            }

            if(Inventory.Contains(0x46))
            {
                PointF look = PointF.Empty;
                var r = (1F, 0F).P().Rotate(A);
                look.X += r.X;
                look.Y += r.Y;
                var raycast = Maths.SimpleRaycastHit(Point, look, SM, 10000F, Common.Segments);
                g.DrawLine(Pens.DarkRed, Point, raycast.LastPoint);
            }
        }
    }
}

// TODO euh raycast sur droites sauf que segments