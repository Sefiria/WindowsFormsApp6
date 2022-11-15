using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp7.UI;

namespace WindowsFormsApp7
{
    public class RenderPalClass
    {
        public static int PalTileSZ = 16;
        public static List<IUI> UI = new List<IUI>();
        public static List<Pixel> Pixels = new List<Pixel>()
        {
            new Pixel(),
            new Pixel(){Gradient = new List<Color>() { Color.White } },
            new Pixel() { Gradient = new List<Color>() {
            Color.FromArgb(255,   0,     0),
            Color.FromArgb(200,   0,   55),
            Color.FromArgb(150,   0, 105),
            Color.FromArgb(100,   0, 155),
            Color.FromArgb(  50,   0, 205),
            Color.FromArgb(    0,   0, 255),
            } },
            new Pixel() { Gradient = new List<Color>() {
            Color.FromArgb(200,   0,   55),
            Color.FromArgb(150,   0, 105),
            Color.FromArgb(100,   0, 155),
            Color.FromArgb(  50,   0, 205),
            Color.FromArgb(    0,   0, 255),
            Color.FromArgb(255,   0,     0),
            } },
            new Pixel() { Gradient = new List<Color>() {
            Color.FromArgb(150,   0, 105),
            Color.FromArgb(100,   0, 155),
            Color.FromArgb(  50,   0, 205),
            Color.FromArgb(    0,   0, 255),
            Color.FromArgb(255,   0,     0),
            Color.FromArgb(200,   0,   55),
            } },
            new Pixel() { Gradient = new List<Color>() {
            Color.FromArgb(100,   0, 155),
            Color.FromArgb(  50,   0, 205),
            Color.FromArgb(    0,   0, 255),
            Color.FromArgb(255,   0,     0),
            Color.FromArgb(200,   0,   55),
            Color.FromArgb(150,   0, 105),
            } },
            new Pixel() { Gradient = new List<Color>() {
            Color.FromArgb(  50,   0, 205),
            Color.FromArgb(    0,   0, 255),
            Color.FromArgb(255,   0,     0),
            Color.FromArgb(200,   0,   55),
            Color.FromArgb(150,   0, 105),
            Color.FromArgb(100,   0, 155),
            } },
            new Pixel() { Gradient = new List<Color>() {
            Color.FromArgb(    0,   0, 255),
            Color.FromArgb(255,   0,     0),
            Color.FromArgb(200,   0,   55),
            Color.FromArgb(150,   0, 105),
            Color.FromArgb(100,   0, 155),
            Color.FromArgb(  50,   0, 205),
            } },
        };
        public static int LastPalY = 0;
        private static byte m_SelectedPixelId = 0;
        public static byte SelectedPixelId
        {
            get => m_SelectedPixelId;
            set
            {
                m_SelectedPixelId = value;
                SecureId();
            }
        }
        public static void SecureId()
        {
            while (m_SelectedPixelId >= Pixels.Count) m_SelectedPixelId -= (byte) Pixels.Count;
            while (m_SelectedPixelId < 0) m_SelectedPixelId += (byte) Pixels.Count;
        }
        public static void Increase() => SelectedPixelId++;
        public static void Decrease() => SelectedPixelId--;


