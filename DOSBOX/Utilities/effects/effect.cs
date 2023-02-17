﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace DOSBOX.Utilities.effects
{
    public abstract class effect
    {
        protected abstract List<byte[,]> g { get; set; }
        protected virtual int scale { get; set; } = 1;
        protected virtual bool pingpong { get; set; } = false;
        bool reverse = false;
        float frame = 0F;
        int framemax => g.Count;
        protected virtual float incr { get; set; } = 0.1F;

        void Tick()
        {
            if (pingpong)
            {
                frame += (reverse ? -1F : 1F) * incr;

                if ((reverse && (int)frame <= 0) || (!reverse && (int)frame >= framemax))
                {
                    frame = reverse ? 0 : framemax - 1;
                    reverse = !reverse;
                }
            }
            else
            {
                frame += incr;
                while ((int)frame >= framemax) frame -= framemax;
            }
        }

        public void Display(int layer, Dispf df = null, Disp d = null)
        {
            if(df == null && d == null) return;

            int w = Core.Layers[layer].GetLength(0);
            int h = Core.Layers[layer].GetLength(1);
            int _w = g[(int)frame].GetLength(0);
            int _h = g[(int)frame].GetLength(1);
            int x = df != null ? (int)df.vec.x : d.vec.x;
            int y = df != null ? (int)df.vec.y : d.vec.y;
            int cx = (g.Max(_g => _g.GetLength(0)) - _w) / 2;
            int cy = (g.Max(_g => _g.GetLength(1)) - _h) / 2;

            for (int i = 0; i < _w * scale; i++)
                for (int j = 0; j < _h * scale; j++)
                    if (x + _w - i - 1 + cx >= 0 && x + _w - i - 1 + cx < w && y + j + cy >= 0 && y + j + cy < h)
                        Core.Layers[layer][x + _w - i - 1 + cx, y + j + cy] = g[(int) frame][i / scale, j / scale];

            Tick();
        }

        public class crash : effect
        {
            protected override bool pingpong { get; set; } = true;
            protected override float incr { get; set; } = 0.5F;
            protected override List<byte[,]> g { get; set; } = new List<byte[,]>()
            {
                new byte[4,4]
                {
                    {0, 0, 3, 0 },
                    {0, 3, 3, 0 },
                    {0, 3, 2, 3 },
                    {3, 0, 3, 3 }
                },
                new byte[6,6]
                {
                    {0, 0, 0, 3, 0, 0 },
                    {3, 0, 0, 3, 3, 3 },
                    {0, 3, 3, 2, 3, 0 },
                    {0, 3, 2, 1, 2, 3 },
                    {3, 2, 3, 3, 2, 3 },
                    {3, 3, 0, 3, 3, 0 },
                },
                new byte[8,8]
                {
                    {0, 0, 3, 0, 0, 0, 3, 3 },
                    {0, 0, 3, 0, 0, 3, 2, 3 },
                    {0, 0, 0, 3, 3, 2, 2, 3 },
                    {0, 3, 3, 2, 2, 2, 3, 0 },
                    {0, 3, 2, 1, 1, 3, 0, 0 },
                    {3, 3, 2, 1, 1, 3, 0, 0 },
                    {0, 0, 3, 2, 2, 3, 3, 3 },
                    {0, 0, 0, 3, 3, 0, 3, 0 },
                },
            };
        }
    }
}
