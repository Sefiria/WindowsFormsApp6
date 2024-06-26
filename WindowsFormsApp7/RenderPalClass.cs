﻿using System;
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
        };
        private static UIButton PalTickValueButton;
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
        public static int EditingPixelId = -1;
        public static void SecureId()
        {
            while (m_SelectedPixelId >= Pixels.Count) m_SelectedPixelId -= (byte) Pixels.Count;
            while (m_SelectedPixelId < 0) m_SelectedPixelId += (byte) Pixels.Count;
        }
        public static void Increase() => SelectedPixelId++;
        public static void Decrease() => SelectedPixelId--;
        public static ColorDialog ColorDialog = new ColorDialog();


        public static void Initialize()
        {
            UI.Clear();

            EditingPixelId = -1;
            SelectedPixelId = 0;

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
            b.OnClick += (s, e) => { var dial = new FormConfigSize(); if (dial.ShowDialog(Core.MainForm) == DialogResult.OK && dial.TilesHasChanged) RenderClass.Resize((int)dial.ResultW, (int)dial.ResultH); };
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

            x = 2;
            y = 100;

            b = new UIButton($"TickVal:{100}", x, y, Color.White, Color.Gray);
            b.Text = $"TickVal:{Core.PalTickValue}";
            b.Image = b.GenerateButtonImage();
            b.Tag = $"Config TickValue";
            b.OnClick += (s, e) => ChangePalTickValue();
            b.BoundWidth = 2F;
            PalTickValueButton = b;
            UI.Add(b);
            x += b.Image.Width + 6;

            DisplayPixelsUI();
        }
        private static void ResetPalTickValueButtonDisplay()
        {
            PalTickValueButton.Text = $"TickVal:{Core.PalTickValue}";
            PalTickValueButton.Image = PalTickValueButton.GenerateButtonImage();
        }
        private static void ChangePalTickValue()
        {
            if (Core.PalTickValue == 100) Core.PalTickValue = -1;
            if (Core.PalTickValue == 50) Core.PalTickValue = 100;
            if (Core.PalTickValue == 25) Core.PalTickValue = 50;
            if (Core.PalTickValue == 10) Core.PalTickValue = 25;
            if (Core.PalTickValue == 5) Core.PalTickValue = 10;
            if (Core.PalTickValue == 1) Core.PalTickValue = 5;
            if (Core.PalTickValue == -1) Core.PalTickValue = 1;
            ResetPalTickValueButtonDisplay();
        }
        private static void DisplayPixelsUI()
        {
            UI.Where(ui => ui.Tag.StartsWith("PixelRef") || ui.Tag.StartsWith("PixelMisc")).ToList().ForEach(ui => UI.Remove(ui));

            UIButton b;
            Bitmap img = new Bitmap(PalTileSZ, PalTileSZ);
            Graphics g = Graphics.FromImage(img);
            int x = 1;
            int y = 150;
            for (byte i = 0; i < Pixels.Count; i++)
            {
                if (2 + (x + 1) * (PalTileSZ + 2) >= Core.RPW)
                {
                    x = 0;
                    y += PalTileSZ + 2;
                }

                g.Clear(Pixels[i].Gradient[0]);
                g.DrawRectangle(Pens.White, x * PalTileSZ, y, PalTileSZ, PalTileSZ);

                b = new UIButton(img, 2 + x * (PalTileSZ + 2), y);
                b.Tag = $"PixelRef {i}";
                b.LinkedPixelId = i;
                b.OnClick += (s, e) => SelectedPixelId = (byte)(s as UIButton).LinkedPixelId;
                UI.Add(b);

                x++;
            }
            g.Dispose();

            b = new UIButton("-", 2, y, w: PalTileSZ, h: PalTileSZ, margin: 0);
            b.Tag = $"PixelMisc Remove PixelRef";
            b.OnClick += (s, e) => { if (Pixels.Count > 1) { Pixels.RemoveAt(Pixels.Count - 1); FIxRenderPixels(Pixels.Count); DisplayPixelsUI(); } };
            UI.Add(b);
            b = new UIButton("+", 2 + x * (PalTileSZ + 2), y, w: PalTileSZ, h: PalTileSZ, margin: 0);
            b.Tag = $"PixelMisc Add PixelRef";
            b.OnClick += (s, e) => { Pixels.Add(new Pixel(Pixels.Last())); DisplayPixelsUI(); };
            UI.Add(b);

            LastPalY = y;
        }

        private static void FIxRenderPixels(int removedId)
        {
            for (int x = 0; x < Core.RWT; x++)
                for (int y = 0; y < Core.RHT; y++)
                    if (RenderClass.Pixels[x, y] == removedId)
                    {
                        RenderClass.Pixels[x, y] = 0;
                        RenderClass.ModifiedPixels.Add(new Point(x, y));
                    }
            if (SelectedPixelId == removedId) SelectedPixelId--;
            DestroyUIEditing();
        }

        private static void LoadImg()
        {
            var dial = new OpenFileDialog();
            dial.Filter = "TIL files | *.til";
            if (dial.ShowDialog() != DialogResult.OK) return;
            string content = File.ReadAllText(dial.FileName).UnZip();
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length != 5) return;
            if (!int.TryParse(lines[0], out int rw)) return;
            if (!int.TryParse(lines[1], out int rh)) return;
            if (!int.TryParse(lines[2], out int tsz)) return;
            if (!int.TryParse(lines[3], out int iten)) return;
            if (lines[4].Length != (rw / tsz) * (rh / tsz)) return;
            Core.RW = rw;
            Core.RH = rh;
            Core.IterationsCount = iten;
            RenderClass.Pixels = new byte[rw / tsz, rh / tsz];
            byte px;
            for (int x = 0; x < rw / tsz; x++)
            {
                for (int y = 0; y < rh / tsz; y++)
                {
                    px = (byte)(lines[4][x * (rh / tsz) + y] - '0');
                    RenderClass.Pixels[x, y] = px < Pixels.Count ? px : (byte) 0;
                    RenderClass.ModifiedPixels.Add(new Point(x, y));
                }
            }
            Core.g.Clear(Color.Black);
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
            content += Core.IterationsCount + Environment.NewLine;
            for (int x = 0; x < Core.RWT; x++)
            {
                for (int y = 0; y < Core.RHT; y++)
                {
                    content += (char)('0'+RenderClass.Pixels[x, y]);
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
            Core.PalTickValue = int.Parse(lines[0]);
            ResetPalTickValueButtonDisplay();
            lines = lines.Skip(1).ToArray();
            Pixel p;
            List<Color> colors;
            Pixels.Clear();
            string line;
            foreach (string l in lines)
            {
                line = l;
                p = new Pixel();
                p.Gradient.Clear();
                p.IsLerp = int.Parse("" + line[0]) == 1;
                line = line.Remove(0, 1);
                colors = line.Split(',').Select(x => Color.FromArgb(int.Parse(x))).ToList();
                foreach (var c in colors)
                    p.Gradient.Add(c);
                Pixels.Add(p);
            }
            RenderClass.ModifiedPixels = RenderClass.GetAllPixelsPoints();
            DisplayPixelsUI();
        }
        private static void SavePal()
        {
            var dial = new SaveFileDialog();
            dial.Filter = "PAL files | *.pal";
            if (dial.ShowDialog() != DialogResult.OK) return;
            string content = Core.PalTickValue.ToString() + Environment.NewLine;
            foreach(var px in Pixels)
            {
                content += px.IsLerp ? "1" : "0";
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
                {
                    if (EditingPixelId != (clickedUI.First() as UIButton).LinkedPixelId)
                        LoadEditPixelUI((clickedUI.First() as UIButton).LinkedPixelId);
                    else
                        DestroyUIEditing();
                }
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


            list = UI.Where(ui => ui.Tag == null || new[] { "MISC", "PxRef", "PixelMisc" }.ToList().Any(tag => ui.Tag.StartsWith(tag))).ToList();
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

            list = UI.Where(ui => ui.Tag?.StartsWith("PixelRef") ?? false).ToList();
            var b = list.First(ui => list.IndexOf(ui) == SelectedPixelId);
            Core.gp.FillRectangle(Brushes.White, b.X + PalTileSZ / 2 - 1, b.Y - 8, 3, 4);
            if (EditingPixelId > -1)
            {
                b = list.First(ui => list.IndexOf(ui) == EditingPixelId);
                Core.gp.FillRectangle(Brushes.White, b.X + PalTileSZ / 2 - 1, b.Y + PalTileSZ + 5, 3, 4);
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
            DestroyUIEditing();
            EditingPixelId = pxid;

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
            b.Tag = $"MISC Remove Gradient";
            b.OnClick += (s, e) => { if (Pixels[pxid].Gradient.Count > 1) { Pixels[pxid].Gradient.RemoveAt(Pixels[pxid].Gradient.Count - 1); LoadEditPixelUI(pxid); } };
            UI.Add(b);
            b = new UIButton("+", 2 + x * (PalTileSZ + 2), y, w: PalTileSZ, h: PalTileSZ, margin: 0);
            b.Tag = $"MISC Add Gradient";
            b.OnClick += (s, e) => { Pixels[pxid].Gradient.Add(Pixels[pxid].Gradient.Last()); LoadEditPixelUI(pxid); };
            UI.Add(b);
            b = new UIButton(">", 2 + (x + 1) * (PalTileSZ + 2), y, w: PalTileSZ, h: PalTileSZ, margin: 0);
            b.Tag = $"MISC Shift Left Gradient";
            b.OnClick += (s, e) => { ShiftLeft(pxid); LoadEditPixelUI(pxid); };
            UI.Add(b);
            Bitmap GenIsLerpImg()
            {
                Bitmap imgL = new Bitmap(16, 16);
                using (Graphics gL = Graphics.FromImage(imgL))
                {
                    gL.Clear(Pixels[pxid].IsLerp ? Color.Lime : Color.Red);
                    gL.DrawRectangle(new Pen(Color.White, 2F), 0, 0, 16, 16);
                    gL.DrawRectangle(new Pen(Color.Black, 2F), 2, 2, 12, 12);
                }
                return imgL;
            }
            b = new UIButton(GenIsLerpImg(), b.X + b.W + 2, y);
            b.Tag = $"MISC isLerp";
            b.OnClick += (s, e) => { Pixels[pxid].IsLerp = !Pixels[pxid].IsLerp; b.Image = GenIsLerpImg(); };
            UI.Add(b);
        }

        private static void ShiftLeft(int pxid)
        {
            var px = Pixels[pxid];
            var g = new List<Color>();
            g.Add(px.Gradient[px.Gradient.Count - 1]);
            for (int i = 0; i < px.Gradient.Count - 1; i++)
                g.Add(px.Gradient[i]);
            Pixels[pxid].Gradient = g;
        }

        public static void LoadEditPixelGradientUI(int pxid, int gid)
        {
            ColorDialog.Color = Pixels[pxid].Gradient[gid];
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                Pixels[pxid].Gradient[gid] = ColorDialog.Color;
                LoadEditPixelUI(pxid);
                RenderClass.ModifiedPixels.AddRange(RenderClass.GetAllPixelsPoints());
            }
        }

        public static void DestroyUIEditing()
        {
            UI.Where(ui => ui is UIButton && ui.Tag != null && (ui.Tag.StartsWith("PxRef") || ui.Tag.StartsWith("MISC"))).ToList().ForEach(ui => UI.Remove(ui));
            EditingPixelId = -1;
        }
    }
}
