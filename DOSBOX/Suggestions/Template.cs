using DOSBOX.Utilities;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class Template : ISuggestion
    {
        public static Template Instance;
        public bool ShowHowToPlay { get; set; }


        public void HowToPlay()
        {

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
