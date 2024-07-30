using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Bullet : Harmful
    {
        public vecf look;

        public Bullet(float x, float y, vecf look)
        {
            vec = new vecf(x, y);
            CreateGraphics();
            this.look = look;
        }

        void CreateGraphics()
        {
            g = new byte[2, 2]
            {
                    { 3, 3 },
                    { 3, 3 },
            };
            scale = 1;
        }

        public void Update()
        {
            float speed = 2F;
            var collider = Fusion.Instance.ColliderRoom(vec + look * speed, _w, _h);
            if (collider == null)
            {
                vec += look * speed;
                look.y += 0.08F;
            }
            else
            {
                (collider as Hittable)?.Hit(this);
                Destroy();
            }

            if (Fusion.Instance.room.isout(vec.x, vec.y))
                Destroy();
        }
        public void Destroy()
        {
            var instance = Fusion.Instance;
            instance.bullets.Remove(this);
            instance.particules.Add(new Particule(vec.x, vec.y, look.Rotate(110F)));
            instance.particules.Add(new Particule(vec.x, vec.y, look.Rotate(-110F)));
        }
    }
}
