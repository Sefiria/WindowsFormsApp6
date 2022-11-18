using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public class RenderClass
    {
        public static byte[,] Pixels = new byte[Core.RWT, Core.RHT];
        public static List<byte[,]> PixelsHistory = new List<byte[,]>();
        public static int MaxPixelsHistory = 8;
        public static List<Point> ModifiedPixels = new List<Point>();
        public static Bitmap Image = new Bitmap(Core.RW, Core.RH);
        public static byte Tool = 0;
        private static int m_GradientId = 0;
        public static int GradientId
        {
            get => m_GradientId;
            set
            {
                m_GradientId = value;
                SecureId();
            }
        }
        public static bool MouseRightDown = false;
        public static double t = 0D;
        private static int OfstX => Core.ExactRenderW / 2 - Core.RWxZ / 2;
        private static int OfstY => Core.ExactRenderH / 2 - Core.RHxZ / 2;

        public static byte[,] CopiedPixels()
        {
            var copy = new byte[Core.RWT, Core.RHT];
            for (int x = 0; x < Core.RWT; x++)
            {
                for (int y = 0; y < Core.RHT; y++)
                {
                    copy[x, y] = Pixels[x, y];
                }
            }
            return copy;
        }
        public static List<Point> GetAllPixelsPoints()
        {
            var all = new List<Point>();
            for (int x = 0; x < Core.RWT; x++)
            {
                for (int y = 0; y < Core.RHT; y++)
                {
                    all.Add(new Point(x, y));
                }
            }
            return all;
        }

        public static void SecureId()
        {
            while (m_GradientId >= Core.IterationsCount) m_GradientId -= Core.IterationsCount;
            while (m_GradientId < 0) m_GradientId += Core.IterationsCount;
        }
        public static void Increase()
        {
            t += 0.1;
            if (t >1D)
            {
                GradientId++;
                t = 0D;
            }
        }
        public static void Decrease() { GradientId--; t = 0F; }

        public static void Initialize()
        {
            for (int x = 0; x < Core.RWT; x++)
            {
                for (int y = 0; y < Core.RHT; y++)
                {
                    Pixels[x, y] = 0;
                    ModifiedPixels.Add(new Point(x, y));
                }
            }

            PixelsHistory.Add(CopiedPixels());
        }

        public static void MouseUpOrLeave()
        {
            MouseRightDown = false;
        }
        public static void MouseDown(MouseEventArgs e)
        {
            if (!IsMouseInRender(e)) return;

            if (MouseRightDown && e.X / Core.TileSzZoom < Pixels.GetLength(0) && e.Y / Core.TileSzZoom < Pixels.GetLength(1))
            {
                RenderPalClass.SelectedPixelId = Pixels[(e.X - OfstX) / Core.TileSzZoom, (e.Y - OfstY) / Core.TileSzZoom];
                RenderPalClass.LoadEditPixelUI(RenderPalClass.SelectedPixelId);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                AddPixelsHistory();

                if (Tool == 0)
                    MouseMove(e);
                else if (Tool == 1)
                {
                    if(!IsMouseInRender(e)) return;
                    Tools.FloodFill(new Point((e.X - OfstX) / Core.TileSzZoom, (e.Y - OfstY) / Core.TileSzZoom), RenderPalClass.SelectedPixelId);
                    SetImage();
                }
            }
        }
        public static bool IsMouseInRender(MouseEventArgs e)
        {
            var a = !(Core.MousePosition.X < OfstX || Core.MousePosition.X >= OfstX + Core.RWxZ || Core.MousePosition.Y < OfstY || Core.MousePosition.Y >= OfstY + Core.RHxZ);
            var b = !(e.X < OfstX || e.X >= OfstX + Core.RWxZ || e.Y < OfstY || e.Y >= OfstY + Core.RHxZ);
            return a && b;
        }
        public static void MouseMove(MouseEventArgs e)
        {
            if(!IsMouseInRender(e)) return;

            int eX = e.X - OfstX;
            int eY = e.Y - OfstY;

            if (MouseRightDown)
            {
                RenderPalClass.SelectedPixelId = Pixels[eX / Core.RWxZ, eY / Core.RHxZ];
                RenderPalClass.LoadEditPixelUI(RenderPalClass.SelectedPixelId);
                return;
            }

            if (Tool != 0)
                return;

            int x1 = (Core.MousePosition.X - OfstX) / Core.Zoom;
            int y1 = (Core.MousePosition.Y - OfstY) / Core.Zoom;
            int x2 = eX / Core.Zoom;
            int y2 = eY / Core.Zoom;

            float d = Maths.Distance(x1, y1, x2, y2);
            if (d == 0F)
            {
                Pixels[x2, y2] = RenderPalClass.SelectedPixelId;
                ModifiedPixels.Add(new Point(x2, y2));
            }
            else
            {
                int x, y;
                for (float t = 0F; t < 1F; t += 1F / Maths.Distance(x1, y1, x2, y2))
                {
                    x = (int)Maths.Lerp(x1, x2, t);
                    y = (int)Maths.Lerp(y1, y2, t);
                    Pixels[x, y] = RenderPalClass.SelectedPixelId;
                    ModifiedPixels.Add(new Point(x, y));
                }
            }
            SetImage();
        }

        public static Color GetGradient(byte px)
        {
            int id = GradientId;
            int count = RenderPalClass.Pixels[px].Gradient.Count;
            while (id >= count) id -= count;
            while (id < 0) id += count;

            if (RenderPalClass.Pixels[px].IsLerp)
            {
                Color s, e, r;
                s = RenderPalClass.Pixels[px].Gradient[id];
                e = id == RenderPalClass.Pixels[px].Gradient.Count - 1 ? RenderPalClass.Pixels[px].Gradient[0] : RenderPalClass.Pixels[px].Gradient[id + 1];
                r = Color.FromArgb((byte)Maths.Lerp(s.R, e.R, t), (byte)Maths.Lerp(s.G, e.G, t), (byte)Maths.Lerp(s.B, e.B, t));
                return r;
            }
            else
            {
                return RenderPalClass.Pixels[px].Gradient[id];
            }
        }
        private static void SetImage()
        {
            Graphics g = Graphics.FromImage(Image);

            var list = new List<Point>(ModifiedPixels);
            foreach (var pt in list)
            {
                Core.g.FillRectangle(new SolidBrush(GetGradient(Pixels[pt.X, pt.Y])), OfstX + pt.X * Core.TileSzZoom, OfstY + pt.Y * Core.TileSzZoom, Core.TileSzZoom, Core.TileSzZoom);
                ModifiedPixels.Remove(pt);
            }

            g.Dispose();
        }

        public static void Update()
        {
            Increase();

            for (int x = 0; x < Core.RWT; x++)
            {
                for (int y = 0; y < Core.RHT; y++)
                {
                    RenderPalClass.Pixels[Pixels[x, y]].Update(x, y);
                    if (RenderPalClass.Pixels[Pixels[x, y]].Gradient.Count > 1)
                    {
                        ModifiedPixels.Add(new Point(x, y));
                    }
                }
            }

            if (ModifiedPixels.Count > 0)
                SetImage();
        }

        public static void Draw()
        {
            Core.ImageUI = new Bitmap(Core.ImageUI.Width, Core.ImageUI.Height);
            Core.gui.Dispose();
            Core.gui = Graphics.FromImage(Core.ImageUI);
            Core.gui.Clear(Color.Black);
            Core.gui.DrawRectangle(Pens.Gray, OfstX - 1, OfstY - 1, Core.RWxZ + 1, Core.RHxZ + 1);

            if (Core.Zoom >= 8)
            {
                Pen p = new Pen(Color.FromArgb(20, 20, 20));
                for (int x = 0; x < Core.RW; x++)
                    for (int y = 0; y < Core.RW; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        if (y > 0) Core.gui.DrawLine(p, OfstX + x * Core.Zoom, OfstY + y * Core.Zoom, OfstX + (x + 1) * Core.Zoom - 1, OfstY + y * Core.Zoom);
                        if (x > 0) Core.gui.DrawLine(p, OfstX + x * Core.Zoom, OfstY + y * Core.Zoom, OfstX + x * Core.Zoom, OfstY + (y + 1) * Core.Zoom - 1);
                    }
            }
            Core.ImageUI.MakeTransparent();
        }

        public static void KeyDown(KeyEventArgs e)
        {
            if(e.Control)
            {
                if(e.KeyCode == Keys.Z && PixelsHistory.Count > 0)
                {
                    Pixels = PixelsHistory.Last();
                    PixelsHistory.RemoveAt(PixelsHistory.Count - 1);
                    ModifiedPixels.AddRange(GetAllPixelsPoints());
                }
                return;
            }
        }

        public static void AddPixelsHistory()
        {
            PixelsHistory.Add(CopiedPixels());
            if (PixelsHistory.Count > MaxPixelsHistory)
                PixelsHistory.RemoveAt(0);
        }

        public static void Resize(int w, int h)
        {
            Core.RW = w * Core.TileSz;
            Core.RH = h * Core.TileSz;

            Core.g.Clear(GetGradient(0));
            Core.g.Clear(Color.Black);
            Pixels = new byte[Core.RW, Core.RH];
            ModifiedPixels = GetAllPixelsPoints();
        }
    }
}
