using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsFormsApp11.Bullets
{
    public class BulletTiny : Bullet
    {
        public float WaveForce = 20F;
        public float WaveAmp = 2F;

        private float WaveValue = 0F;
        private float WaveMaxV = 180F;
        private float WaveDirection = Var.Rnd.Next(2) == 0 ? -1F : 1F;

        public BulletTiny(float x, float y, float lookx = 0F, float looky = -1F) : base(x, y, lookx, looky)
        {
        }

        public override void Update()
        {
            base.Update();

            X += (float)Math.Cos((WaveValue - WaveMaxV).ToRad() * LookY) * WaveAmp + LookX * SpeedMove;
            Y += (float)Math.Sin((WaveValue - (WaveMaxV / 2F)).ToRad() * LookX) * WaveAmp + LookY * SpeedMove;

            WaveValue += WaveForce * WaveDirection;
            if (WaveValue < -WaveMaxV)
            {
                WaveValue = -WaveMaxV;
                WaveDirection = WaveDirection == 1 ? -1F : 1F;
            }
            if (WaveValue > WaveMaxV)
            {
                WaveValue = WaveMaxV;
                WaveDirection = WaveDirection == 1 ? -1F : 1F;
            }

            if (Y + hW < 0)
                Destroy = true;
        }
    }
}
