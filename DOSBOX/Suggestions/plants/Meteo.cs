using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions
{
    public class Meteo
    {
        public List<byte> Previsions;
        public float Time;
        public static readonly float time_speed = 0.2F;

        public Meteo()
        {
            Time = 0F;
            Previsions = new List<byte>();
            NextPrevisions();
        }

        public void NextPrevisions()
        {
            while (Previsions.Count < 12)
            {
                List<byte> next = new List<byte>()
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                List<int> peaks = new List<int>();
                
                int peaks_count = Core.RND.Next(0, 4);
                for(int i=0; i< peaks_count; i++)
                    peaks.Add(Core.RND.Next(0, 12));

                int index, id, v, decrease, updown = -1;
                void recur()
                {
                    decrease = Core.RND.Next(10, 101);
                    if (next[id] > v)
                        return;
                    next[id] = (byte)v;
                    v += decrease * updown;
                    if (v > 0 && v < 11)
                        recur();
                }
                for (int i = 0; i < peaks.Count; i++)
                {
                    index = peaks[i];
                    next[index] = 255;

                    v = 255;
                    id = index;
                    recur();
                    v = 255;
                    id = index;
                    updown = 1;
                    recur();
                }

                Previsions.AddRange(next);
            }
        }

        public void Update()
        {
            Rain();

            if(Time < 24F)
            {
                Time += time_speed;
                return;
            }
            Time -= 24F;
            Previsions.RemoveAt(0);
            NextPrevisions();
        }

        private void Rain()
        {
            if (Previsions[0] == 0)
                return;

            int w = Core.Layers[0].GetLength(0);
            for (int i = 0; i < 255; i++)
            {
                vecf vec = new vecf( Core.RND.Next(0, w), 0F );
                vecf look = new vecf((Core.RND.Next(0, 101) - 50F) / 100F, Core.RND.Next(0, 101) / 100F);
                var waterdrop = new WaterDrop() { vec = vec, look = look };
                Garden.Instance.WaterDrops.Add(waterdrop);
            }
        }
    }
}
