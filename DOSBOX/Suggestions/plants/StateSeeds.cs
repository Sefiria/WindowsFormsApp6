using DOSBOX.Suggestions.plants;
using DOSBOX.Utilities;
using System;
using System.Linq;

namespace DOSBOX.Suggestions
{
    public class Seeds : IState
    {
        int pagemax => (int)Math.Ceiling(Data.Instance.Seeds.Count / 5D) - 1;
        int page;

        public void Init()
        {
            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            page = 0;
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Plants.Instance.CurrentState = null;
                return;
            }

            var seedsofpage = Data.Instance.Seeds.Keys.Skip(5 * page).Take(4).ToList();
            int id = seedsofpage.IndexOf(Data.Instance.SelectedSeed);
            if (id == -1 && seedsofpage.Count > 0)
            {
                id = 0;
                Data.Instance.SelectedSeed = seedsofpage[id];
            }
            if (KB.IsKeyPressed(KB.Key.Left) && page > 0)
            {
                page--;
                if (seedsofpage.Count > 0)
                {
                    id = 0;
                    Data.Instance.SelectedSeed = seedsofpage[id];
                }
            }
            if (KB.IsKeyPressed(KB.Key.Right) && page < pagemax)
            {
                page++;
                if (seedsofpage.Count > 0)
                {
                    id = 0;
                    Data.Instance.SelectedSeed = seedsofpage[id];
                }
            }
            if (KB.IsKeyPressed(KB.Key.Up) && id > 0)
                Data.Instance.SelectedSeed = seedsofpage[id - 1];
            if (KB.IsKeyPressed(KB.Key.Down) && id < seedsofpage.Count - 1)
                Data.Instance.SelectedSeed = seedsofpage[id + 1];

            DisplayUI();
        }

        private void DisplayUI()
        {
            Dispf obj;

            if (string.IsNullOrWhiteSpace(Data.Instance.SelectedSeed) && Data.Instance.Seeds.Count > 0)
                Data.Instance.SelectedSeed = Data.Instance.Seeds.ElementAt(0).Key;

            Graphic.DisplayRectAndBounds(0, 0, 64, 64, 2, 3, 1, 0);
            Graphic.DisplayRectAndBounds(1, 1, 62, 62, 1, 2, 1, 0);

            int x = 4, y = 4;
            int i = 0;
            var seedsofpage = Data.Instance.Seeds.Skip(5 * page).Take(4).ToDictionary(pair => pair.Key, pair => pair.Value);
            int id = seedsofpage.Keys.ToList().IndexOf(Data.Instance.SelectedSeed);

            void disp(string what)
            {
                //string name = what.Split('_')[0];
                if (!seedsofpage.ContainsKey(what))
                    return;

                Type type = Type.GetType($"DOSBOX.Suggestions.plants.Fruits.{what}");
                if (type == null)
                    return;
                obj = (Dispf)Activator.CreateInstance(type, new[] { new vecf(x, y) });

                int h = Math.Max(obj._h, 5) + 2;

                if (i == id)
                    Graphic.DisplayRectAndBounds(x, y, 56, h + 2, 4, 2, 1, 1);
                else
                    Graphic.DisplayRect(x, y, 56, h + 2, 4, 1);
                x+=2;y+=2;
                obj.vec = new vecf(x, y);
                obj.Display(1);
                string txt = "" + seedsofpage[what];
                Text.DisplayText(txt, 60 - 5 * txt.Length, y, 1);
                x = 4;
                y += h + 3;
            }

            foreach (var seed in seedsofpage.Keys)
            {
                disp(seed);
                i++;
            }
        }
    }
}
