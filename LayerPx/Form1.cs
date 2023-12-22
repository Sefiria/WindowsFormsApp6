using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Tooling;
using Tooling.UI;
using static Tooling.KB;

namespace LayerPx
{
    public partial class Form1 : Form
    {
        enum Tools
        {
            PenCircle = 0,//C
            PenSquare = 1,//P
            Bucket,//B
            Eraser,//E
            EyeDrop//mouse middle (instant)
        }

        Bitmap img, Output;
        Graphics g;
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        int W, H, imgw, imgh;
        PointF Center = PointF.Empty;
        PointF Cam = PointF.Empty;
        Tools Tool = Tools.PenSquare;
        bool ShowGrid = false;
        DATA data;
        Color[] pal = new Color[16];// 0 is transparent
        byte pal_index_primary = 1;
        byte pen_size = 1;

        const float CAM_MOV_SPD = 3F;
        const float SCALE_GAP = 0.5F;

        RangeValueF scale = new RangeValueF(1F, 2F, 10F);

        PointF get_pos(Bitmap img) => Center.Minus(img.Width, img.Height).Minus(Cam.x(scale.Value));
        Point get_pos_ms()
        {
            var m = MouseStates.Position;
            var c = Center;
            var o = get_pos(Output);
            var x = new RangeValue((int)((m.X - o.X) / scale.Value), byte.MinValue, byte.MaxValue).Value;
            var y = new RangeValue((int)((m.Y - o.Y) / scale.Value), byte.MinValue, byte.MaxValue).Value;
            return new Point(x, y);
        }
        float Amplitude => IsKeyDown(Key.LeftShift) ? 5F : 1F;
        float imgw_scaled => (imgw * scale.Value);
        float imgh_scaled => (imgh * scale.Value);

        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        // INIT

        private void Initialize()
        {
            Cursor.Hide();

            imgw = 256;
            imgh = 256;

            Output = new Bitmap(imgw, imgh);
            using (Graphics _g = Graphics.FromImage(Output))
                _g.Clear(Color.FromArgb(120, 150, 200));

            W = Render.Width;
            H = Render.Height;

            Center = new PointF(W / 2F, H / 2F);

            pal[0] = Color.Black;
            pal[1] = Color.White;
            data = new DATA();

            ResetGx();
            KB.Init();

            create_ui();

            TimerUpdate.Tick += GlobalUpdate;
            TimerDraw.Tick += GlobalDraw;
            MouseWheel += Render_MouseWheel;
        }
        void create_ui()
        {
            int sz = 24;
            var bt = new UIButton[pal.Length];
            for(int i=0;i<pal.Length; i++)
            {
                bt[i] = new UIButton() {
                    Name = i.ToString(),
                    Position = (sz+(sz*(i%8)),sz+sz*(i/8)).Vf(),
                    Size = (sz, sz).Vf(),
                    OnClick = (ui) => {
                        var id = int.Parse(ui.Name);
                        if (MouseStates.ButtonDown == MouseButtons.Left)
                        {
                            (UIMgt.UI.First(_ui => _ui is UIButton && _ui.Name == pal_index_primary.ToString()) as UIButton).BoundsColor = Color.White;
                            pal_index_primary = (byte)id;
                            (UIMgt.UI.First(_ui => _ui is UIButton && _ui.Name == pal_index_primary.ToString()) as UIButton).BoundsColor = Color.Cyan;
                        }
                        else if (MouseStates.ButtonDown == MouseButtons.Right)
                        {
                            if (def_pal_col(id))
                                (ui as UIButton).Tex = Tooling.Tools.CreateRectBitmap(sz, pal[id]);
                        }
                    },
                    Tex = Tooling.Tools.CreateRectBitmap(sz, pal[i]),
                    SleepTransparent = true,
                };
            }
            bt[pal_index_primary].BoundsColor = Color.Lime;
            UIMgt.UI.AddRange(bt);
        }


        // UTILS

