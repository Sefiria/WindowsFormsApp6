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

        int marge = 4;
        int buttonSz = 24;
        Rectangle rectButtonMinimize => new Rectangle(marge, marge, buttonSz, buttonSz);
        Rectangle rectButtonExit => new Rectangle(Width - marge - buttonSz, marge, buttonSz, buttonSz);
        Pen penButtons = new Pen(Color.Black, 2F);


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
                Core.CurrentSuggestion.ShowHowToPlay = true;
                Core.Cam = vecf.Zero;
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
            {
                if(Keyboard.IsKeyDown(Key.LeftCtrl) && Core.Suggestions[menu_selection].Name == "City")
                    Core.NextSuggestion = new CityEditor();
                else
                    Core.NextSuggestion = Core.Suggestions[menu_selection].Instance;
            }
        }

        private void Draw(object _, EventArgs e)
        {
            Bitmap Image = new Bitmap(320, 320);
            using (Graphics g = Graphics.FromImage(Image))
            {
                g.Clear(Color.FromArgb(140, 140, 140));
                g.DrawImage(CreateRender(), 32, 32);
                DrawHeader(g);
            }
            Render.Image = Image;
        }
        private void DrawHeader(Graphics g)
        {
            var rect = rectButtonMinimize;
            g.DrawRectangle(penButtons, rectButtonMinimize);
            g.DrawLine(penButtons, rect.X + rect.Width / 2 - buttonSz / 3, rect.Y + rect.Height / 2, rect.X + rect.Width / 2 + buttonSz / 3, rect.Y + rect.Height / 2);

            rect = rectButtonExit;
            g.DrawRectangle(penButtons, rectButtonExit);
            g.DrawLine(penButtons, rect.X + rect.Width / 2 - buttonSz / 3, rect.Y + rect.Height / 2 - buttonSz / 3, rect.X + rect.Width / 2 + buttonSz / 3, rect.Y + rect.Height / 2 + buttonSz / 3);
            g.DrawLine(penButtons, rect.X + rect.Width / 2 - buttonSz / 3, rect.Y + rect.Height / 2 + buttonSz / 3, rect.X + rect.Width / 2 + buttonSz / 3, rect.Y + rect.Height / 2 - buttonSz / 3);
        }

        Bitmap CreateRender()
        {
            Bitmap pic = new Bitmap(256, 256, PixelFormat.Format32bppArgb);

            int factor, i, j, w, h;
            for (int layer = 0; layer < Core.Layers.Count; layer++)
            {
                w = Core.Layers[layer].GetLength(0);
                h = Core.Layers[layer].GetLength(1);
                for (int x = 0; x < 256; x++)
                {
                    for (int y = 0; y < 256; y++)
                    {
                        i = (w > 64 ? Core.Cam.i.x : 0) + x / 4;
                        j = (h > 64 ? Core.Cam.i.y : 0) + y / 4;
                        if (layer > 0 && Core.Layers[layer][i, j] == 0)
                            continue;

                        pic.SetPixel(x, y, Core.Palette[Core.Layers[layer][i, j]]);
                    }
                }

                if (layer > 0)
                {
                    w = Core.Layers[layer].GetLength(0);
                    h = Core.Layers[layer].GetLength(1);
                    Core.Layers[layer] = new byte[w, h];
                }
            }



            return pic;
        }

        bool mousedown = false;
        Point mousepos;
        private void Render_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (rectButtonMinimize.Contains(e.Location))
            {
                WindowState = FormWindowState.Minimized;
                return;
            }
            if (rectButtonExit.Contains(e.Location))
            {
                Close();
                return;
            }


            mousedown = true;
            mousepos = e.Location;
        }
        private void Render_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mousedown = false;
        }
        private void Render_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!mousedown)
                return;
            Location = new Point(Location.X + (e.X - mousepos.X), Location.Y + (e.Y - mousepos.Y));
        }
    }
}
