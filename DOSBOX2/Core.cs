using DOSBOX2.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using Tooling;
using DOSBOX2.Common;

namespace DOSBOX2
{
    public class Core
    {
        public static int CurrentSceneIndex = -1;
        private static int menu_selection = 0;

        public static float Speed = 1F;

        public static List<(string Name, IScene Instance)> Suggestions = new List<(string, IScene)>()
        {
            ("Ninja", new SceneNinja()),
        };

        public static void Update()
        {
            if (CurrentSceneIndex == -1)
            {
                var (z, q, s, d) = KB.ZQSD(true);
                if (z && menu_selection > 0) menu_selection--;
                if (s && menu_selection < Suggestions.Count - 1) menu_selection++;
                if (z || q || s || d) Graphic.Clear(3);
                if (KB.IsKeyPressed(KB.Key.Space))
                {
                    CurrentSceneIndex = menu_selection;
                    Graphic.Clear(3);
                    Suggestions[CurrentSceneIndex].Instance.Init();
                }
                else
                {
                    int h = 8;
                    int count_per_screen = Graphic.resolution / h;
                    int page = menu_selection / count_per_screen;
                    for (int i = 0; i < Math.Min(count_per_screen, Suggestions.Count - page * count_per_screen); i++)
                        Text.DisplayText(Suggestions[page * count_per_screen + i].Name, 2, 2 + i * h);
                    Graphic.DrawRect(0, 1, menu_selection * h - page * count_per_screen * h, Graphic.resolution - 4, h);
                }
            }
            else
            {
                if (KB.IsKeyPressed(KB.Key.Back))
                {
                    Suggestions[CurrentSceneIndex].Instance.Dispose();
                    menu_selection = CurrentSceneIndex;
                    CurrentSceneIndex = -1;
                    Graphic.Clear(3);
                }
                else
                {
                    Suggestions[CurrentSceneIndex].Instance.Update();
                    GraphicExt.DisplayControlsRectInfo(0, Graphic.resolution-2);
                }
            }
        }
    }
}
