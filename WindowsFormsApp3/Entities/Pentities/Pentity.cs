using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp3.Entities.Mobs;
using WindowsFormsApp3.Interfaces;

namespace WindowsFormsApp3.Entities.Pentities
{
    public class Pentity : DrawableEntity
    {
        public Entity From;
        public Vecf VectorSpeed = new Vecf();
        public const float Speed = 5F;
        public float Angle = 0F;
        private Bitmap BaseImage;

        public Pentity(Entity From, float X, float Y, Bitmap Image, Vecf VectorSpeed)
        {
            this.From = From;
            this.X = X;
            this.Y = Y;
            BaseImage = this.Image = Image;
            this.VectorSpeed = VectorSpeed;
            Angle = Maths.AngleFromLook(VectorSpeed);
            Image = BaseImage.Rotated(Angle);
        }
        public override void Update()
        {
            var collider = SharedData.Entities.FirstOrDefault(x => x != this && x is MobBase && x.Collides(this));
            if (collider != null)
            {
                Exists = false;
                (collider as IRP)?.Hit(From as IRP);
            }
            else
            {
                X += VectorSpeed.X * Speed;
                Y += VectorSpeed.Y * Speed;
                //Angle += 10F;
                //if (Angle >= 360F)
                //    Angle -= 360F;
                //Image = BaseImage.Rotated(Angle);
            }
        }
    }
}
