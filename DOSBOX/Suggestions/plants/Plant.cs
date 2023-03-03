using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DOSBOX.Suggestions
{
    public class Plant<T> : IPlant where T : Fruit
    {
        public vecf vec { get; set; }
        public byte waterneed { get; set; } = 10;
        public byte water { get; set; }
    public Branch masterbranch { get; set; } = null;

        List<vec> px_seed = new List<vec>() { new vec(0, 0) };

        public Plant(vecf vec)
        {
            this.vec = new vecf(vec);
            water = waterneed;
        }

        int ticksimpact = 0, ticksimpact_max = 4;
        public void Update()
        {
            if (Garden.Instance.Ticks == Garden.Instance.TicksMax)
            {
                ticksimpact++;
                while (ticksimpact > ticksimpact_max)
                    ticksimpact -= ticksimpact_max;
            }
            if (ticksimpact == ticksimpact_max)
            {
                ticksimpact = 0;
                CheckWater();
                Grow();
            }

            masterbranch?.Update();
        }

        private void CheckWater()
        {
            int c = 0;
            foreach (var px in px_seed)
                if (Core.Layers[0][vec.i.x + px.x, vec.i.y + px.y] == 3)
                    c++;
            float percent = c / (float)px_seed.Count;
            if (percent < 0.5F)
            {
                if(water > 0)
                    water--;
            }
            else if (water < waterneed)
                water++;
        }

        public void Display(int layer)
        {
            foreach (var px in px_seed)
                if(!Core.isout(vec.i.x + px.x, vec.i.y + px.y, layer, Core.Cam))
                    Core.Layers[layer][vec.i.x + px.x, vec.i.y + px.y] = 4;
            masterbranch?.Display(layer);
            masterbranch?.DisplayLeaves(layer);
            masterbranch?.DisplayFruits(layer);
        }


        private void Grow()
        {
            if (water < waterneed)
                return;

            if(px_seed.Count < 16)
            {
                new List<vec>(px_seed).ForEach(s =>
                {
                    if (px_seed.Count >= 16)
                        return;
                    if (Core.RND.Next(px_seed.Count) == 0)
                    {
                        if (!Core.isout(vec.i.x + s.x - 1, vec.i.y + s.y, 0, Core.Cam) && !px_seed.Any(px => px.x == s.x - 1 && px.y == s.y) && Core.SafeGet(0, vec.i.x + s.x-1, vec.i.y + s.y) == 3) px_seed.Add(new vec(s.x-1, s.y));
                        if (!Core.isout(vec.i.x + s.x + 1, vec.i.y + s.y, 0, Core.Cam) && !px_seed.Any(px => px.x == s.x + 1 && px.y == s.y) && px_seed.Count < 16 && Core.SafeGet(0, vec.i.x + s.x+1, vec.i.y + s.y) == 3) px_seed.Add(new vec(s.x+1, s.y));
                        if (!Core.isout(vec.i.x + s.x, vec.i.y + s.y - 1, 0, Core.Cam) && !px_seed.Any(px => px.x == s.x && px.y == s.y - 1) && px_seed.Count < 16 && Core.SafeGet(0, vec.i.x + s.x, vec.i.y + s.y-1) == 3) px_seed.Add(new vec(s.x, s.y-1));
                        if (!Core.isout(vec.i.x + s.x, vec.i.y + s.y + 1, 0, Core.Cam) && !px_seed.Any(px => px.x == s.x && px.y == s.y + 1) && px_seed.Count < 16 && Core.SafeGet(0, vec.i.x + s.x, vec.i.y + s.y+1) == 3) px_seed.Add(new vec(s.x, s.y+1));
                    }
                });
            }

            if (masterbranch != null)
            {
                masterbranch.Grow();
            }
            else
            {
                if (px_seed.Count > 6 && Core.RND.Next(17 - px_seed.Count) == 0)
                    masterbranch = new Branch(this, null);
            }
        }

        public Fruit CreateFruit(vecf v) => (Fruit)Activator.CreateInstance(typeof(T), new[] { v });
    }
}
