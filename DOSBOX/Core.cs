using DOSBOX.Suggestions;
using System;
using System.Collections.Generic;
using System.Drawing;
using Tooling;

namespace DOSBOX
{
    public static class Core
    {
        public static Random RND = new Random((int)DateTime.UtcNow.Ticks);
        public static List<byte[,]> Layers = new List<byte[,]>();
        public static List<Color> Palette = new List<Color>()
        {
            Color.FromArgb(224, 248, 208),
            Color.FromArgb(136, 192, 112),
            Color.FromArgb(52, 104, 86),
            Color.FromArgb(8, 24, 32),
            Color.FromArgb(224, 248, 208),
        };
        public static ISuggestion CurrentSuggestion = null, NextSuggestion = null;
        public static vecf Cam = vecf.Zero;
        public static bool KeepCamCoords = false;
        public static long TotalTicks;

        public static bool isout(vecf v, int layer = -1, vecf cam = null) => isout(v.i.x, v.i.y, layer, cam);
        public static bool isout(vec v, int layer = -1, vecf cam = null) => isout(v.x, v.y, layer, cam);
        public static bool isout(float x, float y, int layer = -1, vecf cam = null) => isout((int)x, (int)y, layer, cam);
        public static bool isout(int x, int y, int layer = -1, vecf cam = null)
        {
            int w = layer == -1 ? 64 : Layers[layer].GetLength(0);
            int h = layer == -1 ? 64 : Layers[layer].GetLength(1);
            if (cam == null)
                cam = vecf.Zero;
            return x - cam.x < 0 || y - cam.y < 0 || x - cam.x >= w || y - cam.y >= h;
        }

        public static List<(string Name, ISuggestion Instance)> Suggestions = new List<(string, ISuggestion)>()
        {
            //("Debug", new Debug()),
            ("Road", new Road()),
            ("Breaker", new Breaker()),
            ("Fusion", new Fusion()),
            ("Plants", new Plants()),
            ("City", new City()),
        };

        public static byte SafeGet(int layer, int x, int y, vecf cam = null)
        {
            if (cam == null)
                cam = vecf.Zero;
            if (!isout(x, y, layer, cam))
                return Layers[layer][(int)cam.x + x, (int)cam.y + y];
            return (byte)(layer == 0 ? 4 : 0);
        }
    }
}
