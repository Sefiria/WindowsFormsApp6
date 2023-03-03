using DOSBOX.Suggestions.plants.Fruits;
using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DOSBOX.Suggestions
{
    public class Fruits: IState
    {
        public void Init()
        {
            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI
            
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Plants.Instance.CurrentState = null;
                return;
            }

            DisplayUI();
        }

        private void DisplayUI()
        {
            Dispf obj;

            Graphic.DisplayRectAndBounds(0, 0, 64, 64, 2, 3, 1, 0);
            Graphic.DisplayRectAndBounds(1, 1, 62, 62, 1, 2, 1, 0);

            int x = 4, y = 4;

            void disp(string what)
            {
                if (!plants.Data.Fruits.ContainsKey(what))
                    return;

                Type type = Type.GetType($"DOSBOX.Suggestions.plants.Fruits.{what}");
                if (type == null)
                    return;
                obj = (Dispf)Activator.CreateInstance(type, new[] { new vecf(x, y) });

                int h = Math.Max(obj._h, 5);

                Graphic.DisplayRect(x, y, 56, h + 2, 4, 1);
                x++;y++;
                obj.vec = new vecf(x, y);
                obj.Display(1);
                string txt = "" + plants.Data.Fruits[what];
                Text.DisplayText(txt, 64 - 3 - 5 * txt.Length, y, 1);
                x = 4;
                y += h + 3;
            }

            foreach(var fruit in plants.Data.Fruits.Keys)
                disp(fruit);
        }
    }
}
