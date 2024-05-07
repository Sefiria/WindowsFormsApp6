using DOSBOX.Utilities;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class Debug : ISuggestion
    {
        public static Debug Instance;
        public bool ShowHowToPlay { get; set; } = false;


        public void HowToPlay()
        {
        }

        public void Init()
        {
            Instance = this;

            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Core.CurrentSuggestion = null;
                return;
            }

            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG

            vecf start = new vecf(32, 32);
            int test = 5;
            switch (test)
            {
                default:
                case 0:
                    Graphic.DisplayLine(start, new vecf(start.x - 16, start.y), 1, 0);
                    Graphic.DisplayLine(start, new vecf(start.x + 16, start.y), 2, 0);
                    break;
                case 1:
                    Graphic.DisplayLine(start, new vecf(start.x, start.y - 16), 1, 0);
                    Graphic.DisplayLine(start, new vecf(start.x, start.y + 16), 2, 0);
                    break;
                case 2:
                    Graphic.DisplayLine(start, new vecf(start.x - 16, start.y - 16), 1, 0);
                    Graphic.DisplayLine(start, new vecf(start.x + 16, start.y + 16), 2, 0);
                    break;
                case 3:
                    Graphic.DisplayLine(start, new vecf(start.x + 16, start.y - 16), 1, 0);
                    Graphic.DisplayLine(start, new vecf(start.x - 16, start.y + 16), 2, 0);
                    break;
                case 4:
                    Graphic.DisplayLine(start, new vecf(start.x + 10, start.y - 22), 1, 0);
                    Graphic.DisplayLine(start, new vecf(start.x - 10, start.y + 22), 2, 0);
                    break;
                case 5:
                    Graphic.DisplayLine(start, new vecf(start.x - 10, start.y - 22), 1, 0);
                    Graphic.DisplayLine(start, new vecf(start.x + 10, start.y + 22), 2, 0);
                    break;
            }
        }
    }
}
