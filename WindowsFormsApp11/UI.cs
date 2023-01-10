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
    public class UI
    {
        public static Font Font = new Font("Segoe UI", 12F);

        public static void Draw()
        {
            Graphics g = Var.g;

            int X = 10;
            int Y = 10;

            DrawItem<ItemBlue>(g, ref X, ref Y, Color.Blue);
            DrawItem<ItemGreen>(g, ref X, ref Y, Color.Lime);
            DrawItem<ItemRed>(g, ref X, ref Y, Color.Red);
            DrawItem<ItemYellow>(g, ref X, ref Y, Color.Yellow);
            DrawItem<ItemBlack>(g, ref X, ref Y, Color.Black);
            DrawItem<ItemWhite>(g, ref X, ref Y, Color.White);
        }

        private static void DrawItem<T>(Graphics g, ref int X, ref int Y, Color c) where T:Item
        {
            string str;
            SizeF sz;

            g.FillRectangle(new SolidBrush(c), X, Y, 10, 10);
            g.DrawRectangle(Pens.White, X, Y, 10, 10);
            X += 15;
            str = Var.Data.Ship.ItemPackage.CountOf<T>().ToString();
            sz = g.MeasureString(str, Font);
            g.DrawString(str, Font, Brushes.White, X, Y - 6);
            X += (int)(sz.Width) + 10;
        }
    }
}
