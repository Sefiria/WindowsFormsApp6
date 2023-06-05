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
    internal class Particle : Drawable
    {
        public float move_speed;

        public Particle() : base()
        {
            Position = new Vector2(Global.RND.Next(Global.W), 0);
            CreateTexture();
            move_speed = Global.RND.Next(50, 200) / 10F;
        }
        private void CreateTexture()
        {
            var h = Global.RND.Next(1, 5);
            var px = new byte[h, h];
            for (int y = 0; y < h; y++)
            {
                px[y, 0] = 1;
            }
            Texture = px.PartToBitmap();
        }

        public override void Update()
        {
            Position.Y += move_speed;

            if (Y >= Global.H)
                Exists = false;
        }
    }
}