        private void ResetGx()
        {
            img = new Bitmap(W, H);
            g = Graphics.FromImage(img);
            g.Clear(Color.FromArgb(12, 12, 16));
        }
        private bool def_pal_col(int pal_index)
        {
            Cursor.Show();
            ColorDialog dial = new ColorDialog() { Color = pal[pal_index] };
            var success = dial.ShowDialog() == DialogResult.OK;
            if (success)
                pal[pal_index] = dial.Color;
            Cursor.Hide();
            return success;
        }


        // UPDATE

        private void GlobalUpdate(object sender, EventArgs e)
        {
            MouseStates.OldPosition = MouseStates.Position;
            MouseStates.Position = Render.PointToClient(MousePosition);

            if (IsKeyDown(Key.Z)) Cam = Cam.Minus(0F, CAM_MOV_SPD * Amplitude);
            if (IsKeyDown(Key.Q)) Cam = Cam.Minus(CAM_MOV_SPD * Amplitude, 0F);
            if (IsKeyDown(Key.S)) Cam = Cam.PlusF(0F, CAM_MOV_SPD * Amplitude);
            if (IsKeyDown(Key.D)) Cam = Cam.PlusF(CAM_MOV_SPD * Amplitude, 0F);

            if (IsKeyPressed(Key.G)) ShowGrid = !ShowGrid;

            if (IsKeyPressed(Key.C)) Tool = Tools.PenCircle;
            if (IsKeyPressed(Key.P)) Tool = Tools.PenSquare;
            if (IsKeyPressed(Key.B)) Tool = Tools.Bucket;
            if (IsKeyPressed(Key.E)) Tool = Tools.Eraser;
            if (MouseStates.ButtonDown == MouseButtons.Middle)
            {
                byte index = data.Pointedindex(get_pos_ms());
                if (index > 0)
                {
                    (UIMgt.UI.First(_ui => _ui is UIButton && _ui.Name == pal_index_primary.ToString()) as UIButton).BoundsColor = Color.White;
                    pal_index_primary = index;
                    (UIMgt.UI.First(_ui => _ui is UIButton && _ui.Name == pal_index_primary.ToString()) as UIButton).BoundsColor = Color.Cyan;
                }
            }

            if (MouseStates.Delta != 0)
            {
                if (IsKeyDown(Key.LeftAlt))
                {
                    scale.Value += SCALE_GAP * (MouseStates.Delta < 0 ? -1F : 1F) * Amplitude;
                }
                else
                {
                    pen_size += (byte)((MouseStates.Delta < 0 ? -1 : 1) * Amplitude);
                }
            }

            if (MouseStates.IsDown && MouseStates.ButtonDown != MouseButtons.Middle)
            {
                var m = MouseStates.Position;
                var o = get_pos(Output);
                byte v = MouseStates.ButtonDown == MouseButtons.Left ? pal_index_primary : (byte)0;
                RangeValue x, y;
                if (MouseStates.OldPosition != Point.Empty && MouseStates.PositionChanged)
                {
                    var old = MouseStates.OldPosition;
                    for (float t = 0F; t <= 1F; t+=1F/MouseStates.LenghtDiff)
                    {
                        x = new RangeValue((int)Maths.Lerp((old.X - o.X) / scale.Value, (m.X - o.X) / scale.Value, t), byte.MinValue, byte.MaxValue);
                        y = new RangeValue((int)Maths.Lerp((old.Y - o.Y) / scale.Value, (m.Y - o.Y) / scale.Value, t), byte.MinValue, byte.MaxValue);
                        UseTool((byte)x.Value, (byte)y.Value, v);
                    }
                }
                else
                {
                    var pos = get_pos_ms();
                    UseTool((byte)pos.X, (byte)pos.Y, v);
                }
            }

            KB.Update();
            MouseStates.Update();
            UIMgt.Update();
        }
        void UseTool(byte x, byte y, byte v)
        {
            int direction = IsKeyDown(Key.LeftShift) ? 1 : -1;// !! has to be integer-typed !!
            switch (Tool)
            {
                default: break;
                case Tools.PenCircle: data.SetCircle(direction, x, y, v, pen_size); break;
                case Tools.PenSquare: data.SetSquare(direction, x, y, v, pen_size); break;
            }
        }


