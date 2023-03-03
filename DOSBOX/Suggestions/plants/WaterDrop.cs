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

            if (Core.isout(vec, 1, Core.Cam))
            {
                destroy = true;
                return;
            }

            if (vec.y >= Garden.FloorLevel)
            {
                int x = 0, y = 0;
                while (vec.i.y + y < 64 && Core.Layers[0][vec.i.x, vec.i.y + y] == 3)
                {
                    y++;
                    if (Core.RND.Next(5) == 2)
                        x += Core.RND.Next(64) % 2 == 0 ? 1 : -1;
                }
                if (vec.i.y + y - Core.Cam.i.y < 64 && vec.i.x + x - Core.Cam.i.x >= 0 && vec.i.x + x - Core.Cam.i.x < 64)
                    Core.Layers[0][vec.i.x + x, vec.i.y + y] = 3;
                destroy = true;
                return;
            }

            look.x *= 0.85F;
            look.y *= 1.2F;
        }
    }
}
