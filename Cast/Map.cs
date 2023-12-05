using Cast.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Cast
{
    public class Map
    {
        Bitmap imgmini;
        public Graphics gmini;

        public List<Structural> Structures = new List<Structural>()
        {
            new Wall(new Point(-100, -100), new Point(-10, -100)) { C = Color.Red },
            new Wall(new Point(10, -100), new Point(100, -100)) { C = Color.Red },
            new Wall(new Point(100, -100), new Point(100, 100)) { C = Color.Green },
            new Wall(new Point(100, 100), new Point(-100, 100)) { C = Color.Blue },
            new Wall(new Point(-100, 100), new Point(-100, -100)) { C = Color.Yellow },

            new Door(new Point(-10, -100), new Point(10, -100)),
        };

        public Map()
        {
        }

        private void ResetMini()
        {
            imgmini = new Bitmap(Core.iRW, Core.iRH);
            gmini = Graphics.FromImage(imgmini);
        }

        public void Update()
        {
            Structures.Where(x => x is Door).Cast<Door>().ToList().ForEach(x => x.Update());
            Core.Entities.Update();
        }

        public void Draw(Graphics g)
        {
            DrawMap(g);
            DrawUI(g);
            Core.Entities.DrawMinimap(g);
        }
        public void DrawMap(Graphics g)
        {
            g.FillRectangle(Brushes.DimGray, 0, Core.hh, Core.RW, Core.hh);


            PointF forward;
            float anglemax = Core.Cam.FOV / 2, angle = -anglemax;
            float x, y, nearval, near;
            Brush b;
            Structural S = null;
            Entity E = null;


            draw_entities();

            forward = Maths.Rotate(Core.Cam.Forward, angle);
            while (angle < anglemax)
            {
                var result = RayCastTool.RayCast(Core.Cam.Near, forward);
                S = result.Structure;
                //E = result.Entity;
                //if (E != null && (S == null || Maths.Distance(E.Position, Core.Cam.Position) < Math.Min(Maths.Distance(S.A, Core.Cam.Position), Maths.Distance(S.B, Core.Cam.Position))))
                //{
                //    nearval = 1F - Maths.Length(result.ContactPoint.Minus(Core.Cam.Near).x((float)Math.Cos((angle).ToRadians())).DivF(Core.Cam.Far));
                //    if (nearval < 0F) nearval = 0F;
                //    x = Core.hw + (angle / Core.Cam.FOV) * Core.hw;
                //    Console.WriteLine(angle);
                //    y = Core.hh * 1F - E.Bounds.Height / 2;
                //    b = new SolidBrush(Color.FromArgb(Math.Min((int)(E.C.R * nearval), 255), Math.Min((int)(E.C.G * nearval), 255), Math.Min((int)(E.C.B * nearval), 255)));
                //    var r = new RectangleF(x, y, 12, nearval * E.Bounds.Height * 10F);
                //    g.FillRectangle(b, r);
                //}

                if (S != null)
                {
                    nearval = 1F - Maths.Length(result.ContactPoint.Minus(Core.Cam.Near).x((float)Math.Cos((angle).ToRadians())).DivF(Core.Cam.Far));
                    if (nearval < 0F) nearval = 0F;
                    near = nearval * Core.hh * 0.9F;
                    x = Core.hw + (angle / Core.Cam.FOV) * Core.RW;
                    y = Core.hh * 1.2F - near;
                    b = new SolidBrush(Color.FromArgb(Math.Min((int)(S.C.R * nearval), 255), Math.Min((int)(S.C.G * nearval), 255), Math.Min((int)(S.C.B * nearval), 255)));
                    g.FillRectangle(b, x, y, 18*3, near * 2);
                }

                angle+=3;
                forward = Maths.Rotate(forward, 1);
            }
        }
        private void draw_entities()
        {
            float anglemax = Core.Cam.FOV / 2;
            var list = RayCastTool.RayCastEntities(Core.Cam.Near, Core.Cam.Forward, -anglemax, anglemax);
            foreach(var kv in list)
            {
                kv.Entity.Draw(Core.g, new PointF(kv.LineX, Core.CenterPoint.Y));
            }
        }

        public void DrawUI(Graphics g)
        {
        }
        public void prepare_minimap()
        {
            ResetMini();
            var _g = gmini;
            _g.DrawLine(new Pen(Color.DimGray, 5F), Core.CenterPoint, Core.Cam.LookFovLeft);
            _g.DrawLine(new Pen(Color.DimGray, 5F), Core.CenterPoint, Core.Cam.LookFovRight);
            _g.DrawLine(new Pen(Color.DimGray, 5F), Core.Cam.LookFovLeft, Core.Cam.LookFovRight);
            Structures.ForEach(x => _g.DrawLine(new Pen(Color.White, 3F), Core.CenterPoint.PlusF(x.A.Minus(Core.Cam.Position)), Core.CenterPoint.PlusF(x.B.Minus(Core.Cam.Position))));
            _g.FillEllipse(Brushes.Yellow, Core.CenterPoint.X - 5, Core.CenterPoint.Y - 5, 10, 10);

        }
        public void draw_minimap(Graphics g)
        {
            g.DrawImage(new Bitmap(imgmini, new Size(100, 100)), Core.iRW - 100, 1);
            g.DrawRectangle(Pens.Gray, Core.iRW - 101, 0, 100, 100);
        }

        public bool InWorldLimits(PointF p)
        {
            var c = Core.Cam;
            var cp = c.Position;
            return !(
                p.X < cp.X - Core.hw
             || p.Y < cp.Y - Core.hh
             || p.X >= cp.X + Core.hw
             || p.Y >= cp.Y + Core.hh
             );
        }
    }
}
