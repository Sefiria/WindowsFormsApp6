using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
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

        const int w = byte.MaxValue;
        const int h = byte.MaxValue;
        float z = 10F;
        float x=w / 2, y= h/ 2;
        float mvspd = 0.5F;
        byte[] Pixels = new byte[w * h];
        float[] Fluids = new float[w * h];
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image;
        Graphics g;
        byte px_pal_sel = 0;
        Font font;
        Size CharSize;

        public Form1()
        {
            InitializeComponent();

            font = new Font("Courrier New", 12F, FontStyle.Regular);
            CharSize = TextRenderer.MeasureText("A", font);

            PaletteBrushes = new List<SolidBrush>();
            foreach (Color c in Palette)
                PaletteBrushes.Add(new SolidBrush(c));

            KB.Init();
            MouseStates.Initialize(Render);

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            var (z, q, s, d) = KB.ZQSD();
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
                        if (right_down) Fluids[index] = 1F;
                        else Pixels[index] = px_pal_sel;
                    }
                }
            }

            if (MouseStates.Delta != 0)
                this.z *= 1 + MouseStates.Delta / 1000F;

            ManageFluids();

            KB.Update();
            MouseStates.Update();
        }

        void ManageFluids()
        {
            bool px(int x, int y) => Pixels[y * w + x] != 3;
            float get(int x, int y) => Fluids[y * w + x];
            float fluiddiff(int x, int y, int ofst_x, int ofst_y)
            {
                float diff = get(x,y) - get(x + ofst_x, y + ofst_y);
                return diff > 0.01F ? diff : 0F;
            }

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (Fluids[y * w + x] == 0F)
                        continue;

                    bool bottom = y < h - 1 && px(x, y + 1) && get(x, y + 1) < 1F;
                    bool left = x > 0 && px(x - 1, y) && get(x - 1, y) < 1F;
                    bool right = x < w - 1 && px(x + 1, y) && get(x + 1, y) < 1F;
                    bool top = y > 0 && px(x, y - 1) && get(x, y - 1) < 1F;
                    float d, d2, q, spd = 0.2F;
                    
                    void move(int ofst_x, int ofst_y, float _d)
                    {
                        if (_d <= 0F) return;
                        q = Math.Max(0.02F, _d);
                        Fluids[y*w+x] -= q;
                        if (get(x, y) < 0F) Fluids[y * w + x] = 0F;
                        Fluids[(y + ofst_y) * w + x + ofst_x] += q;
                    }
                    void job(int ofst_x, int ofst_y, bool isdiff = true)
                    {
                        move(ofst_x, ofst_y, isdiff ? fluiddiff(x, y, ofst_x, ofst_y) * spd : get(x, y) * 0.25F);
                    }

                    if (bottom) job(0, 1, false);
                    else if (left && right) { d = fluiddiff(x, y, -1, 0) * spd; d2 = fluiddiff(x, y, 1, 0) * spd; move(-1, 0, d); move(1, 0, d2); }
                    else if (left) job(-1, 0);
                    else if (right) job(1, 0);
                    else if (top) job(0, -1);

                    if (get(x, y) > 1F) Fluids[y * w + x] = 1F;
                    if (get(x, y) < 0.01F) Fluids[y * w + x] = 0F;
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
                int endX = Math.Min(w, (int)(x + Width / z));
                int endY = Math.Min(h, (int)(y + Height / z));

                for (int y = startY; y < endY; y++)
                    for (int x = startX; x < endX; x++)
                    {
                        int index = y * w + x;
                        if(Pixels[index] == 0 && Fluids[index] > 0F)
                            g.FillRectangle(PaletteBrushes[Fluids[index] >= 0.2F ? 2 : 1], (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                        else
                            g.FillRectangle(PaletteBrushes[Pixels[index]], (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                    }

                var ms = MouseStates.Position.ToPoint();
                int worldX = (int)((ms.X - Width / 2) / z + x);
                int worldY = (int)((ms.Y - Height / 2) / z + y);
                if (worldX >= 0 && worldX < w && worldY >= 0 && worldY < h)
                {
                    int index = worldY * w + worldX;
                    g.DrawString(Maths.Round(Fluids[index], 2).ToString(), font, Brushes.Black, ms.X - CharSize.Width / 2, ms.Y - CharSize.Height * 1.5F);
                }
            }
            Render.Image = Image;
        }

    }
}
