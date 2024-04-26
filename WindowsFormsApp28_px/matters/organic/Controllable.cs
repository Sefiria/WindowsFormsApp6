using System;
using System.Data;
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
                var nears = Common.Matters.Except(this).Where(e => e.Point.vecf().Distance(Point.vecf()) < diameter + SM * 4F).ToList();
                nears.ForEach(e => e.Action(this));
                if (KB.IsKeyPressed(KB.Key.Space) && Inventory.Contains(DB.Tex.T46) && nears.Count == 0)
                    Shot();
            }
        }
        public void Shot()
        {
            vecf look;
            int color;
            int side_diffusion = RandomThings.arnd(20, 40);
            var position = Point.Plus(Look.x(10F));
            for (int i = 0; i < 10; i++)
            {
                look = Look.Rotate(RandomThings.arnd(-side_diffusion, side_diffusion)).vecf();
                color = (RandomThings.rnd(10) < 5 ? Color.Orange : Color.Yellow).ToArgb();
                ParticlesManager.Particles.Add(new Particle(position.vecf() + look * 24, 2F, look, 10F, color, 40));
            }

            Common.Bullets.Add(new Bullet(position, A, 6, 8F, Color.White.ToArgb(), 200));
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);

            //var anybt = KB.AnyZQSD() || KB.IsKeyDown(KB.Key.Space);
            //if (timer_display_look > 0F || anybt)
            //{
            //    timer_display_look -= 0.05F;
            //    float look_size = 24F;
            //    var raycast = Maths.SimpleRaycastHit(Point, Look, SM, look_size, Common.Segments);

            //    g.DrawLine(new Pen(Color.FromArgb((int)(byte.MaxValue * Math.Max(0F, timer_display_look)), Color.FromArgb(C)), 2F), Point, raycast.LastPoint);
            //    if (anybt)
            //        timer_display_look = 1F;
            //}

            //if (Inventory.Contains(DB.Tex.T46))
            //{
            //    var raycast = Maths.SimpleRaycastHit(Point, Look, SM, 10000F, Common.Segments);
            //    g.DrawLine(Pens.DarkRed, Point, raycast.LastPoint);
            //}
        }
    }
}
