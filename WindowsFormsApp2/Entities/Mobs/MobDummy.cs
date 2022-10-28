using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Interfaces;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2.Entities.Mobs
{
    public class MobDummy : MobBase
    {
        Knife Knife = new Knife(MaterialQuality.Wood);
        public MobDummy(int TX = 0, int TY = 0) : base(Resources.mob_dummy.MadeTransparent(), TX * SharedCore.TileSize, TY * SharedCore.TileSize)
        {
            HPMax = HP = 5;
        }

        public override void Update()
        {
            base.Update();

            if((Look.X != 0 || Look.Y != 0) && Tools.RND.Next(10) == 5)
            {
                Knife.Throw(this);
            }
        }

        public override void Hit(IDamager damager)
        {
            HP -= damager.Damage;
            if (HP <= 0)
            {
                HP = 0;
                Exists = false;
            }
        }
    }
}