        public static void Initialize()
        {
            UI.Clear();

            UIButton b;
            int x = 2, y = 10;

            y = 10;
            x = 2;
            b = new UIButton("P", x, y, Color.White, Color.Gray);
            b.Tag = $"Tool Pen";
            b.OnClick += (s, e) => RenderClass.Tool = 0;
            b.BoundWidth = 2F;
            UI.Add(b);
            x += (int)Core.gp.MeasureString(b.Text, Control.DefaultFont).Width + 10 + b.Margin * 2;

            b = new UIButton("B", x, y, Color.White, Color.Gray);
            b.Tag = $"Tool Bucket";
            b.OnClick += (s, e) => RenderClass.Tool = 1;
            b.BoundWidth = 2F;
            UI.Add(b);
            x += (int)Core.gp.MeasureString(b.Text, Control.DefaultFont).Width + 10 + b.Margin * 2;

            x = 2;
            y = 50;

            b = new UIButton("Size", x, y, Color.White, Color.Gray);
            b.Tag = $"Config Size";
            b.OnClick += (s, e) => { var dial = new FormConfigSize(); if (dial.ShowDialog() == DialogResult.OK) RenderClass.Resize((int)dial.ResultW, (int)dial.ResultH, (int)dial.numTileSize.Value); };
            b.BoundWidth = 2F;
            UI.Add(b);
            x += b.Image.Width + 6;

            b = new UIButton("Load Pal", x, y, Color.White, Color.Gray);
            b.Tag = $"Config LoadPal";
            b.OnClick += (s, e) => LoadPal();
            b.BoundWidth = 2F;
            UI.Add(b);
            x += b.Image.Width + 6;

            b = new UIButton("Save Pal", x, y, Color.White, Color.Gray);
            b.Tag = $"Config SavePal";
            b.OnClick += (s, e) => SavePal();
            b.BoundWidth = 2F;
            UI.Add(b);
            x += b.Image.Width + 6;

            b = new UIButton("Load Img", x, y, Color.White, Color.Gray);
            b.Tag = $"Config LoadImg";
            b.OnClick += (s, e) => LoadImg();
            b.BoundWidth = 2F;
            UI.Add(b);
            x += b.Image.Width + 6;

            b = new UIButton("Save Img", x, y, Color.White, Color.Gray);
            b.Tag = $"Config SaveImg";
            b.OnClick += (s, e) => SaveImg();
            b.BoundWidth = 2F;
            UI.Add(b);
            x += b.Image.Width + 6;

            Bitmap img = new Bitmap(PalTileSZ, PalTileSZ);
            Graphics g = Graphics.FromImage(img);
            x = 0;
            y = 100;
            for (byte i=0; i < Pixels.Count; i++)
            {
                if(2 + (x + 1) * (PalTileSZ + 2) >= Core.RPW)
                {
                    x = 0;
                    y += PalTileSZ + 2;
                }

                g.Clear(Pixels[i].Gradient[0]);
                g.DrawRectangle(Pens.White, x * PalTileSZ, y, PalTileSZ, PalTileSZ);

                b = new UIButton(img, 2 + x * (PalTileSZ + 2), y);
                b.Tag = $"PixelRef {i}";
                b.LinkedPixelId = i;
                b.OnClick += (s, e) => SelectedPixelId = (byte) (s as UIButton).LinkedPixelId;
                UI.Add(b);

                x++;
            }
            g.Dispose();

            LastPalY = y;
        }
        private static void LoadImg()
        {
            var dial = new OpenFileDialog();
            dial.Filter = "TIL files | *.til";
            if (dial.ShowDialog() != DialogResult.OK) return;
            string content = File.ReadAllText(dial.FileName).UnZip();
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length != 4) return;
            if (!int.TryParse(lines[0], out int rw)) return;
            if (!int.TryParse(lines[1], out int rh)) return;
            if (!int.TryParse(lines[2], out int tsz)) return;
            if (lines[3].Length != (rw / tsz) * (rh / tsz)) return;
            Core.RW = rw;
            Core.RH = rh;
            Core.TileSz = tsz;
            RenderClass.Pixels = new byte[rw / tsz, rh / tsz];
            for (int x = 0; x < rw / tsz; x++)
            {
                for (int y = 0; y < rh / tsz; y++)
                {
                    RenderClass.Pixels[x, y] = (byte)int.Parse(""+lines[3][x * (rh / tsz) + y]);
                    RenderClass.ModifiedPixels.Add(new Point(x, y));
                }
            }
        }
        private static void SaveImg()
        {
            var dial = new SaveFileDialog();
            dial.Filter = "TIL files | *.til";
            if (dial.ShowDialog() != DialogResult.OK) return;
            string content = "";
            content += Core.RW + Environment.NewLine;
            content += Core.RH + Environment.NewLine;
            content += Core.TileSz + Environment.NewLine;
            for (int x = 0; x < Core.RWT; x++)
            {
                for (int y = 0; y < Core.RHT; y++)
                {
                    content += RenderClass.Pixels[x, y];
                }
            }
            File.WriteAllText(dial.FileName, content.Zip());
        }
        private static void LoadPal()
        {
            var dial = new OpenFileDialog();
            dial.Filter = "PAL files | *.pal";
            if (dial.ShowDialog() != DialogResult.OK) return;
            string content = File.ReadAllText(dial.FileName).UnZip();
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length != Pixels.Count) return;
            Pixel p;
            List<Color> colors;
            for (int i = 0; i < lines.Length; i++)
            {
                p = new Pixel();
                p.Gradient.Clear();
                colors = lines[i].Split(',').Select(x => Color.FromArgb(int.Parse(x))).ToList();
                foreach (var c in colors)
                    p.Gradient.Add(c);
                Pixels[i] = p;
            }
            RenderClass.ModifiedPixels = RenderClass.GetAllPixelsPoints();
        }
        private static void SavePal()
        {
            var dial = new SaveFileDialog();
            dial.Filter = "PAL files | *.pal";
            if (dial.ShowDialog() != DialogResult.OK) return;
            string content = "";
            foreach(var px in Pixels)
            {
                foreach (var c in px.GradientArgb)
                    content += c + (px.GradientArgb.IndexOf(c) < px.GradientArgb.Count - 1 ? "," : "");
                if(Pixels.IndexOf(px) < Pixels.Count - 1)
                    content += Environment.NewLine;
            }
            File.WriteAllText(dial.FileName, content.Zip());
        }

        public static void MouseDown(MouseEventArgs e)
        {
            var clickedUI = UI.Where(x => e.X >= x.X && e.X < x.X + x.W && e.Y >= x.Y && e.Y < x.Y + x.H).ToList();
            if (clickedUI.Count > 0)
            {
                if(clickedUI.First().Tag != null && clickedUI.First().Tag.StartsWith("PxRef"))
                    LoadEditPixelGradientUI((clickedUI.First() as UIButton).LinkedPixelId, (clickedUI.First() as UIButton).LinkedPixelGradiantId);

                if (e.Button == MouseButtons.Left)
                    clickedUI.ForEach(x => x.Clicked());
                else if (e.Button == MouseButtons.Right && clickedUI.First() is UIButton && clickedUI.First().Tag.StartsWith("PixelRef"))
                    LoadEditPixelUI((clickedUI.First() as UIButton).LinkedPixelId);
            }
        }
        public static void MouseMove(MouseEventArgs e)
        {
            var hoverUI = UI.Where(x => e.X >= x.X && e.X < x.X + x.W && e.Y >= x.Y && e.Y < x.Y + x.H).ToList();
            UI.ForEach(x => x.Hover  = false);
            hoverUI.ForEach(x => x.Hover  = true);
        }

        public static void Update()
        {
        }

        public static void Draw()
        {
            Core.gp.Clear(Color.Black);

            var list = UI.Where(ui => ui is UIButton && ui.Tag != null && !ui.Tag.StartsWith("PxRef") && (ui as UIButton).LinkedPixelId != -1).ToList();
            foreach (var ui in list)
            {
                using (Graphics g = Graphics.FromImage(ui.Image))
                    g.Clear(RenderClass.GetGradient((byte)(ui as UIButton).LinkedPixelId));
            }
            foreach (var ui in list)
            {
                Color c;
                int uiid = list.IndexOf(ui);
                if (ui.Hover)
                {
                    if (uiid == SelectedPixelId)
                        c = Color.Cyan;
                    else
                        c = Color.FromArgb(200, 200, 200);
                }
                else
                {
                    if (uiid == SelectedPixelId)
                        c = Color.White;
                    else
                        c = Color.FromArgb(150, 150, 150);
                }
                ui.Draw(Core.gp, c);
            }


            list = UI.Where(ui => ui.Tag == null || ui.Tag.StartsWith("MISC") || ui.Tag.StartsWith("PxRef")).ToList();
            foreach (var ui in list)
                ui.Draw(Core.gp);


            list = UI.Where(ui => ui is UIButton && ui.Tag != null && (ui.Tag.StartsWith("Tool") || ui.Tag.StartsWith("Config"))).ToList();
            foreach (var ui in list)
            {
                Color c;
                int uiid = list.IndexOf(ui);
                if (ui.Hover)
                {
                    if (uiid == RenderClass.Tool)
                        c = Color.Cyan;
                    else
                        c = Color.FromArgb(200, 200, 200);
                }
                else
                {
                    if (uiid == RenderClass.Tool)
                        c = Color.White;
                    else
                        c = Color.FromArgb(150, 150, 150);
                }
                ui.Draw(Core.gp, c);
            }
        }

        public static void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add || (!e.Shift && e.KeyCode == Keys.Tab))
                Increase();
            if (e.KeyCode == Keys.Subtract || (e.Shift && e.KeyCode == Keys.Tab))
                Decrease();

            var kvs = new List<int>() { 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 219, 187 };
            if (kvs.Contains(e.KeyValue))
                SelectedPixelId = (byte)kvs.IndexOf(e.KeyValue);

            if (e.KeyCode == Keys.OemQuotes)
                RenderClass.Tool = (byte)(RenderClass.Tool == 0 ? 1 : 0);
            if (e.KeyCode == Keys.P)
                RenderClass.Tool = 0;
            if (e.KeyCode == Keys.B)
                RenderClass.Tool = 1;
        }

        public static void LoadEditPixelUI(int pxid)
        {
            UI.Where(ui => ui is UIButton && ui.Tag != null && (ui.Tag.StartsWith("PxRef") || ui.Tag.StartsWith("MISC"))).ToList().ForEach(ui => UI.Remove(ui));

            UIButton b;
            Bitmap img = new Bitmap(PalTileSZ, PalTileSZ);
            Graphics g = Graphics.FromImage(img);
            int x = 1, y = LastPalY + (PalTileSZ + 2) * 2;
            for (byte i = 0; i < Pixels[pxid].Gradient.Count; i++)
            {
                if (2 + (x + 1) * (PalTileSZ + 2) >= Core.RPW)
                {
                    x = 0;
                    y++;
                }

                g.Clear(Pixels[pxid].Gradient[i]);
                g.DrawRectangle(Pens.DimGray, x * PalTileSZ, y, PalTileSZ, PalTileSZ);

                b = new UIButton(img, 2 + x * (PalTileSZ + 2), y);
                b.Tag = $"PxRef {pxid} Gradient {i}";
                b.LinkedPixelId = pxid;
                b.LinkedPixelGradiantId = i;
                UI.Add(b);

                x++;
            }
            g.Dispose();

            b = new UIButton("-", 2, y, w: PalTileSZ, h: PalTileSZ, margin: 0);
            b.Tag = $"MISC Add Gradient";
            b.OnClick += (s, e) => { if (Pixels[pxid].Gradient.Count > 1) { Pixels[pxid].Gradient.RemoveAt(Pixels[pxid].Gradient.Count - 1); LoadEditPixelUI(pxid); } };
            UI.Add(b);
            b = new UIButton("+", 2 + x * (PalTileSZ + 2), y, w: PalTileSZ, h: PalTileSZ, margin: 0);
            b.Tag = $"MISC Remove Gradient";
            b.OnClick += (s, e) => { Pixels[pxid].Gradient.Add(Pixels[pxid].Gradient.Last()); LoadEditPixelUI(pxid); };
            UI.Add(b);
        }
        public static void LoadEditPixelGradientUI(int pxid, int gid)
        {
            var dial = new ColorDialog() { Color = Pixels[pxid].Gradient[gid] };
            if (dial.ShowDialog() == DialogResult.OK)
            {
                Pixels[pxid].Gradient[gid] = dial.Color;
                LoadEditPixelUI(pxid);
                RenderClass.ModifiedPixels.AddRange(RenderClass.GetAllPixelsPoints());
            }
        }
    }
}
