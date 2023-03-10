using DOSBOX.Suggestions.plants;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DOSBOX.Suggestions
{
    public class Plant<T> : ClassIPlant where T : Fruit
    {
        public Plant(vecf vec)
        {
            this.vec = new vecf(vec);
            waterneed = 9;
            water = waterneed;
        }

        int ticksimpact = 0, ticksimpact_max = 4;
        public override void Update(byte[,] ActiveBG = null)
        {
            if (Data.Garden.Ticks == Data.Garden.TicksMax)
            {
                ticksimpact++;
                while (ticksimpact > ticksimpact_max)
                    ticksimpact -= ticksimpact_max;
            }
            if (ticksimpact == ticksimpact_max)
            {
                ticksimpact = 0;
                if (ActiveBG == null)
                {
                    CheckWater();
                    Grow();
                }
                else
                {
                    CheckWater(ActiveBG);
                    Grow(ActiveBG);
                }
            }

            masterbranch?.Update();
        }

        private void CheckWater(byte[,] ActiveBG = null)
        {
            int c = 0;
            foreach (var px in px_seed)
            {
                if(ActiveBG == null)
                {
                    if (Core.Layers[0][vec.i.x + px.x, vec.i.y + px.y] == 3)
                        c++;
                }
                else
                {
                    if (ActiveBG[vec.i.x + px.x, 63 - vec.i.y - px.y] == 3)
                        c++;
                }
            }
            float percent = c / (float)px_seed.Count;
            if (percent < 0.5F)
            {
                if(water > 0)
                    water--;
            }
            else if (water < waterneed)
                water++;
        }

        public override void Display(int layer)
        {
            foreach (var px in px_seed)
                if(!Core.isout(vec.i.x + px.x, vec.i.y + px.y, layer, Core.Cam))
                    Core.Layers[layer][vec.i.x + px.x, vec.i.y + px.y] = 4;
            masterbranch?.Display(layer);
            masterbranch?.DisplayLeaves(layer);

            masterbranch?.DisplayFruits(layer);
        }


        private void Grow(byte[,] ActiveBG = null)
        {
            if (water < waterneed)
                return;


            byte SafeGetOnActiveBG(int x, int y, vecf cam = null)
            {
                if (cam == null) cam = vecf.Zero;
                return !(x - cam.i.x < 0 || y < 0 || x >= ActiveBG.GetLength(0) || y - cam.i.y >= ActiveBG.GetLength(1)) ? ActiveBG[x - cam.i.x, y - cam.i.y] : (byte)4;
            }
            byte SafeGet(int x, int y, vecf cam = null) => ActiveBG == null ? Core.SafeGet(0, x, y, cam) : SafeGetOnActiveBG(x, y, cam);

            if (px_seed.Count < 16)
            {
                new List<vec>(px_seed).ForEach(s =>
                {
                    if (px_seed.Count >= 16)
                        return;
                    if (Core.RND.Next(px_seed.Count) == 0)
                    {
                        if (!Core.isout(vec.i.x + s.x - 1, vec.i.y + s.y, 0, Core.Cam) && !px_seed.Any(px => px.x == s.x - 1 && px.y == s.y) && SafeGet(vec.i.x + s.x-1, vec.i.y + s.y) == 3) px_seed.Add(new vec(s.x-1, s.y));
                        if (!Core.isout(vec.i.x + s.x + 1, vec.i.y + s.y, 0, Core.Cam) && !px_seed.Any(px => px.x == s.x + 1 && px.y == s.y) && px_seed.Count < 16 && SafeGet(vec.i.x + s.x+1, vec.i.y + s.y) == 3) px_seed.Add(new vec(s.x+1, s.y));
                        if (!Core.isout(vec.i.x + s.x, vec.i.y + s.y - 1, 0, Core.Cam) && !px_seed.Any(px => px.x == s.x && px.y == s.y - 1) && px_seed.Count < 16 && SafeGet(vec.i.x + s.x, vec.i.y + s.y-1) == 3) px_seed.Add(new vec(s.x, s.y-1));
                        if (!Core.isout(vec.i.x + s.x, vec.i.y + s.y + 1, 0, Core.Cam) && !px_seed.Any(px => px.x == s.x && px.y == s.y + 1) && px_seed.Count < 16 && SafeGet(vec.i.x + s.x, vec.i.y + s.y+1) == 3) px_seed.Add(new vec(s.x, s.y+1));
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
                    masterbranch = new Branch(Guid, null);
            }
        }

        public override Fruit CreateFruit(vecf v) => (Fruit)Activator.CreateInstance(typeof(T), new[] { v });

        public override int GetPotential() => masterbranch?.GetPotential() ?? 0;
    }
}
