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
            if (Global.SpaceParticlesLook.Y == 1)
                Position = new Vector2(Global.RND.Next(Global.W), 0);
            if (Global.SpaceParticlesLook.Y == -1)
                Position = new Vector2(Global.RND.Next(Global.W), Global.H - 1);
            if (Global.SpaceParticlesLook.X == 1)
                Position = new Vector2(0, Global.RND.Next(Global.H));
            if (Global.SpaceParticlesLook.X == -1)
                Position = new Vector2(Global.W - 1, Global.RND.Next(Global.H));
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
            Position += Global.SpaceParticlesLook * move_speed;

            if (Y >= Global.H)
                Exists = false;
        }
    }
}
