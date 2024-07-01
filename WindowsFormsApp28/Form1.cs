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
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image;
        Graphics g;
        byte px_pal_sel = 0;

        public Form1()
        {
            InitializeComponent();

            PaletteBrushes = new List<SolidBrush>();
            foreach (Color c in Palette)
                PaletteBrushes.Add(new SolidBrush(c));

            KB.Init();
            MouseStates.Initialize(Render);

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, System.EventArgs e)
        {
            var (z, q, s, d) = KB.ZQSD();

            float adjustedSpeed = mvspd / this.z * 20F;

            if (z) y -= adjustedSpeed;
            if (q) x -= adjustedSpeed;
            if (s) y += adjustedSpeed;
            if (d) x += adjustedSpeed;

            var ms = MouseStates.Position.ToPoint();
            var oldMs = MouseStates.OldPosition.ToPoint();

            if (MouseStates.IsButtonPressed(MouseButtons.Left))
            {
                int worldX = (int)((ms.X - Width / 2) / this.z + x);
                int worldY = (int)((ms.Y - Height / 2) / this.z + y);
                if (worldX >= 0 && worldX < w && worldY >= 0 && worldY < h)
                {
                    int index = worldY * w + worldX;
                    px_pal_sel = Pixels[index] == 0 ? (byte)3 : (byte)0;
                }
            }
            else if (MouseStates.IsDown)
            {
                for (float t = 0; t <= 1; t += 0.01f)
                {
                    PointF interpolatedPoint = Maths.Lerp(oldMs, ms, t);
                    int worldX = (int)((interpolatedPoint.X - Width / 2) / this.z + x);
                    int worldY = (int)((interpolatedPoint.Y - Height / 2) / this.z + y);
                    if (worldX >= 0 && worldX < w && worldY >= 0 && worldY < h)
                    {
                        int index = worldY * w + worldX;
                        Pixels[index] = px_pal_sel;
                    }
                }
            }

            if (MouseStates.Delta != 0)
                this.z *= 1 + MouseStates.Delta / 1000F;

            KB.Update();
            MouseStates.Update();
        }


        private void Draw(object sender, System.EventArgs e)
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
                        g.FillRectangle(PaletteBrushes[Pixels[index]], (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                    }
            }
            Render.Image = Image;
        }

    }
}
