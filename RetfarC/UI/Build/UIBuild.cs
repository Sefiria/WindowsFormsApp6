using RetfarC.UI.Build;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetfarC
{
    public static class UIBuild
    {
        public static bool Display = false;
        public static bool DisplayCraftNeeds = false;

        private static int SX = 0, SY = 0;
        private static List<UICategory> Categories = new List<UICategory>()
        {
            new UICategory_Production(),
            //new UICategory_Test1(),
            //new UICategory_Test2(),
        };

        public static void KeyDown(KeyEventArgs e)
        {
            if (!Display)
                return;

            if (DisplayCraftNeeds == false)
            {
                if (e.KeyCode == Keys.Z && SY > 0)
                {
                    SY--;
                    SX = 0;
                }
                if (e.KeyCode == Keys.S && SY < Categories.Count - 1)
                {
                    SY++;
                    SX = 0;
                }
                if (e.KeyCode == Keys.Q && SX > 0)
                {
                    SX--;
                }
                if (e.KeyCode == Keys.D && SX < Categories[SY].Structures.Count - 1)
                {
                    SX++;
                }

                if (e.KeyCode == Keys.Enter)
                    DisplayCraftNeeds = true;
            }
            else
            {
                if (e.KeyCode == Keys.Escape)
                    DisplayCraftNeeds = false;
                if (e.KeyCode == Keys.Enter)
                    CraftIt();
            }
        }

        private static void CraftIt()
        {
        }

        public static void Draw()
        {
            if (!Display) return;

            if (DisplayCraftNeeds == false)
            {
                int x = 5, y = Core.Hh - ((Categories.Count - 1) * (32 + 5) + (32 * 2 + 5)) / 2;

                for (int c = 0; c < Categories.Count; c++)
                {
                    if (SY == c)
                    {
                        Core.g.DrawImage(new Bitmap(Categories[c].Image, 32 * 2, 32 * 2), 5, y);

                        x = 5 + (32 * 2) + 5;
                        for (int s = 0; s < Categories[SY].Structures.Count; s++)
                        {
                            var img = Categories[SY].Structures[s].Image;
                            Core.g.DrawImage(SX == s ? new Bitmap(img, (int)(32 * 1.5F), (int)(32 * 1.5F)) : img, x, y + (SX == s ? 8 : 16));
                            x += SX == s ? (int)(32 * 1.5F) : 32;
                        }
                        y += (32 * 2) + 5;
                    }
                    else
                    {
                        Core.g.DrawImage(Categories[c].Image, 5, y);
                        y += 32 + 5;
                    }
                }
            }
            else
            {
                Core.g.FillRectangle(Brushes.DimGray, Core.W * 0.1F, Core.H * 0.1F, Core.W * 0.8F, Core.H * 0.8F);
                var needs = Categories[SY].Structures[SX].Needs;
                int x = (int)(Core.W * 0.1F) + 10, y = (int)(Core.H * 0.1F) + 10;
                int widthest = 0;
                string str;
                SizeF sz;
                Bitmap img;
                foreach (var need in needs)
                {
                    img = need.Image;
                    str = $"{need.GetType().ToString().Split('.').Last()} ({need.Count})";
                    sz = Core.g.MeasureString(str, Core.Font);
                    sz.Width += img.Width + 5;
                    Core.g.FillRectangle(Brushes.Black, x, 4 + y, sz.Width + 7, sz.Height + 2);
                    Core.g.DrawImage(img, 4 + x, 4 + y);
                    Core.g.DrawString(str, Core.Font, Brushes.White, 5 + x + img.Width + 5, 5 + y);
                    if (widthest < sz.Width)
                        widthest = (int)sz.Width;
                    y += (int)sz.Height + 5;
                    if (y + (int)sz.Height > Core.H * 0.8F - 10)
                    {
                        y = 0;
                        x += widthest + 15;
                    }
                }
            }
        }
    }
}
