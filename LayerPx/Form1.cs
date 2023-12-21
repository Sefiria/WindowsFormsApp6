using LayerPx.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tooling;
using static Tooling.KB;

namespace LayerPx
{
    public partial class Form1 : Form
    {
        enum Tools
        {
            Pen = 0,
            Bucket,
            Eraser,
            EyeDrop
        }

        Bitmap img, Output;
        Graphics g;
         Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        int W, H, imgw, imgh;
        PointF Center = PointF.Empty;
        PointF Cam = PointF.Empty;
        Tools Tool = Tools.Pen;
        bool ShowGrid = true;

        const float CAM_MOV_SPD = 3F;
        const float SCALE_GAP = 0.5F;

        RangeValueF Scale = new RangeValueF(1F, 2F, 10F);

        PointF get_pos(Bitmap img) => Center.Minus(img.Width / 2F, img.Height / 2F).Minus(Cam);
        float Amplitude => IsKeyDown(Key.LeftShift) ? 5F : 1F;
        float imgw_scaled => (imgw * Scale.Value);
        float imgh_scaled => (imgh * Scale.Value);

        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            imgw = 256;
            imgh = 256;

            Output = new Bitmap(imgw, imgh);
            using (Graphics _g = Graphics.FromImage(Output))
                _g.Clear(Color.FromArgb(120, 150, 200));

            W = Render.Width;
            H = Render.Height;

            Center = new PointF(W / 2F, H / 2F);

            ResetGx();
            KB.Init();

            TimerUpdate.Tick += GlobalUpdate;
            TimerDraw.Tick += GlobalDraw;
            MouseWheel += Render_MouseWheel;
        }

        private void ResetGx()
        {
            img = new Bitmap(W, H);
            g = Graphics.FromImage(img);
            g.Clear(Color.FromArgb(12, 12, 16));
        }

        private void GlobalUpdate(object sender, EventArgs e)
        {
            if (IsKeyDown(Key.Z)) Cam = Cam.Minus(0F, CAM_MOV_SPD * Amplitude);
            if (IsKeyDown(Key.Q)) Cam = Cam.Minus(CAM_MOV_SPD * Amplitude, 0F);
            if (IsKeyDown(Key.S)) Cam = Cam.PlusF(0F, CAM_MOV_SPD * Amplitude);
            if (IsKeyDown(Key.D)) Cam = Cam.PlusF(CAM_MOV_SPD * Amplitude, 0F);

            if (IsKeyPressed(Key.G)) ShowGrid = !ShowGrid;

            if (MouseStates.Delta != 0)
            {
                Scale.Value += SCALE_GAP * (MouseStates.Delta < 0 ? -1F : 1F) * Amplitude;
            }

            KB.Update();
            MouseStates.Update();
        }

        private void GlobalDraw(object sender, EventArgs e)
        {
            ResetGx();
            Draw();
            DrawUI();
            Render.Image = img;
        }

        void Draw()
        {
            g.DrawImage(Output.Resize((int)imgw_scaled, (int)imgh_scaled), get_pos(Output));
        }

        void DrawUI()
        {
            var pos = get_pos(Output);
            if (ShowGrid)
            {
                int img_sz = 8;
                int sz = img_sz * (int)Scale.Value;
                for (int y = 0; y < imgw_scaled; y += sz)
                    g.DrawLine(Pens.DimGray, pos.X, pos.Y + y, pos.X + imgw_scaled, pos.Y + y);
                for (int x = 0; x < imgh_scaled; x += sz)
                    g.DrawLine(Pens.DimGray, pos.X + x, pos.Y, pos.X + x, pos.Y + imgh_scaled);
            }
            g.DrawRectangle(Pens.White, new Rectangle((int)pos.X, (int)pos.Y, (int)imgw_scaled, (int)imgh_scaled));
        }



        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonDown = e.Button;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            MouseStates.Position = e.Location;
        }
        private void Render_MouseWheel(object sender, MouseEventArgs e)
        {
            MouseStates.Delta = e.Delta;
        }
    }
}
