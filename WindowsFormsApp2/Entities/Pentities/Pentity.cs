using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Interfaces;

namespace WindowsFormsApp2.Entities.Pentities
{
    public class Pentity : DrawableEntity
    {
        public Entity From;
        public IDamager Damager;
        public VecF VectorSpeed = new VecF();
        public const float Speed = 5F;
        public float Angle = 0F;
        private Bitmap BaseImage;

        public Pentity(Entity From, IDamager Damager, float X, float Y, Bitmap Image, VecF VectorSpeed)
        {
            this.From = From;
            this.Damager = Damager;
            this.X = X;
            this.Y = Y;
            BaseImage = this.Image = Image;
            this.VectorSpeed = VectorSpeed;
        }
        public override void Update()
        {
            Entity e = Tools.GetEntity_PositionTileIsOccupied(X + VectorSpeed.X * Speed, Y + VectorSpeed.Y * Speed, new List<Entity>() { From, Damager as Entity, this }, !(From is Player));
            if (e != null)
            {
                Exists = false;
                (e as IRP)?.Hit(Damager);
            }
            else
            {
                X += VectorSpeed.X * Speed;
                Y += VectorSpeed.Y * Speed;
                Angle += 10F;
                if (Angle >= 360F)
                    Angle -= 360F;
                Image = BaseImage.Rotated(Angle);
            }
        }
    }
}
