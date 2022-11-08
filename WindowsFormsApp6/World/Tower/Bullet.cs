using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.World.Entities;

namespace WindowsFormsApp6.World.Tower
{
    public class Bullet
    {
        public bool Destroy = false;
        public int DMG, SPD;
        public float BX, BY, X, Y;
        public TowerUnit Target;
        public Bitmap Image;

        private float t = 0F;

        public Bullet(float x, float y, TowerUnit target, int dmg, int spd, int ResourceType = 0)
        {
            BX = X = x;
            BY = Y = y;
            Target = target;
            DMG = dmg;
            SPD = spd;
            switch(ResourceType)
            {
                default:
                case 0: Image = Resources.bullet_0; break;
            }
            Image.MakeTransparent();
        }
        public void Draw()
        {
            Core.g.DrawImage(Image, X, Y);
        }

        public void Update()
        {
            if(Target == null || Target.Destroy)
            {
                Destroy = true;
                return;
            }

            if (t < 1F)
            {
                X = Maths.Lerp(BX, Target.X, t);
                Y = Maths.Lerp(BY, Target.Y, t);
                t += SPD / Maths.Distance(X, Y, Target.X, Target.Y);
            }

            if(t >= 1F)
            {
                Target.HP -= DMG;
                Destroy = true;
            }
        }
    }
}
