using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp13
{
    internal class Cam
    {
        public vecf vec;
        public float angle, fov = 10F, movespeed = 2F, turnspeed = 6F;
        public bool found = false;
        public static readonly int super_min = -500, super_max = 300;

        // seek -er
        public int SuperBack = super_min;
        // hide -er
        public int SuperHidden = super_min;

        public vecf look => angle.AngleToVecf();

        public Cam(float x = float.NaN, float y = float.NaN, float angle = 0F)
        {
            vec = new vecf(float.IsNaN(x) ? Core.Map.hw : x, float.IsNaN(y) ? Core.Map.hh : y);
            this.angle = angle;
        }

        public void Update()
        {
            while (angle < 0F) angle += 360;
            while (angle >= 360F) angle -= 360;

            if (SuperBack > super_min)
                SuperBack -= SuperBack >= 0 ? 2 : 1;

            if (SuperHidden > super_min)
                SuperHidden--;
        }

        internal void MoveForward()
        {
            if (found) return;
            if(!Collides(vec, new vecf(vec.x + look.x * movespeed * 2F, vec.y + look.y * movespeed * 2F)))
                vec += look * movespeed;
        }
        internal void MoveBackward()
        {
            if (found) return;
            if(!Collides(vec, new vecf(vec.x - look.x * movespeed * 2F, vec.y - look.y * movespeed * 2F)))
                vec -= look * movespeed;
        }
        internal bool Collides(vecf a, vecf b)
        {
            vecf loc = new vecf(vec);
            bool ko = false;
            for (double t = 0D; t <= 1D && !ko; t += 1D / (movespeed * 2F))
            {
                loc.x = Maths.Lerp(a.x, b.x, t);
                loc.y = Maths.Lerp(a.y, b.y, t);
                if (Core.Map.Image.GetPixel((int)loc.x, (int)loc.y).ToArgb() != Color.Black.ToArgb())
                    ko = true;
            }
            return ko;
        }

        internal void TurnLeft()
        {
            if (found) return;
            angle -= turnspeed;
        }
        internal void TurnRight()
        {
            if (found) return;
            angle += turnspeed;
        }

        public void Draw(Graphics g, Color c)
        {
            float circlesz = 10F;
            if(!found)
                g.DrawLine(new Pen(c, 2F), vec.x, vec.y, vec.x + look.x * circlesz, vec.y + look.y * circlesz);
            g.DrawEllipse(new Pen(c), vec.x - circlesz / 2F, vec.y - circlesz / 2F, circlesz, circlesz);
        }

        public void GoSuperBack()
        {
            if (SuperBack == super_min)
                SuperBack = super_max;
        }
        public bool IsSuperBack() => SuperBack >= 0;

        public void GoSuperHidden()
        {
            if (SuperHidden == super_min)
                SuperHidden = super_max;
        }
        public bool IsSuperHidden() => SuperHidden >= 0;
    }
}
