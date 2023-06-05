using PishConverter.Common.MapSpec;
using PishConverter.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PishConverter.Common
{
    internal class Bullet : Drawable
    {
        public Vector2 Look;
        public float move_speed = 4F;
        public Drawable Owner;

        public Bullet(Drawable owner, Vector2 position, Vector2 look) : base()
        {
            Owner = owner;
            Position = position;
            Look = look;
            CreateTexture();
        }
        private void CreateTexture()
        {
            Texture = new byte[1, 1]
            {
                { 1 },
            }.ToBitmap(4);
        }

        public override void Update()
        {
            Position += Look * move_speed;

            if (Y >= Global.H || Y + H < 0F)
                Exists = false;

            var list = new List<Drawable>(Map.Instance.Entities);
            list.Remove(this);
            list.Remove(Owner);
            foreach(var e in list)
            {
                if ((Position - e.Position).LengthSquared() <= e.W * W)
                {
                    Exists = false;
                    (e as IHit)?.Hit();
                }
            }
        }
    }
}
