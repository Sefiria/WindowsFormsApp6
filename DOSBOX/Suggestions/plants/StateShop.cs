using DOSBOX.Suggestions.plants;
using DOSBOX.Suggestions.plants.Fruits;
using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class Shop : IState
    {
        [JsonIgnore]
        List<Dictionary<string, int>> pages_itemsAndPrices = new List<Dictionary<string, int>>()
        {
            new Dictionary<string, int>(){
                ["water"] = 10,
                ["Pomme"] = 20,
                ["Tomate"] = 100,
            },
            new Dictionary<string, int>(){
                ["Concombre"] = 500,
                ["water\ntank"] = 500,
            },
            new Dictionary<string, int>(){
                ["water\nbucket A"] = 500,
                ["water\nbucket B"] = 5000,
            },
            new Dictionary<string, int>(){
                ["land A"] = 4999,
                ["land B"] = 15001,
            },
        };
        Dictionary<string, int> itemsAndPricesPerPage(int page) => page < 0 ? pages_itemsAndPrices[0] : (page >= pages_itemsAndPrices.Count ? pages_itemsAndPrices[pages_itemsAndPrices.Count - 1] : pages_itemsAndPrices[page]);
        Dictionary<string, int> itemsAndPrices => itemsAndPricesPerPage(page);
        int pagemax => pages_itemsAndPrices.Count - 1;
        int page;
        int selectid;
        effect.coin coin = new effect.coin();
        effect.waterdrop waterbucket = new effect.waterdrop();

        public void Init()
        {
            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            page = 0;
            selectid = 0;

            void remove_existing_item_of_shop(string item)
            {
                if (Data.Instance.Items.ContainsKey(item))
                    pages_itemsAndPrices.FirstOrDefault(x => x.ContainsKey(item))?.Remove(item);
            }
            remove_existing_item_of_shop("water\ntank");
            remove_existing_item_of_shop("water\nbucket A");
            remove_existing_item_of_shop("water\nbucket B");
            remove_existing_item_of_shop("land A");
            remove_existing_item_of_shop("land B");
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
            {
                page--;
                selectid = 0;
            }
            if (KB.IsKeyPressed(KB.Key.Right) && page < pages_itemsAndPrices.Count - 1)
            {
                page++;
                selectid = 0;
            }

            if (KB.IsKeyPressed(KB.Key.Space) && Data.Instance.Coins >= itemsAndPrices.ElementAt(selectid).Value)
            {
                Data.Instance.Coins -= itemsAndPrices.ElementAt(selectid).Value;
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

            _x = 32 - ("" + Data.Instance.Coins).Length * 5 / 2;
            coin.Display(1, v: new vec(_x - 2 - coin.w(0), y));
            Text.DisplayText("" + Data.Instance.Coins, _x, y, 1);
            y += 7;

            void disp(KeyValuePair<string, int> itemandprice)
            {
                string item = itemandprice.Key;
                int price = itemandprice.Value;
                bool enough_money = Data.Instance.Coins >= price;

                Type type = Type.GetType($"DOSBOX.Suggestions.plants.Fruits.{item}");
                if (type == null)
                    return;
                obj = (Dispf)Activator.CreateInstance(type, new[] { new vecf(x, y) });

                byte c = (byte)(enough_money ? 0 : 1);

                Int32Rect rect = new Int32Rect(x, y, 56, 10);
                Graphic.DisplayRectAndBounds(rect, c, (byte)(i == selectid ? 3 : c + 1), 1, 1);
                x+=2;y+=2;
                obj.vec = new vecf(x, y);
                obj.Display(1);
                string txt = "" + price;
                Text.DisplayText(txt, 60 - 5 * txt.Length, y, 1);
                x = 4;
                y += 10;
            }

            void dispeffect(KeyValuePair<string, int> itemandprice, effect effect = null)
            {
                bool firsttxt = effect != null;
                string[] split = itemandprice.Key.Split('\n');
                bool enough_money = Data.Instance.Coins >= itemandprice.Value;
                byte c = (byte)(enough_money ? 0 : 1);
                Int32Rect rect = new Int32Rect(x, y, 56, 10 * split.Length);
                Graphic.DisplayRectAndBounds(rect, c, (byte)(i == selectid ? 3 : c + 1), 1, 1);
                x += 2;
                y += 2;
                if (effect != null)
                {
                    effect.Display(1, v: new vec(x, y));
                    x += 2;
                }

                string txt = "" + itemandprice.Value;
                Text.DisplayText(txt, 60 - 5 * txt.Length, y, 1);

                foreach (string splittxt in split)
                {
                    Text.DisplayText(splittxt, x + (firsttxt ? effect.w(0) + 1 : 0), y, 1);
                    firsttxt = false;
                    y += 10;
                }
                x = 4;
            }

            foreach (KeyValuePair<string, int> item in itemsAndPrices)
            {
                switch(item.Key)
                {
                    default: disp(item); break;
                    case "water": dispeffect(item, waterbucket); break;
                    case "water\ntank":
                    case "water\nbucket A":
                    case "land A":
                    case "land B":
                    case "water\nbucket B": dispeffect(item); break;
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
                    if(Data.Instance.Seeds.ContainsKey(item.Key))
                        Data.Instance.Seeds[item.Key]++;
                    else
                        Data.Instance.Seeds.Add(item.Key, 1);
                    break;
                case "water":
                    Data.Instance.WaterBucket = Data.Instance.WaterBucketMax;
                    break;
                case "water\ntank":
                    pages_itemsAndPrices.First(x => x.ContainsKey(item.Key)).Remove(item.Key);
                    Data.Instance.Items.Add(item.Key, 1);
                    break;
                case "water\nbucket A":
                    pages_itemsAndPrices.First(x => x.ContainsKey(item.Key)).Remove(item.Key);
                    Data.Instance.Items.Add(item.Key, 1);
                    Data.Instance.WaterBucketMax = 200;
                    break;
                case "water\nbucket B":
                    pages_itemsAndPrices.FirstOrDefault(x => x.ContainsKey("water\nbucket A"))?.Remove("water\nbucket A");
                    pages_itemsAndPrices.First(x => x.ContainsKey(item.Key)).Remove(item.Key);
                    Data.Instance.Items.Add(item.Key, 1);
                    Data.Instance.WaterBucketMax = 500;
                    break;
                case "land A":
                    pages_itemsAndPrices.First(x => x.ContainsKey(item.Key)).Remove(item.Key);
                    Data.Instance.Items.Add(item.Key, 1);
                    Data.Garden.MapWidth = 200;
                    break;
                case "land B":
                    pages_itemsAndPrices.FirstOrDefault(x => x.ContainsKey("land A"))?.Remove("land A");
                    pages_itemsAndPrices.First(x => x.ContainsKey(item.Key)).Remove(item.Key);
                    Data.Instance.Items.Add(item.Key, 1);
                    Data.Garden.MapWidth = 500;
                    break;
            }
        }
    }
}
