using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp3.Entities;
using WindowsFormsApp3.Entities.Pentities;
using WindowsFormsApp3.Properties;
using WindowsFormsApp3.SpecialTypes;

namespace WindowsFormsApp3.Entities.Weapons
{
    public class Gun : Weapon
    {
        static Bitmap Image = null;

        public Gun(DrawableEntity Source) : base(Source)
        {
            if(Image == null)
            {
                Image = Resources.simple_bullet;
                Image.MakeTransparent();
            }
        }

        public override void Action()
        {
            new Pentity(Source, Source.X, Source.Y, Image, Source.Look);
        }
    }
}
