using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp11.Items;
using System.Windows.Forms;

namespace WindowsFormsApp11
{
    public class UICraftingTable
    {
        public static Font Font = new Font("Segoe UI", 12F);

        public static void Draw()
        {
            Graphics g = Var.g;

            int X = 20;
            int Y = 20;

            CraftingTable.ItemsRectType.Clear();
            DrawItem<ItemBlue>(g, ref X, ref Y, Color.Blue);
            DrawItem<ItemGreen>(g, ref X, ref Y, Color.Lime);
            DrawItem<ItemRed>(g, ref X, ref Y, Color.Red);
            DrawItem<ItemYellow>(g, ref X, ref Y, Color.Yellow);
            DrawItem<ItemBlack>(g, ref X, ref Y, Color.Black);
            DrawItem<ItemWhite>(g, ref X, ref Y, Color.White);

            DrawCraftBox(g);
        }

        private static void DrawCraftBox(Graphics g)
        {
            var table = CraftingTable.Table;

            void DrawTable(int rowCount)
            {
                int x = 0, y = 0, sz = CraftingTable.CraftBox.Width / rowCount;
                foreach(var item in table)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(item.Color)),
                        CraftingTable.CraftBox.X + x * sz,
                        CraftingTable.CraftBox.Y + y * sz,
                        sz,
                        sz
                        );
                    x++;
                    if (x == rowCount)
                    {
                        x = 0;
                        y++;
                    }
                }
            }

            if (table.Count == 1)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(table[0].Color)), CraftingTable.CraftBox);
            }
            else if (table.Count <= 2 * 2)
            {
                DrawTable(2);
            }
            else if (table.Count <= 3 * 3)
            {
                DrawTable(3);
            }
            else if (table.Count <= 4 * 4)
            {
                DrawTable(4);
            }
            else if (table.Count <= 5 * 5)
            {
                DrawTable(5);
            }

            g.DrawRectangle(Pens.White, CraftingTable.CraftBox);
        }

        private static void DrawItem<T>(Graphics g, ref int X, ref int Y, Color c) where T:Item
        {
            string str;
            SizeF sz;

            CraftingTable.ItemsRectType.Add((new Rectangle(X, Y, 20, 20), typeof(T)));
            g.FillRectangle(new SolidBrush(c), X, Y, 20, 20);
            g.DrawRectangle(Pens.White, X, Y, 20, 20);
            X += 30;
            str = Var.Data.Ship.ItemPackage.CountOf<T>().ToString();
            sz = g.MeasureString(str, Font);
            g.DrawString(str, Font, Brushes.White, X, Y - 6);
            X += (int)(sz.Width) + 10;
        }
    }
}
