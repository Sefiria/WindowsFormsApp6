using DOSBOX.Suggestions.plants;
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
        public bool destroy { get; set; } = false;
        public vecf vec { get; set; }
        public vecf look { get; set; }

        public void Update(byte[,] ActiveBG = null)
        {
            if (destroy)
                return;

            vec += look;

            if (Core.isout(vec, 1) && Core.isout(new vecf(vec.x, vec.y + 4), 1))
            {
                destroy = true;
                return;
            }

            if (vec.y >= Data.Garden.FloorLevel)
            {
                int x = 0, y = 0, w = Core.Layers[0].GetLength(0);
                while (vec.i.y + y < 64 && Core.Layers[0][vec.i.x, vec.i.y + y] == 3)
                {
                    y++;
                    if (Core.RND.Next(5) == 2)
                        x += Core.RND.Next(64) % 2 == 0 ? 1 : -1;
                }
                if (vec.i.y + y < 64 && vec.i.x + x >= 0 && vec.i.x + x < w)
                    Core.Layers[0][vec.i.x + x, vec.i.y + y] = 3;
                destroy = true;
                if (Data.Instance.Items.ContainsKey("water\ntank") && Data.Instance.WaterBucket < Data.Instance.WaterBucketMax && Core.RND.Next(11) == 5)
                    Data.Instance.WaterBucket++;
                return;
            }

            look.x *= 0.85F;
            if (Math.Round(look.y, 4) == 0F)
                look.y = 0.6F;
            look.y *= 1.2F;
        }
    }
}
