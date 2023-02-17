using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DOSBOX.Suggestions
{
    public class Template : ISuggestion
    {
        public static Template Instance;


        public void Init()
        {
            Instance = this;

            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI
            
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
                Core.CurrentSuggestion = null;


            DisplayUI();


            Collisions();
        }

        private void DisplayUI()
        {
        }

        private void Collisions()
        {
        }
    }
}
