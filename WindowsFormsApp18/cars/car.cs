using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using WindowsFormsApp18.utilities;
using WindowsFormsApp18.Utilities;

namespace WindowsFormsApp18.cars
{
    public class car : Box
    {
        static List<Color> unused_colors = new List<Color>() { Color.Red, Color.Lime, Color.Yellow, Color.Cyan, Color.Orange, Color.Magenta };
        static List<Color> used_colors = new List<Color>();
        Color c;
        Pen m_pen = null;
        Pen pen => m_pen ?? (m_pen = new Pen(c));
        Angle angle = new Angle(Angle.Up);
        Bitmap Image;
        float turnSpeed = 6F, moveSpeed = 4F;

        public car(float x, float y, float w, float h) : base(x, y, w, h)
        {
            c = unused_colors[Core.RND.Next(unused_colors.Count)];
            unused_colors.Remove(c);
            used_colors.Add(c);

            int diagonal = (int)Maths.Length(new vecf(w, h));
            Image = new Bitmap(diagonal + 2, diagonal + 2);
            using (var g = Graphics.FromImage(Image))
            {
                g.DrawRectangle(pen, 1 + (diagonal - w) / 2, 1 + (diagonal - h) / 2, w, h);
                g.FillRectangle(new SolidBrush(c), 1 + (diagonal - w) / 2, 1 + (diagonal - h) / 2, w, h / 4);
            }
        }

        public void Update()
        {
            int id = Data.Instance.cars.IndexOf(this);
            //MoveForward();
            if (KB.IsKeyDown(id == 0 ? KB.Key.Z : KB.Key.NumPad8))
                MoveForward();
            if (KB.IsKeyDown(id == 0 ? KB.Key.S : KB.Key.NumPad2))
                vec += Maths.Rotate(new vecf(0F, moveSpeed / 2), angle.Value);

            if (KB.IsKeyDown(id == 0 ? KB.Key.Q : KB.Key.NumPad4))
                angle.Value -= turnSpeed;
            if (KB.IsKeyDown(id == 0 ? KB.Key.D : KB.Key.NumPad6))
                angle.Value += turnSpeed;
        }
        public void Display()
        {
            Core.g.DrawImage(Image.Rotated(angle), vec.x - Math.Max(w, h) / 2, vec.y - Math.Max(w, h) / 2);
            //var l = new vecf(0F, -1F);
            //float a = angle.Value;
            //var r = Maths.Rotate(l, a);
            //var v = new vecf(vec.x + r.x * Math.Max(w, h) / 2, vec.y + r.y * Math.Max(w, h) / 2);
            //Core.g.DrawEllipse(Pens.Lime, v.x, v.y, 2, 2);
        }
        public void MouseInput(MouseEventArgs e, bool up = false)
        {
        }
        public void MouseWheel(MouseEventArgs e)
        {
        }

        Mur __m = null;
        void MoveForward()
        {
            var l = new vecf(0F, -1F);
            float a = angle.Value;
            var r = Maths.Rotate(l, a);
            var v = new vecf(vec.x + r.x * Math.Max(w, h) / 2, vec.y + r.y * Math.Max(w, h) / 2);
            float s = moveSpeed;
            bool c = false;
            Mur mur = null;
            for (int i = 0; i < moveSpeed && !c; i++)
            {
                if ((mur = Collides(v, r)) != null)
                {
                    c = true;
                    bool u = Maths.IsPointLeftToSegment(mur.A, mur.B, v);
                        Console.WriteLine(u);
                    vec += mur.Normale * 5F * (Maths.IsPointLeftToSegment(mur.A, mur.B, v) ? 1F : -1F);
                }
                else
                    vec += r;
            }
        }

        private Mur Collides(vecf v, vecf look)
        {
            var rc = Maths.Raycast(v, look, Core.Image, Color.White);
            var murs = Data.Instance.map.murs.Where(m => Maths.CollisionSegment(m.A.x, m.A.y, m.B.x, m.B.y, rc.x, rc.y, 10F));

            foreach(var mur in murs)
            {
                if (Maths.CollisionBoxSegment(this, mur.A, mur.B))
                    return mur;
            }

            return null;
        }
    }
}
