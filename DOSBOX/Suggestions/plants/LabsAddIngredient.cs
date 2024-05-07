using DOSBOX.Suggestions.plants;
using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class LabsAddIngredient : IState
    {
        List<Dictionary<string, int>> pagesLabsItems;
        Dictionary<string, int> LabsItemsPerPage(int page) => page < 0 ? pagesLabsItems[0] : (page >= pagesLabsItems.Count ? pagesLabsItems[pagesLabsItems.Count - 1] : pagesLabsItems[page]);
        Dictionary<string, int> LabsItems => LabsItemsPerPage(page);
        int pagemax => pagesLabsItems.Count - 1;
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

            // create pages
            pagesLabsItems = new List<Dictionary<string, int>>();
            const int itemsPerPage = 4;
            for(int i=0; i< Data.Instance.LabsItems.Count; i+= itemsPerPage)
                pagesLabsItems.Add(Data.Instance.LabsItems.Skip(i * itemsPerPage).Take(itemsPerPage).ToDictionary(pair => pair.Key, value => value.Value));
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Labs.ShowAddIngredient = false;
                return;
            }

            if (KB.IsKeyPressed(KB.Key.Up) && selectid > 0)
                selectid--;
            if (KB.IsKeyPressed(KB.Key.Down) && selectid < LabsItems.Count - 1)
                selectid++;
            if (KB.IsKeyPressed(KB.Key.Left) && page > 0)
            {
                page--;
                selectid = 0;
            }
            if (KB.IsKeyPressed(KB.Key.Right) && page < pagesLabsItems.Count - 1)
            {
                page++;
                selectid = 0;
            }

            if (KB.IsKeyPressed(KB.Key.Space) && LabsItems.Count > 0)
            {
                AddIngredient();
            }

            DisplayUI();
        }

        private void DisplayUI()
        {
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

            foreach (KeyValuePair<string, int> item in LabsItems)
            {
                dispeffect(item);
                i++;
            }
        }

        private void AddIngredient()
        {
            var item = LabsItems.ElementAt(selectid);

            LabsItems[item.Key]--;
            if (LabsItems[item.Key] <= 0)
                LabsItems.Remove(item.Key);

            Data.Instance.LabsItems[item.Key]--;
            if(Data.Instance.LabsItems[item.Key] <= 0)
                Data.Instance.LabsItems.Remove(item.Key);

            Labs.IngredientToAdd = item.Key;
            Labs.ShowAddIngredient = false;
        }
    }
}
