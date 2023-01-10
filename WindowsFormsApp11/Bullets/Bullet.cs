using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Bullets
{
    public abstract class Bullet : Entity
    {
        public bool Destroy = false;
        public float SpeedMove = 0.5F;

        public int hW => W / 2;
        public int hH => H / 2;

        public float LookX = 0F, LookY = -1F;

        public Bullet(float x, float y, float lookx, float looky, int w = 5, int h = 5, float spdmv = 6F)
        {
            X = x;
            Y = y;
            LookX = lookx;
            LookY = looky;
            W = w;
            H = h;
            SpeedMove = spdmv;

            Var.Data.Entities.Add(this);
        }

        public new virtual void Update()
        {
            if (!Destroy)
            {
                Var.Data.Travelers.Where(t => Maths.IsColliding(this, t))
                                            .ToList()
                                            .ForEach(t => { t.Destroy = true; Destroy = true; });
            }

            if (Destroy)
                Var.Data.Entities.Remove(this);
        }

        public new virtual void Draw()
        {
            Var.g.FillEllipse(Brushes.Red, X - hW, Y - hW, W, H);
        }
    }
}
