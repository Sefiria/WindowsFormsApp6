using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Fly : Dispf
    {
        public Fly(vecf v)
        {
            vec = v;
            CreateGraphics();
            DisplayCenterSprite = true;
        }
        public Fly(int x, int y)
        {
            vec = new vecf(x, y);
            CreateGraphics();
            DisplayCenterSprite = true;
        }
        void CreateGraphics()
        {
            g = new byte[1, 1] { { (byte)RandomThings.arnd(1, 2) } };
            scale = (byte)RandomThings.arnd(1, 2);
        }
        public void Update()
        {
            float speed = 1F;
            float closeThreshold = Tile.TSZ * 2F;

            vecf samus = Fusion.Instance.samus.vec - (12F, 16F).Vf();
            float d = vec.Distance(samus);
            vecf look = (RandomThings.rnd1(), RandomThings.rnd1()).Vf();

            if (d > closeThreshold)
            {
                look += samus - vec;
            }

            vec += look.Normalized() * speed / scale;

        }
        public void LeaveScreen()
        {
            float speed = 1F;
            vecf samus = Fusion.Instance.samus.vec - (Fusion.Instance.samus._w, Fusion.Instance.samus._h).Vf() * Tile.TSZ;
            float d = samus.Distance(vec);
            vecf look = (vec - samus).Normalized();
            vec += look * speed / scale;
        }
    }
}
