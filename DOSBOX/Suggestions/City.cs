using DOSBOX.Suggestions.city;
using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DOSBOX.Suggestions
{
    public class City : ISuggestion
    {
        public static City Instance;

        public bool ShowHowToPlay { get; set; }


        public void HowToPlay()
        {
            Text.DisplayText("press space", 4, 4, 0);

            if (KB.IsKeyDown(KB.Key.Space))
            {
                Graphic.Clear(0, 0);
                ShowHowToPlay = false;
            }
        }

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
            {
                Core.CurrentSuggestion = null;
                return;
            }

            if (ShowHowToPlay)
            {
                HowToPlay();
                return;
            }

            Data.Instance.map.Update();
            Data.Instance.user.Update();

            Data.Instance.map.Display();
            Data.Instance.user.Display();
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
