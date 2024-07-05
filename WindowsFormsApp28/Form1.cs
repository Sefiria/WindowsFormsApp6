using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp28
{
    public partial class Form1 : Form
    {
        List<Color> Palette = new List<Color>{
                Color.FromArgb(219, 229, 219),
                Color.FromArgb(175, 207, 151),
                Color.FromArgb(88, 139, 112),
                Color.FromArgb(35, 48, 64) };
        List<SolidBrush> PaletteBrushes;

        const int w = 256;
        const int h = 256;
        float z = 10F;
        float x=w / 2, y= h/ 2;
        float mvspd = 0.5F;
        byte[] Pixels = new byte[w * h];
        Fluid[] Fluids = new Fluid[w * h];
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image;
        byte px_pal_sel = 0;
        Font font;
        Size CharSize;
        double time = 0;
        bool busy = false;

        public Form1()
        {
            InitializeComponent();

            font = new Font("Courrier New", 12F, FontStyle.Regular);
            CharSize = TextRenderer.MeasureText("A", font);

            PaletteBrushes = new List<SolidBrush>();
            foreach (Color c in Palette)
                PaletteBrushes.Add(new SolidBrush(c));

            for(int x=0;x<w; x++)
                for(int y=0;y<h;y++)
                    Fluids[y*w+x] = new Fluid();

            KB.Init();
            MouseStates.Initialize(Render);

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            if (busy || !Focused) return;
            busy = true;
            var (z, q, s, d) = KB.ZQSD();
            var kc = KB.IsKeyPressed(KB.Key.C);
            var kx = KB.IsKeyPressed(KB.Key.X);
            var left_pressed = MouseStates.IsButtonPressed(MouseButtons.Left);
            var right_pressed = MouseStates.IsButtonPressed(MouseButtons.Right);
            bool pressed = left_pressed || right_pressed;
            var left_down = MouseStates.IsButtonDown(MouseButtons.Left);
            var right_down = MouseStates.IsButtonDown(MouseButtons.Right);
            var down = left_down || right_down;
            float adjustedSpeed = mvspd / this.z * 20F;

            if (z) y -= adjustedSpeed;
            if (q) x -= adjustedSpeed;
            if (s) y += adjustedSpeed;
            if (d) x += adjustedSpeed;

            if (kx) ResetPixels();
            if (kc) ResetFluids();

            var ms = MouseStates.Position.ToPoint();
            var oldMs = MouseStates.OldPosition.ToPoint();

            if (pressed)
            {
                int worldX = (int)((ms.X - Width / 2) / this.z + x);
                int worldY = (int)((ms.Y - Height / 2) / this.z + y);
                if (worldX >= 0 && worldX < w && worldY >= 0 && worldY < h)
                {
                    int index = worldY * w + worldX;
                    px_pal_sel = Pixels[index] == 0 ? (byte)(left_pressed?3:1) : (byte)0;
                }
            }
            else if (down)
            {
                for (float t = 0; t <= 1; t += 0.01f)
                {
                    PointF interpolatedPoint = Maths.Lerp(oldMs, ms, t);
                    int worldX = (int)((interpolatedPoint.X - Width / 2) / this.z + x);
                    int worldY = (int)((interpolatedPoint.Y - Height / 2) / this.z + y);
                    if (worldX >= 0 && worldX < w && worldY >= 0 && worldY < h)
                    {
                        int index = worldY * w + worldX;
                        if (right_down) Fluids[index].Q = Math.Min(10F, Fluids[index].Q+0.01F);
                        else Pixels[index] = px_pal_sel;
                    }
                }
            }

            if (MouseStates.Delta != 0)
                this.z *= 1 + MouseStates.Delta / 1000F;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ManageFluids();
            time = stopwatch.Elapsed.TotalSeconds;

            KB.Update();
            MouseStates.Update();
            busy = false;
        }

        int tick = 0;
        void ManageFluids()
        {
            //if (tick++ < 10) return;
            //tick = 0;

            Fluid[] new_fluids = Fluids.Select(fluid => new Fluid(fluid)).ToArray();

            bool px(int x, int y) => Pixels[y * w + x] != 3 && get(x,y) < 0.1F;
            float get(int x, int y) => new_fluids[y * w + x].Q;
            float fluiddiff(int x, int y, int ofst_x, int ofst_y)
            {
                float diff = get(x, y) - get(x + ofst_x, y + ofst_y);
                //float diff = get(x, y);
                return diff;// > 0.01F ? diff : 0F;
            }

            int startX = Math.Max(0, (int)(x - Width / (2 * z)));
            int startY = Math.Max(0, (int)(y - Height / (2 * z)));
            int endX = Math.Min(w-1, (int)(x + Width / z));
            int endY = Math.Min(h-1, (int)(y + Height / z));

            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    if (get(x, y) == 0F)
                        continue;

                    bool bottom = y < h - 1 && px(x, y + 1);
                    bool left = x > 0 && px(x - 1, y);
                    bool right = x < w - 1 && px(x + 1, y);
                    bool top = y > 0 && !left && !right && Pixels[y * w + x] != 3 && get(x, y) > 1F;
                    float d, d2, q, spd = 0.7F;

                    void move(int ofst_x, int ofst_y, float _d)
                    {
                        q = Math.Min(Fluids[y * w + x].Q, _d);
                        Fluids[y * w + x].Q -= q;
                        if (Fluids[y * w + x].Q < 0F) Fluids[y * w + x].Q = 0F;
                        Fluids[(y + ofst_y) * w + x + ofst_x].Q += q;
                    }
                    void job(int ofst_x, int ofst_y, bool isdiff = true)
                    {
                        move(ofst_x, ofst_y, get(x, y) * spd);
                    }

                    if (bottom) job(0, 1, false);
                    else if (left && right)
                    {
                        d = fluiddiff(x, y, -1, 0) * spd;
                        d2 = fluiddiff(x, y, 1, 0) * spd;
                        if (d == d2)
                        {
                            move(-1, 0, d / 2F);
                            move(1, 0, d2 / 2F);
                        }
                        else
                        {
                            if (d < d2)
                                move(-1, 0, d);
                            else
                                move(1, 0, d2);
                        }
                    }
                    else if (left || right)
                    {
                        if (right) job(1, 0);
                        if (left) job(-1, 0);
                    }
                    else if (top) job(0, -1);

                    if (Fluids[y * w + x].Q < 0.01F) Fluids[y * w + x].Q = 0F;
                }
            }
        }
        void ResetFluids()
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Fluids[y * w + x].Q = 0F;
                }
            }
        }
        void ResetPixels()
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Pixels[y * w + x] = 0;
                }
            }
        }

        private void Draw(object sender, EventArgs e)
        {
            Image = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(Image))
            {
                int startX = Math.Max(0, (int)(x - Width / (2 * z)));
                int startY = Math.Max(0, (int)(y - Height / (2 * z)));
                int endX = Math.Min(w-1, (int)(x + Width / z));
                int endY = Math.Min(h-1, (int)(y + Height / z));

                for (int y = startY; y < endY; y++)
                    for (int x = startX; x < endX; x++)
                    {
                        int index = y * w + x;
                        
                        if (Pixels[index] != 3)
                        {
                            //    if(Fluids[index].Q == 0F)
                            //        g.FillRectangle(Brushes.White, (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                            //    else
                            //    g.FillRectangle(new SolidBrush(Color.FromArgb((byte)(100 - Fluids[index].Q * 100), (byte)(200 - Fluids[index].Q * 200), (byte)(255 - Fluids[index].Q * 255))), (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                            if (Fluids[index].Q > 0F)
                                g.FillRectangle(PaletteBrushes[2], (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                            else if (Fluids[index].LF > 0F)
                            {
                                g.FillRectangle(PaletteBrushes[1], (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                                Fluids[index].LF -= Fluid.GraphicalDeathSpeed;
                            }
                    }
                        else
                            g.FillRectangle(PaletteBrushes[Pixels[index]], (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                    }

                var ms = MouseStates.Position.ToPoint();
                int worldX = (int)((ms.X - Width / 2) / z + x);
                int worldY = (int)((ms.Y - Height / 2) / z + y);
                if (worldX >= 0 && worldX < w && worldY >= 0 && worldY < h)
                {
                    int index = worldY * w + worldX;
                    var tx = $"{{{worldX},{worldY}}} {Maths.Round(Fluids[index].Q, 2)}";
                    var txw = g.MeasureString(tx, font).Width;
                    g.DrawString(tx, font, Brushes.Black, ms.X - txw / 2, ms.Y - CharSize.Height * 1.5F);
                }

                g.DrawString(time.ToString(), font, Brushes.Black, 10, 10);
            }
            Render.Image = Image;
        }

    }
}
