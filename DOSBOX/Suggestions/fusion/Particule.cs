using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Particule : Dispf
    {
        vecf look;

        public Particule(float x, float y, vecf look)
        {
            vec = new vecf(x, y);
            CreateGraphics();
            this.look = look;
        }

        void CreateGraphics()
        {
            g = new byte[1, 1]
            {
                    { 2 },
            };
            scale = 1;
        }

        public void Update()
        {
            var instance = Fusion.Instance;
            float speed = 2F;
            if (!instance.CollidesRoom(vec + look * speed, _w, _h))
            {
                vec += look * speed;
                look += new vecf(0F, 0.5F);
            }
            else
            {
                instance.particules.Remove(this);
            }

            if (instance.room.isout(vec.x, vec.y))
                instance.particules.Remove(this);
        }
    }
}
