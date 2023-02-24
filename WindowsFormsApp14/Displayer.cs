using DOSBOX.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace WindowsFormsApp14
{
    internal class Displayer
    {
        struct pt
        {
            public float x;
            public float d;
            public Color c;
        }

        public static void Display(Graphics g, Map m, Cam c)
        {
            g.Clear(Color.Black);

            List<pt> pts = new List<pt>();

            Bitmap minmp = new Bitmap(m.w, m.h);
            Graphics minmp_g = Graphics.FromImage(minmp);

            var look = new vecf(c.look);

            minmp_g.DrawImage(m.Image, 0, 0);

            vecf s = new vecf(c.vec);
            for (float fov = -c.fov; fov <= c.fov; fov+=5)
            {
                vecf fovlook = look.Rotate(fov);
                vecf e = Maths.GetRaycastLine(s, fovlook, 0, 0, m.w, m.h);
                vecf hit = vecf.Zero;
                Color col;
                double length = new Vector(e.x - s.x, e.y - s.y).Length;
                //for (double t = 0D; t <= 1D && hit == vecf.Zero; t += length)
                //{
                (hit, col) = Maths.Raycast(s, fovlook, m.Image, Color.Black);
                minmp_g.DrawLine(Pens.DarkCyan, s.x, s.y, (hit != vecf.Zero ? hit.x : e.x), (hit != vecf.Zero ? hit.y : e.y));
                if (hit != vecf.Zero)
                {
                    vecf sfov = Maths.GetRaycastLine(s, look.Rotate(-c.fov), 0, 0, m.w, m.h);
                    vecf efov = Maths.GetRaycastLine(s, look.Rotate(c.fov), 0, 0, m.w, m.h);
                    float hitd = Maths.Distance(s, hit);
                    float x = Maths.Distance(s + sfov * hitd, hit) / Maths.Distance(s + sfov * hitd, s + efov * hitd);
                    float d = Maths.Distance(s, hit) / Maths.Distance(s, e);
                    pts.Add(new pt{x = x,d = d,c= col});
                    //Console.WriteLine($"{Maths.Distance(s, hit)} {Maths.Distance(s, e)} {Maths.Distance(s, hit) / Maths.Distance(s, e)}");
                }
                //}
            }
            //Console.WriteLine();
            //Console.WriteLine();

            //pt pt;
            //int count = pts.Count;
            //bool mt() => pts.Count == 0;
            //vecf sfov = Maths.GetRaycastLine(c.vec + look * 20F, look.Rotate(-c.fov), 0, 0, m.w, m.h);
            //vecf efov = Maths.GetRaycastLine(c.vec + look * 20F, look.Rotate(c.fov), 0, 0, m.w, m.h);
            //float lght = Maths.Distance(sfov, efov);
            //Brush wall_tex = Brushes.White;
            //Pen wall_bounds = new Pen(Color.Blue, 4F);
            //while (/*lght > 0F &&*/ !mt())
            //{
                //pt = pts.Dequeue();
                //vecf v = ((pt.v - sfov) / lght) * m.w;
                //if (count == 1)
                //{
                //    g.FillRectangle(wall_tex, 0, pt.d * m.h, v.x, m.h - pt.d * m.h);
                //    //g.DrawRectangle(wall_bounds, 0, pt.d * m.h, x, m.h - pt.d * m.h);
                //}
                //else if(mt())
                //{
                //    g.FillRectangle(wall_tex, v.x, pt.d * m.h, m.w, m.h - pt.d * m.h);
                //    //g.DrawRectangle(wall_bounds, x, pt.d * m.h, m.w, m.h - pt.d * m.h);
                //}
                //else
                //{
                    //pt n = pts.ToArray()[0];
                    //g.FillRectangle(wall_tex, pt.x, 0, );
                    //g.DrawRectangle(wall_bounds, x, pt.d * m.h, w, m.h - pt.d * m.h);
                //}
            //}
            if (pts.Count > 1)
            {
                //var path = new GraphicsPath();
                for (int i = 0; i < pts.Count && i + 1 < pts.Count; i++)
                    g.FillRectangle(new SolidBrush(pts[i].c), pts[i].x * m.w, m.h / 2F - (1F - pts[i].d) * m.hh, pts[i + 1].x * m.w, m.h / 2F + (1F - pts[i].d) * m.hh);
                //path.AddRectangle(new RectangleF(pts[i].x * m.w, m.h / 2F - pts[i].d * m.hh, pts[i + 1].x * m.w, m.h / 2F + pts[i].d * m.hh));
                //g.FillPath(wall_tex, path);
            }


            int mnmpszs = (int)(m.w / 4F);
            g.DrawImage(minmp, 0, 0, mnmpszs, mnmpszs);
            g.DrawRectangle(Pens.Yellow, 0, 0, mnmpszs, mnmpszs);
        }
    }
}
