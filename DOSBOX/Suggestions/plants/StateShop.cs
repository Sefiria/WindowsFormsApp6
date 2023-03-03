using DOSBOX.Suggestions.plants.Fruits;
using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DOSBOX.Suggestions
{
    public class Shop : IState
    {
        List<Dictionary<string, int>> pages_itemsAndPrices = new List<Dictionary<string, int>>()
        {
            new Dictionary<string, int>(){
                ["water"] = 10,
                ["Pomme"] = 20,
                ["Tomate"] = 100,
            },
            new Dictionary<string, int>(){
                ["Concombre"] = 500,
            },
        };
        Dictionary<string, int> itemsAndPricesPerPage(int page) => page < 0 ? pages_itemsAndPrices[0] : (page >= pages_itemsAndPrices.Count ? pages_itemsAndPrices[pages_itemsAndPrices.Count - 1] : pages_itemsAndPrices[page]);
        Dictionary<string, int> itemsAndPrices => itemsAndPricesPerPage(page);
        int pagemax => pages_itemsAndPrices.Count - 1;
        int page = 0;
        int selectid = 0;
        effect.coin coin = new effect.coin();
        effect.waterdrop waterbucket = new effect.waterdrop();

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

            if (KB.IsKeyPressed(KB.Key.Up) && selectid > 0)
                selectid--;
            if (KB.IsKeyPressed(KB.Key.Down) && selectid < itemsAndPrices.Count - 1)
                selectid++;
            if (KB.IsKeyPressed(KB.Key.Left) && page > 0)
                page--;
            if (KB.IsKeyPressed(KB.Key.Right) && page < pages_itemsAndPrices.Count - 1)
                page++;

            if (KB.IsKeyPressed(KB.Key.Space) && plants.Data.Coins >= itemsAndPrices.ElementAt(selectid).Value)
            {
                plants.Data.Coins -= itemsAndPrices.ElementAt(selectid).Value;
                Buy();
            }

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

            _x = 32 - ("" + plants.Data.Coins).Length * 5 / 2;
            coin.Display(1, v: new vec(_x - 2 - coin.w(0), y));
            Text.DisplayText("" + plants.Data.Coins, _x, y, 1);
            y += 7;

            void disp(KeyValuePair<string, int> itemandprice)
            {
                string item = itemandprice.Key;
                int price = itemandprice.Value;
                bool enough_money = plants.Data.Coins >= price;

                Type type = Type.GetType($"DOSBOX.Suggestions.plants.Fruits.{item}");
                if (type == null)
                    return;
                obj = (Dispf)Activator.CreateInstance(type, new[] { new vecf(x, y) });

                int h = Math.Max(obj._h, 5) + 2;
                byte c = (byte)(enough_money ? 0 : 1);

                Graphic.DisplayRectAndBounds(x, y, 56, h + 2, c, (byte)(i == selectid ? 3 : c + 1), 1, 1);
                x+=2;y+=2;
                obj.vec = new vecf(x, y);
                obj.Display(1);
                string txt = "" + price;
                Text.DisplayText(txt, 60 - 5 * txt.Length, y, 1);
                x = 4;
                y += h + 1;
            }

            void dispeffect(KeyValuePair<string, int> itemandprice, effect effect)
            {
                bool enough_money = plants.Data.Coins >= itemandprice.Value;
                byte c = (byte)(enough_money ? 0 : 1);
                Graphic.DisplayRectAndBounds(x, y, 56, 9, c, (byte)(i == selectid ? 3 : c + 1), 1, 1);
                effect.Display(1, v: new vec(x + 2, y + 2));
                Text.DisplayText(itemandprice.Key, x + 2 + effect.w(0) + 1, y + 2, 1);
                string txt = "" + itemandprice.Value;
                Text.DisplayText(txt, 60 - 5 * txt.Length, y + 2, 1);
                y += 10;
            }

            foreach (KeyValuePair<string, int> item in itemsAndPrices)
            {
                switch(item.Key)
                {
                    default: disp(item); break;
                    case "water": dispeffect(item, waterbucket); break;
                }
                i++;
            }
        }

        private void Buy()
        {
            var item = itemsAndPrices.ElementAt(selectid);
            switch (item.Key)
            {
                default:
                    if(plants.Data.Seeds.ContainsKey(item.Key))
                        plants.Data.Seeds[item.Key]++;
                    else
                        plants.Data.Seeds.Add(item.Key, 1);
                    break;
                case "water":
                    plants.Data.WaterBucket = plants.Data.WaterBucketMax;
                    break;
            }
        }
    }
}
