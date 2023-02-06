using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RetfarC
{
    public class Data
    {
        public static int DMG = 10;
        public static List<Item> Items = new List<Item>();

        private static List<(int timer, Item item)> UITimerAndLastAddedItem = new List<(int timer, Item item)>();

        public static void AddItem(Item item)
        {
            void addlastitem(Item _i)
            {
                var _it = UITimerAndLastAddedItem.FirstOrDefault(i => i.item.GetType() == _i.GetType()).item;
                if (_it != null)
                {
                    UITimerAndLastAddedItem.RemoveAt(UITimerAndLastAddedItem.FindIndex(x => x.item == _i));
                    UITimerAndLastAddedItem.Add((500, _it));
                }
                else
                {
                    UITimerAndLastAddedItem.Add((500, _i));
                }
            }

            var it = Items.FirstOrDefault(i => i.GetType() == item.GetType());
            if (it != null)
            {
                it.Count += item.Count;
                addlastitem(it);
            }
            else
            {
                Items.Add(item);
                addlastitem(item);
            }
        }

        public static void DrawUI()
        {
            DrawLastAddedItems();
            DrawInventory();
        }
        private static void DrawLastAddedItems()
        {
            int timer;
            Item item;
            (int timer, Item item) sui;
            SizeF sz;
            string str;
            float w, h;
            Brush b;
            Bitmap img;
            var list = new List<(int timer, Item item)>(UITimerAndLastAddedItem);
            foreach (var ui in list)
            {
                timer = ui.timer;
                item = ui.item;
                if (timer > 0)
                {
                    str = $"{item.GetType().ToString().Split('.').Last()} ({item.Count})";
                    img = item.Image;
                    sz = Core.g.MeasureString(str, Core.Font);
                    h = sz.Height + 5;
                    Core.g.DrawImage(img, Core.W - img.Width, 5 + (list.Count - list.IndexOf(ui) - 1) * h);
                    w = sz.Width + img.Width + 5;
                    b = timer > 255 ? Brushes.White : new SolidBrush(Color.FromArgb(timer, Color.White));
                    Core.g.DrawString(str, Core.Font, b, Core.W - w, 5 + (list.Count - list.IndexOf(ui) - 1) * h);

                    sui = UITimerAndLastAddedItem[UITimerAndLastAddedItem.IndexOf(ui)];
                    UITimerAndLastAddedItem[UITimerAndLastAddedItem.IndexOf(ui)] = (sui.timer - 2, sui.item);
                }
                else
                {
                    UITimerAndLastAddedItem.Remove(ui);
                }
            }
        }
        private static void DrawInventory()
        {
            if(Core.ShowInventory)
            {
                var items = new List<Item>(Items);
                int x = 0, y = 0;
                int widthest = 0;
                string str;
                SizeF sz;
                Bitmap img;
                foreach (var item in items)
                {
                    img = item.Image;
                    str = $"{item.GetType().ToString().Split('.').Last()} ({item.Count})";
                    Core.g.DrawImage(img, 4 + x, 4 + y);
                    sz = Core.g.MeasureString(str, Core.Font);
                    sz.Width += img.Width + 5;
                    Core.g.FillRectangle(Brushes.Black, 4 + x + img.Width + 5, 4 + y, sz.Width + 2, sz.Height + 2);
                    Core.g.DrawString(str, Core.Font, Brushes.White, 5 + x + img.Width + 5, 5 + y);
                    if (widthest < sz.Width)
                        widthest = (int)sz.Width;
                    y += (int)sz.Height + 5;
                    if(y + (int)sz.Height > Core.H)
                    {
                        y = 0;
                        x += widthest + 15;
                    }
                }
            }
        }
    }
}
