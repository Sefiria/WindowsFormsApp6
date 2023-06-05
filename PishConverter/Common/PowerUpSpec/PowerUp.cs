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
    internal class PowerUp : Drawable
    {
        public Vector2 Look;
        public float move_speed = 4F;
        public PowerUpTransformInfo Info;

        public PowerUp(PowerUpTransformInfo info, Vector2 position, Vector2 look) : base()
        {
            Info = info;
            Position = position;
            Look = look;
            CreateTexture();
        }
        private void CreateTexture()
        {
            Texture = new byte[5, 5]
            {
                { 0, 1, 1, 1, 0 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 1, 0, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 1 },
            }.ToBitmap();
        }

        public override void Update()
        {
            if (Exists == false)
                return;

            Position += Look * move_speed;

            if (Y >= Global.H)
                Exists = false;

            if ((Position - User._.Position).LengthSquared() <= User._.W * W)
            {
                Exists = false;
                User._.GivePowerUp(Info);
            }
        }
    }
}
