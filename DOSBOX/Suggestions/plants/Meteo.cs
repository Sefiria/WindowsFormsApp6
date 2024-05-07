using DOSBOX.Suggestions.plants;
using DOSBOX.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class Meteo
    {
        public List<byte> Previsions { get; set; }
        public float Time { get; set; }
        public static readonly float time_speed = 0.2F;
        public static readonly byte max_value = 100;

        /// <summary>
        /// Not to be called (workaround for Newtonsoft.Json loading data specific constructor case)
        /// </summary>
        /// <param name="_"></param>
        [JsonConstructor]
        public Meteo(bool _ = false)// Combine pour charger meteo depuis sauvegarde sans écraser Previsions dans le constructeur
        {
            Time = 0F;
        }
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
                int peaks_count = Core.RND.Next(40) / 10, v, index, id, decrease, updown = -1;

                for (int i = 0; i < peaks_count; i++)
                {
                    do { v = Core.RND.Next(12); }
                    while (peaks.Contains(v));
                    peaks.Add(v);
                }

                void recur()
                {
                    id += updown;
                    if (id < 0 || id > 11)
                        return;
                    decrease = Core.RND.Next(10, 101);
                    if (next[id] > v)
                        return;
                    v += decrease * updown;
                    next[id] = (byte)v;
                    if (v > 0)
                        recur();
                }
                for (int i = 0; i < peaks.Count; i++)
                {
                    index = peaks[i];
                    next[index] = max_value;

                    v = max_value;
                    id = index;
                    recur();
                    v = max_value;
                    id = index;
                    updown = 1;
                    recur();
                }

                Previsions.AddRange(next);
            }
        }

        public void Update()
        {
            if ((int)Time % 2 == 0)
                Rain();

            if (Time >= 24F)
            {
                Time -= 24F;
                Previsions.RemoveAt(0);
                NextPrevisions();
            }
            else
            {
                Time += time_speed;
            }
        }

        private void Rain()
        {
            if (Previsions[0] == 0 || Previsions[0] / 20 == 0)
                return;

            int w = Core.Layers[0].GetLength(0);
            for (int i = 0; i < Previsions[0] / 20; i++)
            {
                vecf vec = new vecf(Core.RND.Next(0, w), -Core.RND.Next(8));
                vecf look = new vecf((Core.RND.Next(0, 101) - 50F) / 100F, Core.RND.Next(0, 101) / 100F);
                var waterdrop = new WaterDrop() { vec = vec, look = look };
                Data.Garden.WaterDrops.Add(waterdrop);
            }
        }

        public void Display(int layer)
        {
            Int32Rect bounds = new Int32Rect(9, 2, 64 - 14, 6);
            for (int x = 0; x < 24; x++)
                for (int y = (int)bounds.Height - 1; Previsions.Count >= 12 && y >= bounds.Height - 1 - Previsions[(int)((x / (float)24) * 12)] / max_value; y--)
                    Core.Layers[layer][bounds.X + x + x % 2, bounds.Y + y] = 3;

            int h = (int)Time;
            int m = (int)(Math.Round(Time - (int)Time, 2) * 60);
            if (h == 24) h = 0;
            if (m == 24) m = 0;
            Text.DisplayText(string.Format("{0, 2}:{1, -2}", h, m), 63 - 8 * 3, 2, layer);
        }
    }
}
