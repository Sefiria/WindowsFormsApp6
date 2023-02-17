using DOSBOX.Suggestions;
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

        public static List<(string Name, ISuggestion Instance)> Suggestions = new List<(string, ISuggestion)>()
        {
            ("Road", new Road()),
            ("Breaker", new Breaker()),
        };
    }
}
