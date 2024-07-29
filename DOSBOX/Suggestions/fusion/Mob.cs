using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Mob : Dispf
    {
        byte timershot = 0;
        float jump_look_y = 0F;
        bool is_on_ground = false;

        public sbyte ShieldMax = 0;
        public sbyte Shield = 0;
        public sbyte Lifes = 1;

        public Mob(int x, int y)
        {
            vec = new vecf(x, y);
            CreateGraphics();
            DisplayCenterSprite = true;
        }
        void CreateGraphics()
        {
            g = new byte[1, 1];
            for (int x = 0; x < _w; x++)
                for (int y = 0; y < _h; y++)
                    g[x, y] = 1;
            scale = 1;
        }
        public void Update()
        {
            float d_full_aggro = Tile.TSZ * 2F;

            float d = vec.Distance(Fusion.Instance.samus.vec);
            float ratio = d <= d_full_aggro ? 1F : (float)Math.Exp(-(d - d_full_aggro));
            vecf look = ((Fusion.Instance.samus.vec - vec) * ratio + (RandomThings.rnd1(), RandomThings.rnd1()).Vf() * (1F - ratio)).Normalized();
            move(look);
        }
        private bool move(vecf look)
        {
            float speed = 0.2F;
            bool collides;

            // y
            if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x, vec.y + look.y * speed))))
                vec.y += look.y;

            // x
            if (look.x != 0F)
            {
                if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x + look.x * speed, vec.y))))
                    vec.x += look.x;
                else
                {
                    collides = false;
                    float n = -1F;
                    for (float i = 0.1F; i <= 1.05F && !collides; i += 0.1F)
                        if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x + look.x * i * speed, vec.y))))
                            n = i;
                    if (collides && n != -1F)
                        vec.x += look.x * n * speed;
                }
            }
            return !collides;
        }
    }
}
