using DOSBOX.Suggestions.plants;
using DOSBOX.Suggestions.plants.Fruits;
using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace DOSBOX.Suggestions
{
    public class Sell : IState
    {
        int pagemax => (int)Math.Ceiling(Data.Instance.FruitsAndItems.Count / 2F) - 1;
        int page;
        int selectid;
        effect.coin coin = new effect.coin();

        public void Init()
        {
            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            page = 0;
            selectid = 0;
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Plants.Instance.CurrentState = null;
                return;
            }

            if (KB.IsKeyPressed(KB.Key.Up) && selectid > 0)
                selectid--;
            if (KB.IsKeyPressed(KB.Key.Down) && selectid < 1)
                selectid++;
            if (KB.IsKeyPressed(KB.Key.Left) && page > 0)
            {
                page--;
                selectid = 0;
            }
            if (KB.IsKeyPressed(KB.Key.Right) && page < pagemax)
            {
                page++;
                selectid = 0;
            }

            if (KB.IsKeyPressed(KB.Key.Space))
                sell();

            DisplayUI();
        }

        private void DisplayUI()
        {
            Dispf obj;

            Graphic.DisplayRectAndBounds(0, 0, 64, 64, 1, 2, 1, 0);
            Graphic.DisplayRectAndBounds(1, 1, 62, 62, 0, 1, 1, 0);

            int x = 4, y = 4;
            int i = 0;

            int _x = 32 - ($"{page+1}/{pagemax+1}").Length * 5 / 2;
            Text.DisplayText($"{page + 1}/{pagemax + 1}", _x, 55, 1);

            _x = 32 - ("" + Data.Instance.Coins).Length * 5 / 2;
            coin.Display(1, v: new vec(_x - 2 - coin.w(0), y));
            Text.DisplayText("" + Data.Instance.Coins, _x, y, 1);
            y += 7;

            void disp(KeyValuePair<string, int> item)
            {
                bool own = Data.Instance.FruitsAndItems.ContainsKey(item.Key);

                Type type = Type.GetType($"DOSBOX.Suggestions.plants.Fruits.{item.Key}");
                if (type == null)
                    return;
                obj = (Dispf)Activator.CreateInstance(type, new[] { new vecf(x, y) });

                byte c = (byte)(own ? 0 : 1);

                Int32Rect rect = new Int32Rect(x, y, 56, 20);
                Graphic.DisplayRectAndBounds(rect, c, (byte)(i == selectid ? 3 : c + 1), 1, 1);
                x+=2;
                y+=2;
                obj.vec = new vecf(x, y);
                obj.Display(1);
                string price = "" + SellCatalogue.PriceOf(item.Key);
                Text.DisplayText("" + item.Value, 60 - 5 * ("" + item.Value).Length, y, 1);
                Text.DisplayText(price, 60 - 5 * price.Length, y + 10, 1);
                x = 4;
                y += 20;
            }

            void dispeffect(KeyValuePair<string, int> item, effect effect = null)
            {
                bool own = Data.Instance.FruitsAndItems.ContainsKey(item.Key);
                byte c = (byte)(own ? 0 : 1);
                string[] split = item.Key.Split('\n');
                Int32Rect rect = new Int32Rect(x, y, 56, 20);
                Graphic.DisplayRectAndBounds(rect, c, (byte)(i == selectid ? 3 : c + 1), 1, 1);
                x+=2;
                y+=2;
                if (effect != null)
                {
                    effect.Display(1, v: new vec(x, y));
                    x += 2;
                }
                bool firsttxt = effect != null;
                string price = "" + SellCatalogue.PriceOf(item.Key);
                Text.DisplayText("" + item.Value, 60 - 5 * ("" + item.Value).Length, y, 1);
                Text.DisplayText(price, 60 - 5 * price.Length, y + 10, 1);
                foreach (string splittxt in split)
                {
                    Text.DisplayText(splittxt, x + (firsttxt ? effect.w(0) + 1 : 0), y, 1);
                    firsttxt = false;
                    y += 10;
                }
                x = 4;
            }

            var list = Data.Instance.FruitsAndItems.Skip(2 * page).Take(2);
            foreach (KeyValuePair<string, int> item in list)
            { 
                if(Data.Instance.Fruits.ContainsKey(item.Key))
                    disp(item);
                else
                    dispeffect(item);
                i++;
            }
        }

        private void sell()
        {
            if (Data.Instance.FruitsAndItems.Count == 0)
                return;

            void Remove(string key)
            {
                if(Data.Instance.Fruits.ContainsKey(key))
                {
                    Data.Instance.Fruits[key]--;
                    if (Data.Instance.Fruits[key] == 0)
                        Data.Instance.Fruits.Remove(key);
                }
                else
                {
                    Data.Instance.Items[key]--;
                    if (Data.Instance.Items[key] == 0)
                        Data.Instance.Items.Remove(key);
                }
            }

            var list = Data.Instance.FruitsAndItems.Skip(2 * page).Take(2);
            var item = list.ElementAt(selectid);
            Data.Instance.Coins += SellCatalogue.PriceOf(item.Key);
            Remove(item.Key);
        }
    }
}