        // DRAW

        private void GlobalDraw(object sender, EventArgs e)
        {
            ResetGx();
            Draw();
            DrawUI();
            Render.Image = img;
        }
        void Draw()
        {
            //g.DrawImage(Output.Resize((int)imgw_scaled, (int)imgh_scaled), get_pos(Output));
            for (int y = 0; y < byte.MaxValue+1; y++)
            {
                for (int x = 0; x < byte.MaxValue+1; x++)
                {
                    if (data[128, (byte)x, (byte)y] == 0)
                        continue;
                    var pos = get_pos(Output);
                    var dt = data.PointedData(x, y);
                    var sat = Maths.Diff(128, dt.layer) / 128F;
                    g.FillRectangle(new SolidBrush(pal[dt.index].ChangeSaturation(sat)), pos.X + x * scale.Value, pos.Y + y * scale.Value, scale.Value, scale.Value);
                }
            }
        }
        void DrawUI()
        {
            draw_grid_and_img();
            draw_cursor();
            UIMgt.Draw(g);
        }
        void draw_grid_and_img()
        {
            var pos = get_pos(Output);
            if (ShowGrid)
            {
                int img_sz = 8;
                int sz = img_sz * (int)scale.Value;
                for (int y = 0; y < imgw_scaled; y += sz)
                    g.DrawLine(Pens.DimGray, pos.X, pos.Y + y, pos.X + imgw_scaled, pos.Y + y);
                for (int x = 0; x < imgh_scaled; x += sz)
                    g.DrawLine(Pens.DimGray, pos.X + x, pos.Y, pos.X + x, pos.Y + imgh_scaled);
            }
            g.DrawRectangle(Pens.White, new Rectangle((int)pos.X, (int)pos.Y, (int)imgw_scaled, (int)imgh_scaled));
        }
        void draw_cursor()
        {
            var ms = MouseStates.Position;
            int lgh = 16;
            int hlgh = lgh / 2;
            g.DrawLine(Pens.White, ms.X, ms.Y, ms.X + lgh, ms.Y + lgh);
            g.DrawLine(Pens.White, ms.X, ms.Y, ms.X + lgh - 1, ms.Y + lgh);
            g.DrawLine(Pens.Black, ms.X + 1, ms.Y, ms.X + lgh + 1, ms.Y + lgh);
            g.DrawLine(Pens.Black, ms.X + 1, ms.Y, ms.X + lgh + 2, ms.Y + lgh);
            g.FillEllipse(new SolidBrush(pal[pal_index_primary]), ms.X + lgh, ms.Y + lgh, hlgh, hlgh);
            g.DrawEllipse(Pens.White, ms.X + lgh, ms.Y + lgh, hlgh, hlgh);
            g.DrawEllipse(Pens.Black, ms.X + lgh + 1, ms.Y + lgh, hlgh, hlgh);

            var pos = get_pos(Output);
            var pointed = data.PointedData(get_pos_ms());
            if(Tool == Tools.PenCircle)
                g.DrawEllipse(Pens.Gray, pos.X + (pointed.x - pen_size / 2) * scale.Value, pos.Y + (pointed.y - pen_size / 2) * scale.Value, pen_size * scale.Value, pen_size * scale.Value);
            else if (Tool == Tools.PenSquare)
                g.DrawRectangle(Pens.Gray, pos.X + (pointed.x - pen_size / 2) * scale.Value, pos.Y + (pointed.y - pen_size / 2) * scale.Value, pen_size * scale.Value, pen_size * scale.Value);
        }


        // FORM EVENTS

        private void Render_MouseWheel(object sender, MouseEventArgs e)
        {
            MouseStates.Delta = e.Delta;
        }
        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonDown = e.Button;
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonDown = MouseButtons.None;
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            MouseStates.ButtonDown = MouseButtons.None;
        }
    }
}

// TODO : UP/DOWN TOO FAST ON HOLDING CLICK
//        HAS TO MANAGE RELEASED CLICK FOR IT