using DOSBOX.Suggestions;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;

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

        public static bool isout(vecf v) => isout(v.i.x, v.i.y);
        public static bool isout(vec v) => isout(v.x, v.y);
        public static bool isout(float x, float y) => isout((int)x, (int)y);
        public static bool isout(int x, int y) => x < 0 || y < 0 || x >= 64 || y >= 64;

        public static List<(string Name, ISuggestion Instance)> Suggestions = new List<(string, ISuggestion)>()
        {
            //("Debug", new Debug()),
            ("Road", new Road()),
            ("Breaker", new Breaker()),
            ("Fusion", new Fusion()),
            ("Plants", new Plants()),
        };
    }
}
