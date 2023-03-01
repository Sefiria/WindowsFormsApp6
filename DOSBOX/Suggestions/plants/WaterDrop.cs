using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions
{
    public class WaterDrop
    {
        public bool destroy = false;
        public vecf vec, look;

        public void Update()
        {
            if (destroy)
                return;

            vec += look;

            if (Core.isout(vec))
            {
                destroy = true;
                return;
            }

            if (vec.y >= Garden.FloorLevel)
            {
                int y = 0;
                while (vec.i.y + y < 64 && Core.Layers[0][vec.i.x, vec.i.y + y] == 3)
                    y++;
                if (vec.i.y + y < 64)
                    Core.Layers[0][vec.i.x, vec.i.y + y] = 3;
                destroy = true;
                return;
            }

            look.x *= 0.85F;
            look.y *= 1.2F;
        }
    }
}
