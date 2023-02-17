using DOSBOX.Suggestions;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DOSBOX
{
    public partial class DOSBOX : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        public DOSBOX()
        {
            InitializeComponent();

            KB.Init();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            if (Core.NextSuggestion != null)
            {
                Core.CurrentSuggestion = Core.NextSuggestion;
                Core.NextSuggestion = null;
                Core.CurrentSuggestion.Init();
            }

            if (Core.CurrentSuggestion == null)
            {
                UpdateMenu();

                if (KB.IsKeyPressed(KB.Key.Escape))
                    Dispose();
            }
            else
            {
                Core.CurrentSuggestion.Update();
            }

            KB.Update();
        }

        int menu_selection = 0;
        private void UpdateMenu()
        {
            if (Core.Layers.Count != 2)
            {
                Core.Layers.Clear();
                Core.Layers.Add(new byte[64, 64]); // BG
                Core.Layers.Add(new byte[64, 64]); // UI
                Graphic.Clear(0, 0);
                Graphic.Clear(0, 1);
            }

            if (KB.IsKeyPressed(KB.Key.Up) && menu_selection > 0)
                menu_selection--;
            if (KB.IsKeyPressed(KB.Key.Down) && menu_selection < Core.Suggestions.Count - 1)
                menu_selection++;

            Graphic.Clear(2, 0);
            Graphic.Clear(0, 1);
            Graphic.DisplayRectAndBounds(2, 2, 60, 60, 0, 1, 1, 0);
            int x = 6, y = 6, w = 56, h = 9;
            Graphic.DisplayRectAndBounds(x - 2, y - 2 + menu_selection * (h+  1), w, h, 0, 1, 1, 1);
            for (int i = 0; i < Core.Suggestions.Count; i++)
            {
                Utilities.Text.DisplayText(Core.Suggestions[i].Name, x, y, 1);
                y += h + 1;
            }

            if (KB.IsKeyDown(KB.Key.Enter))
                Core.NextSuggestion = Core.Suggestions[menu_selection].Instance;
        }

        private void Draw(object _, EventArgs e)
        {
            Bitmap Image = new Bitmap(320, 320);
            using (Graphics g = Graphics.FromImage(Image))
            {
                g.Clear(Color.FromArgb(140, 140, 140));
                g.DrawImage(CreateRender(), 32, 32);
            }
            Render.Image = Image;
        }

        Bitmap CreateRender()
        {
            Bitmap pic = new Bitmap(256, 256, PixelFormat.Format32bppArgb);

            int length, factor;

            for(int layer=0; layer<Core.Layers.Count; layer++)
            {
                length = Core.Layers[layer].GetLength(0);
                factor = 256 / length;
                for (int x = 0; x < 256; x++)
                {
                    for (int y = 0; y < 256; y++)
                    {
                        if (layer > 0 && Core.Layers[layer][x / factor, y / factor] == 0)
                            continue;
                        
                        pic.SetPixel(x, y, Core.Palette[Core.Layers[layer][x / factor, y / factor]]);
                    }
                }

                if (layer > 0)
                {
                    int w = Core.Layers[layer].GetLength(0);
                    int h = Core.Layers[layer].GetLength(1);
                    Core.Layers[layer] = new byte[w, h];
                }
            }



            return pic;
        }
    }
}
