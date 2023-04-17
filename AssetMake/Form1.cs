using AssetMake.main;
using AssetMake.Properties;
using AssetMake.UI;
using AssetMake.utilities;
using AssetMake.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AssetMake
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        MouseEventArgs LastMouseEventArgs = null;
        string anim_to_play = "walk";
        int Ticks = 0;
        int TickMax => anim_to_play == "walk" ? 10 : 5;
        int AnimFrame = 0, AnimFrameMax = 16, loop = 0;
        public List<IUI> UI = new List<IUI>();

        public Form1()
        {
            InitializeComponent();

            Core.rw = Render.Width;
            Core.rh = Render.Height;
            Core.Image = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(Core.Image);

            Data.Instance.Init();
            KB.Init();
            Core.Init();

            InitInterface();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            if(LastMouseEventArgs != null)
                Render_MouseMove(null, LastMouseEventArgs);

            Tick();

            UpdateInterface();

            KB.Update();
        }

        private void Tick()
        {
            if (Ticks >= TickMax)
            {
                Ticks = 0;
                if (AnimFrame >= AnimFrameMax)
                {
                    AnimFrame = 0;
                }
                else
                    AnimFrame++;

                loop++;
                if (loop == (anim_to_play == "walk" ? 16 : 32))
                {
                    loop = 0;
                    anim_to_play = anim_to_play == "walk" ? "run" : "walk";
                }
            }
            else
                Ticks++;
        }

        private void Draw(object _, EventArgs e)
        {
            Core.Image = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(Core.Image);
            Core.g.Clear(Color.Black);

            DisplayInterface();

            Render.Image = Core.Image;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if(Core.MouseHolding == false)
            {
                MouseEventClickInterface(e);
            }

            Core.MouseHolding = true;
            Render_MouseMove(null, e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            Core.MouseHolding = false;
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            Core.MouseHolding = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            LastMouseEventArgs = e;
            Core.MouseLocation = e.Location;

            if (Core.MouseHolding)
            {
            }
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
        }








        int iw = 26, ih = 26;
        Asset asset;
        private void InitInterface()
        {
            Core.ImageInterface = new Bitmap(iw * Core.PixelScale, ih * Core.PixelScale);
            Core.gInterface = Graphics.FromImage(Core.ImageInterface);

            asset = new Asset(Resources.texture_street);

            int icosz = 16;
            Dictionary<string, (int Index, Bitmap Icon, Color Pixel)> customizables = new Dictionary<string, (int Index, Bitmap Icon, Color Pixel)>();
            void setpx(string key, Color c) => customizables[key] = (customizables[key].Index, customizables[key].Icon, c);
            void getpx(string key) => setpx(key, asset.Texture.GetPixel(customizables[key].Index, 0));
            void setIco(string key, Color c) { var img = new Bitmap(icosz, icosz); using (var g = Graphics.FromImage(img)) g.Clear(customizables[key].Pixel); customizables[key] = (customizables[key].Index, img, c); }
            void addCustom(string key) { customizables[key] = (customizables.Count, null, Color.Transparent); getpx(key); setIco(key, customizables[key].Pixel); }

            addCustom("Peau");
            addCustom("Cheveux");
            addCustom("T-Shirt");
            addCustom("Pantalon");
            addCustom("Chaussures");

            Color newcolor(Color c)
            {
                ColorDialog diag = new ColorDialog() { Color = c };
                if (diag.ShowDialog() == DialogResult.OK)
                    return diag.Color;
                return c;
            }

            UILabel l;
            int x = 200, y = 50, iy = 0;

            void click(string key)
            {
                var custom = customizables[key];
                var c = newcolor(custom.Pixel);
                asset.Texture.SetPixel(custom.Index, 0, c);
                if(custom.Index == 0) asset.Texture.SetPixel(custom.Index, 1, Color.FromArgb(c.R / 2, c.G / 2, c.B / 2));
                asset.LoadTexturesToAnims();
                setpx(key, c);
                setIco(key, c);
            }
            void CreateCustom(string key)
            {
                var custom = customizables[key];
                l = new UILabel(key, x, y + iy);
                UI.Add(l);
                UIButton b = new UIButton(custom.Icon, x + 100, y + iy);
                UI.Add(b);
                UI.Last().OnClick += (s, e) => {
                    click(key);
                    custom = customizables[key];
                    b.Icon = custom.Icon;
                    b.Image = b.GenerateButtonImage();
                };
                y += 40;
            }

            foreach(var customizable in customizables)
                CreateCustom(customizable.Key);
        }
        private void MouseEventClickInterface(MouseEventArgs e)
        {
            var clickedUI = UI.Where(i => e.X >= i.X && e.X < i.X + i.W && e.Y >= i.Y && e.Y < i.Y + i.H).ToList();
            if (clickedUI.Count > 0)
                clickedUI.ForEach(i => i.Clicked());
        }
        private void UpdateInterface()
        {
        }
        private void DisplayInterface()
        {
            Core.ImageInterface = new Bitmap(iw * Core.PixelScale, ih * Core.PixelScale);
            Core.gInterface = Graphics.FromImage(Core.ImageInterface);

            asset.Display(Core.gInterface, anim_to_play, AnimFrame, vecf.Zero);

            Core.g.DrawImage(Core.ImageInterface, 50, 50);

            UI.ForEach(x => x.Draw());
        }
    }
}
