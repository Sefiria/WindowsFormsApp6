using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp11.Travelers
{
    public abstract class Traveler : Entity
    {
        public bool Destroy = false;
        public float SpeedMove = 0.5F;
        public float HP = 1;
        public abstract ItemPackage Loot { get; set; }

        public int hW => W / 2;
        public int hH => H / 2;

        public Traveler(float x = -1F, float y = -1F, int w = 10, int h = 10, float spdmv = 0.5F)
        {
            W = w;
            H = h;
            X = x != -1F ? x : Var.Rnd.Next(hW, Var.W - hW);
            Y = y != -1F ? y : -H;
            SpeedMove = spdmv;

            Var.Data.Entities.Add(this);
        }

        public new virtual void Update()
        {
            if (Destroy)
            {
                DropLoot();
                Var.Data.Entities.Remove(this);
            }
        }

        public new virtual void Draw()
        {
            Var.g.DrawRectangle(Pens.LightGray, X - hW, Y - hW, W, H);
        }


        private void DropLoot()
        {
            new ItemPackageBlock(X, Y, Loot);
        }
    }
}
