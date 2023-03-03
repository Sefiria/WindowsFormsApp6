using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DOSBOX.Suggestions
{
    public class Plants : ISuggestion
    {
        public static Plants Instance;
        public bool ShowHowToPlay { get; set; }
        public IState CurrentState, NextState;
        public static List<(string Name, IState Instance)> States = new List<(string, IState)>()
        {
            ("Garden", new Garden()),
            ("Fruits", new Fruits()),
            ("Seeds", new Seeds()),
        };


        public void HowToPlay()
        {
            Text.DisplayText("press space", 2, 2, 0);
            Text.DisplayText("to continue", 2, 8, 0);

            if (KB.IsKeyDown(KB.Key.Space))
            {
                Graphic.Clear(0, 0);
                ShowHowToPlay = false;
            }
        }

        public void Init()
        {
            Instance = this;

            plants.Data.Init();

            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // UI
            Graphic.Clear(0, 0);
            Graphic.Clear(0, 1);
        }

        public void Update()
        {
            if (ShowHowToPlay)
            {
                HowToPlay();
                return;
            }

            if (NextState != null)
            {
                CurrentState = NextState;
                NextState = null;
                if (CurrentState != Garden.Instance)
                    CurrentState.Init();
                else
                    Garden.Instance.InitActive();
            }

            if (CurrentState == null)
            {
                UpdateMenu();

                if (KB.IsKeyPressed(KB.Key.Escape))
                {
                    Garden.Instance = null;
                    Core.CurrentSuggestion = null;
                    return;
                }
            }
            else
            {
                CurrentState.Update();
            }
        }


        int menu_selection = 0;
        private void UpdateMenu()
        {
            if (KB.IsKeyPressed(KB.Key.Up) && menu_selection > 0)
                menu_selection--;
            if (KB.IsKeyPressed(KB.Key.Down) && menu_selection < States.Count - 1)
                menu_selection++;

            Graphic.Clear(2, 0);
            Graphic.Clear(0, 1);
            Graphic.DisplayRectAndBounds(2, 2, 60, 60, 0, 1, 1, 0);
            int x = 6, y = 6, w = 56, h = 9;
            Graphic.DisplayRectAndBounds(x - 2, y - 2 + menu_selection * (h + 1), w, h, 0, 1, 1, 1);
            for (int i = 0; i < States.Count; i++)
            {
                Text.DisplayText(States[i].Name, x, y, 1);
                y += h + 1;
            }

            if (KB.IsKeyDown(KB.Key.Enter))
                NextState = States[menu_selection].Instance;
        }
    }
}
