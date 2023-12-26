using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using Tooling.UI;
using static LayerPx.Form1;
using static Tooling.KB;

namespace LayerPx
{
    public partial class Form1 : Form
    {
        public static Form1 Instance = null;
        public Queue<Point> draw_refresh_queue = new Queue<Point>();
        public int layer_at_first_press = DATA_LAYERS / 2;
        public ToolModes Mode = ToolModes.Normal;

        enum Tools
        {
            PenCircle = 0,
            PenSquare = 1,
        }
        public enum ToolModes
        {
            Normal = 0,
            Force,
            Auto,
            Up,
            Down,
        }
        public enum MirrorMode
        {
            None = 0,
            X,
            Y,
            XY,
        }

        Bitmap img, Output;
        Graphics g_render, g;
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        int imgw, imgh;
        PointF Center = PointF.Empty;
        PointF Cam = PointF.Empty;
        PointF sun;
        PointF mirror_src;
        Tools Tool = Tools.PenSquare;
        MirrorMode mirror_mode = MirrorMode.None;
        bool ShowGrid = false, ShowSunAndMirror = false, ShowShadows = true;
        bool refresh_shadows = false;
        DATA data;
        Color[] pal = new Color[16];// 0 is transparent
        int pal_index_primary = 1;
        int pen_size = 1;
        int holding_layer_target = DATA_LAYERS / 2;
        bool holding_layer_released = true;
        int fixed_layer = DATA_LAYERS / 2;
        int layer_gap = 1;
        bool mouseleft_released = true;

        const int DATA_LAYERS = 8, DATA_WIDTH = 127, DATA_HEIGHT = 127;
        const float CAM_MOV_SPD = 3F, SUN_MOV_SPD = 2F;
        const float SCALE_GAP = 0.5F;

        RangeValueF scale = new RangeValueF(1F, 2F, 10F);
        PointF get_world_sun => get_pos(sun); 
        PointF get_world_mirror => get_pos(mirror_src); 

        PointF get_pos(PointF pt) => Center.PlusF(pt.Minus(Cam).x(scale.Value));
        PointF get_pos(Bitmap _img) => Center.Minus(imgw_scaled / 2, imgh_scaled / 2).Minus(Cam.x(scale.Value));
        float get_pos_x(Bitmap _img) => Center.X - imgw_scaled / 2F - Cam.X * scale.Value;
        float get_pos_y(Bitmap _img) => Center.Y - imgh_scaled / 2F - Cam.Y * scale.Value;
        Point get_pos_ms(PointF pos)
        {
            var o = get_pos(Output);
            var x = new RangeValue((int)((pos.X - o.X) / scale.Value), 0, DATA_WIDTH).Value;
            var y = new RangeValue((int)((pos.Y - o.Y) / scale.Value), 0, DATA_HEIGHT).Value;
            return new Point(x, y);
        }
        Point get_pos_ms() => get_pos_ms(MouseStates.Position);
        Point get_pos_oldms() => get_pos_ms(MouseStates.OldPosition);
        float Amplitude => IsKeyDown(Key.LeftShift) ? 5F : 1F;
        float imgw_scaled => (imgw * scale.Value);
        float imgh_scaled => (imgh * scale.Value);
        int W => Render.Width;
        int H => Render.Height;
        int get_mirror_x(int x) => (int)(mirror_src.X + imgw / 2F + (mirror_src.X + imgw / 2F - x));
        int get_mirror_y(int y) => (int)(mirror_src.Y + imgh / 2F + (mirror_src.Y + imgh / 2F - y));
        Point get_mirror_pt(int x, int y) => new Point(get_mirror_x(x), get_mirror_y(y));

        public Form1()
        {
            InitializeComponent();
            Instance = this;
            Initialize();
        }

        // INIT

        private void Initialize()
        {
            Cursor.Hide();

            imgw = DATA_WIDTH + 1;
            imgh = DATA_HEIGHT + 1;

            Center = new PointF(W / 2F, H / 2F);

            pal[0] = Color.Black;
            pal[1] = Color.White;
            data = new DATA(DATA_LAYERS, DATA_WIDTH, DATA_HEIGHT);

            sun = (0F, 0F).P();

            ResetGx();
            ResetGxRender();
            KB.Init();

            create_ui();

            TimerUpdate.Tick += GlobalUpdate;
            TimerDraw.Tick += GlobalDraw;
            MouseWheel += Render_MouseWheel;
            Resize += (s, e) => { ResetGx(); ResetGxRender(); };
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
                            pal_index_primary = id;
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
            Output = new Bitmap((int)imgw_scaled, (int)imgh_scaled);
            g = Graphics.FromImage(Output);
            g.Clear(Color.Black);
            ResetDraw();
        }
        private void ResetGxRender()
        {
            img = new Bitmap(W, H);
            g_render = Graphics.FromImage(img);
            g_render.Clear(Color.FromArgb(12, 12, 16));
        }
        private bool def_pal_col(int pal_index)
        {
            Cursor.Show();
            ColorDialog dial = new ColorDialog() { Color = pal[pal_index] };
            var success = dial.ShowDialog() == DialogResult.OK;
            if (success)
            {
                pal[pal_index] = dial.Color;
                ResetDraw();
            }
            Cursor.Hide();
            return success;
        }


        // UPDATE

        private void GlobalUpdate(object sender, EventArgs e)
        {
            MouseStates.OldPosition = MouseStates.Position;
            MouseStates.Position = Render.PointToClient(MousePosition);

            if (IsKeyDown(Key.LeftCtrl) && IsKeyDown(Key.LeftAlt) && IsKeyDown(Key.LeftShift) && IsKeyPressed(Key.C))
            {
                data.Clear();
                ResetGx();
            }

            if (IsKeyDown(Key.Z)) { Cam = Cam.Minus(0F, CAM_MOV_SPD * Amplitude); refresh_shadows = true; }
            if (IsKeyDown(Key.Q)) { Cam = Cam.Minus(CAM_MOV_SPD * Amplitude, 0F); refresh_shadows = true; }
            if (IsKeyDown(Key.S)) { Cam = Cam.PlusF(0F, CAM_MOV_SPD * Amplitude); refresh_shadows = true; }
            if (IsKeyDown(Key.D)) { Cam = Cam.PlusF(CAM_MOV_SPD * Amplitude, 0F); refresh_shadows = true; }

            if(ShowSunAndMirror)
            {
                if (IsKeyDown(Key.LeftCtrl))
                {
                    if (IsKeyDown(Key.Numpad8)) { sun = sun.Minus(0F, SUN_MOV_SPD * Amplitude); refresh_shadows = true; }
                    if (IsKeyDown(Key.Numpad4)) { sun = sun.Minus(SUN_MOV_SPD * Amplitude, 0F); refresh_shadows = true; }
                    if (IsKeyDown(Key.Numpad2)) { sun = sun.PlusF(0F, SUN_MOV_SPD * Amplitude); refresh_shadows = true; }
                    if (IsKeyDown(Key.Numpad6)) { sun = sun.PlusF(SUN_MOV_SPD * Amplitude, 0F); refresh_shadows = true; }
                }
                else
                {
                    if (IsKeyDown(Key.Numpad8)) mirror_src = mirror_src.Minus(0F, SUN_MOV_SPD * Amplitude);
                    if (IsKeyDown(Key.Numpad4)) mirror_src = mirror_src.Minus(SUN_MOV_SPD * Amplitude, 0F);
                    if (IsKeyDown(Key.Numpad2)) mirror_src = mirror_src.PlusF(0F, SUN_MOV_SPD * Amplitude);
                    if (IsKeyDown(Key.Numpad6)) mirror_src = mirror_src.PlusF(SUN_MOV_SPD * Amplitude, 0F);
                }
            }

            if (IsKeyPressed(Key.G)) ShowGrid = !ShowGrid;
            if (IsKeyPressed(Key.H)) ShowSunAndMirror = !ShowSunAndMirror;
            if (IsKeyPressed(Key.J)) { ShowShadows = !ShowShadows; if (ShowShadows) refresh_shadows = true; }

            if (IsKeyPressed(Key.P)) Tool = Tool == Tools.PenCircle ? Tools.PenSquare : Tools.PenCircle;

            if(IsKeyDown(Key.LeftCtrl))
            {
                if (IsKeyPressed(Key.Left)) mirror_mode = (MirrorMode)Maths.Range(0, Enum.GetNames(typeof(MirrorMode)).Count() - 1, (int)mirror_mode - 1);
                if (IsKeyPressed(Key.Right)) mirror_mode = (MirrorMode)Maths.Range(0, Enum.GetNames(typeof(MirrorMode)).Count() - 1, (int)mirror_mode + 1);
            }
            else
            {
                if (IsKeyPressed(Key.Left)) Mode = (ToolModes)Maths.Range(0, Enum.GetNames(typeof(ToolModes)).Count() - 1, (int)Mode - 1);
                if (IsKeyPressed(Key.Right)) Mode = (ToolModes)Maths.Range(0, Enum.GetNames(typeof(ToolModes)).Count() - 1, (int)Mode + 1);
            }

            if (IsKeyDown(Key.LeftCtrl))
            {
                if (IsKeyPressed(Key.Up)) layer_gap = (int)Maths.Range(0, DATA_LAYERS, layer_gap + (IsKeyDown(Key.LeftShift) ? 10 : 1));
                if (IsKeyPressed(Key.Down)) layer_gap = (int)Maths.Range(0, DATA_LAYERS, layer_gap - (IsKeyDown(Key.LeftShift) ? 10 : 1));
            }
            else
            {
                if (IsKeyPressed(Key.Up)) fixed_layer = (int)Maths.Range(0, DATA_LAYERS, fixed_layer + layer_gap);
                if (IsKeyPressed(Key.Down)) fixed_layer = (int)Maths.Range(0, DATA_LAYERS, fixed_layer - layer_gap);
            }

            if (MouseStates.ButtonDown == MouseButtons.Middle)
            {
                int index = data.Pointedindex(get_pos_ms());
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
                    ResetGx();
                    refresh_shadows = true;
                }
                else
                {
                    pen_size += (int)((MouseStates.Delta < 0 ? -1 : 1) * Amplitude);
                    if (pen_size < 1) pen_size = 1;
                    if (pen_size > Math.Min(DATA_WIDTH, DATA_HEIGHT) / 2) pen_size = Math.Min(DATA_WIDTH, DATA_HEIGHT) / 2;
                }
            }

            if (MouseStates.IsDown == false)
            {
                mouseleft_released = true;
                holding_layer_released = true;
            }
            else if (MouseStates.ButtonDown != MouseButtons.Middle)
            {
                if(mouseleft_released)
                    layer_at_first_press = data.PointedLayer(get_pos_ms());
                mouseleft_released = false;
                int v = MouseStates.ButtonDown == MouseButtons.Left ? pal_index_primary : 0;
                if (MouseStates.OldPosition != Point.Empty && MouseStates.PositionChanged)
                {
                    var old = get_pos_oldms();
                    var m = get_pos_ms();
                    RangeValue x, y;
                    for (float t = 0F; t <= 1F; t += 1F / MouseStates.LenghtDiff)
                    {
                        x = new RangeValue((int)Maths.Lerp(old.X, m.X, t), 0, DATA_WIDTH);
                        y = new RangeValue((int)Maths.Lerp(old.Y, m.Y, t), 0, DATA_HEIGHT);
                        UseTool(x.Value, y.Value, v);
                    }
                }
                else
                {
                    var pos = get_pos_ms();
                    UseTool(pos.X, pos.Y, v, IsKeyDown(Key.LeftShift));
                }
            }
            KB.Update();
            MouseStates.Update();
            UIMgt.Update();
        }
        void UseTool(int _x, int _y, int v, bool isBucket = false)
        {
            void use(int x, int y)
            {
                if (!new List<ToolModes>() { ToolModes.Normal, ToolModes.Force }.Contains(Mode))
                {
                    if (holding_layer_released)
                    {
                        holding_layer_released = false;
                        int direction = (Mode == ToolModes.Auto ? (IsKeyDown(Key.LeftCtrl) ? -1 : 1) : (Mode == ToolModes.Up ? 1 : -1)) * layer_gap;
                        holding_layer_target = data.calc_layer(direction, x, y).@new;
                    }
                    if (isBucket)
                    {
                        data.FillWithLayer(holding_layer_target, x, y, v);
                    }
                    else
                    {
                        switch (Tool)
                        {
                            default: break;
                            case Tools.PenCircle: data.SetCircleWithLayer(holding_layer_target, x, y, v, pen_size); break;
                            case Tools.PenSquare: data.SetSquareWithLayer(holding_layer_target, x, y, v, pen_size); break;
                        }
                    }
                }
                else
                {
                    bool f = Mode != ToolModes.Normal;

                    if (isBucket)
                    {
                        data.FillWithLayer(fixed_layer, x, y, v, force: f);
                    }
                    else
                    {
                        switch (Tool)
                        {
                            default: break;
                            case Tools.PenCircle: data.SetCircleWithLayer(fixed_layer, x, y, v, pen_size, force: f); break;
                            case Tools.PenSquare: data.SetSquareWithLayer(fixed_layer, x, y, v, pen_size, force: f); break;
                        }
                    }
                }
            }

            switch (mirror_mode)
            {
                case MirrorMode.None: use(_x, _y); break;
                case MirrorMode.X:
                    use(_x, _y);
                    use(get_mirror_x(_x), _y);
                    break;
                case MirrorMode.Y:
                    use(_x, _y);
                    use(_x, get_mirror_y(_y));
                    break;
                case MirrorMode.XY:
                    var mirror_pt = get_mirror_pt(_x, _y);
                    use(_x, _y);
                    use(mirror_pt.X, _y);
                    use(_x, mirror_pt.Y);
                    use(mirror_pt.X, mirror_pt.Y);
                    break;
            }

            refresh_shadows = true;
        }


        // DRAW

        private void GlobalDraw(object sender, EventArgs e)
        {
            ResetGxRender();
            Draw();
            draw_shadows();
            g_render.DrawImage(Output, get_pos(Output));
            draw_sun_and_mirror();
            DrawUI();
            Render.Image = img;
        }
        void ResetDraw(string sender = "")
        {
            for (int y = 0; y < DATA_HEIGHT; y++)
            {
                for (int x = 0; x < DATA_WIDTH; x++)
                {
                    int l = data.PointedLayer(x, y);
                    if (data[l, x, y] == 0)
                        continue;
                    var dt = data.PointedData(x, y);
                    var brightness = dt.layer / (float)DATA_LAYERS;
                    g.FillRectangle(new SolidBrush(pal[dt.index].WithBrightness(brightness)), x * scale.Value, y * scale.Value, scale.Value, scale.Value);
                }
            }
            if(sender != nameof(draw_shadow))
                refresh_shadows = true;
        }
        void Draw()
        {
            //Console.WriteLine($"{MouseStates.Position} - {pos}");
            while(draw_refresh_queue.Count > 0)
            {
                Point pt = draw_refresh_queue.Dequeue();
                int x = pt.X, y = pt.Y;
                var dt = data.PointedData(x, y);
                int l = dt.layer;
                var brightness = dt.layer / (float)DATA_LAYERS;
                g.FillRectangle(new SolidBrush(pal[dt.index].WithBrightness(brightness)), x * scale.Value, y * scale.Value, scale.Value, scale.Value);
            }
        }
        void DrawUI()
        {
            draw_grid_and_img();
            draw_modes();
            draw_cursor();
            UIMgt.Draw(g_render);
        }
        void draw_grid_and_img()
        {
            var pos = get_pos(Output);
            if (ShowGrid)
            {
                int img_sz = 8;
                int sz = img_sz * (int)scale.Value;
                for (int y = 0; y < imgw_scaled; y += sz)
                    g_render.DrawLine(Pens.DimGray, pos.X, pos.Y + y, pos.X + imgw_scaled, pos.Y + y);
                for (int x = 0; x < imgh_scaled; x += sz)
                    g_render.DrawLine(Pens.DimGray, pos.X + x, pos.Y, pos.X + x, pos.Y + imgh_scaled);
            }
            g_render.DrawRectangle(Pens.White, new Rectangle((int)pos.X, (int)pos.Y, (int)imgw_scaled, (int)imgh_scaled));
        }
        void draw_cursor()
        {
            var ms = MouseStates.Position;
            int lgh = 16;
            int hlgh = lgh / 2;
            g_render.DrawLine(Pens.White, ms.X, ms.Y, ms.X + lgh, ms.Y + lgh);
            g_render.DrawLine(Pens.White, ms.X, ms.Y, ms.X + lgh - 1, ms.Y + lgh);
            g_render.DrawLine(Pens.Black, ms.X + 1, ms.Y, ms.X + lgh + 1, ms.Y + lgh);
            g_render.DrawLine(Pens.Black, ms.X + 1, ms.Y, ms.X + lgh + 2, ms.Y + lgh);
            g_render.FillEllipse(new SolidBrush(pal[pal_index_primary]), ms.X + lgh, ms.Y + lgh, hlgh, hlgh);
            g_render.DrawEllipse(Pens.White, ms.X + lgh, ms.Y + lgh, hlgh, hlgh);
            g_render.DrawEllipse(Pens.Black, ms.X + lgh + 1, ms.Y + lgh, hlgh, hlgh);

            var pos = get_pos(Output);
            var pointed = get_pos_ms();
            int _x = pointed.X;
            int _y = pointed.Y;

            void draw_target(int x, int y)
            {
                if (Tool == Tools.PenCircle)
                    g_render.DrawEllipse(Pens.Gray, pos.X + (x - pen_size / 2) * scale.Value, pos.Y + (y - pen_size / 2) * scale.Value, pen_size * scale.Value, pen_size * scale.Value);
                else if (Tool == Tools.PenSquare)
                    g_render.DrawRectangle(Pens.Gray, pos.X + (x - pen_size / 2) * scale.Value, pos.Y + (y - pen_size / 2) * scale.Value, pen_size * scale.Value, pen_size * scale.Value);
            }

            switch (mirror_mode)
            {
                case MirrorMode.None: draw_target(_x, _y); break;
                case MirrorMode.X:
                    draw_target(_x, _y);
                    draw_target(get_mirror_x(_x), _y);
                    break;
                case MirrorMode.Y:
                    draw_target(_x, _y);
                    draw_target(_x, get_mirror_y(_y));
                    break;
                case MirrorMode.XY:
                    var mirror_pt = get_mirror_pt(_x, _y);
                    draw_target(_x, _y);
                    draw_target(mirror_pt.X, _y);
                    draw_target(_x, mirror_pt.Y);
                    draw_target(mirror_pt.X, mirror_pt.Y);
                    break;
            }
        }
        void draw_modes()
        {
            for (int i = 0; i < 5; i++)
            {
                g_render.FillRectangle(i == (int)Mode ? Brushes.Gray : Brushes.DimGray, W - 10 - (5 - i) * 22, 10, 20, 20);
                g_render.DrawString($"{Enum.GetName(typeof(ToolModes), (ToolModes)i)[0]}", DefaultFont, i == (int)Mode ? Brushes.White : Brushes.Gray, W - 10 - (5 - i) * 22 + 4, 10+3);
            }
            g_render.DrawString($"draw mode :", DefaultFont, Brushes.White, W - 190, 13);

            for (int i = 0; i < 4; i++)
                g_render.FillRectangle(i == (int)mirror_mode ? Brushes.LightSteelBlue : Brushes.LightSlateGray, W - 10 - (4 - i) * 27, 40, 25, 20);
            g_render.DrawString($"mirror mode :", DefaultFont, Brushes.White, W - 190, 43);
            g_render.DrawString($"_", DefaultFont, Brushes.Black, W - 7 - 4 * 27 + 4, 40 + 3);
            g_render.DrawString($"X", DefaultFont, Brushes.Black, W - 7 - 3 * 27 + 4, 40 + 3);
            g_render.DrawString($"Y", DefaultFont, Brushes.Black, W - 7 - 2 * 27 + 4, 40 + 3);
            g_render.DrawString($"XY", DefaultFont, Brushes.Black, W - 10 - 1 * 27 + 4, 40 + 3);

            g_render.DrawString($"gap:{layer_gap}", DefaultFont, Brushes.White, W - 60, 70);
            g_render.DrawString($"layer:{fixed_layer}", DefaultFont, Brushes.White, W - 60, 90);
        }
        void draw_sun_and_mirror()
        {
            if (!ShowSunAndMirror)
                return;

            // SUN

            var gizmo_radius = 8F;
            var center = get_pos(Output).PlusF(imgw_scaled / 2F, imgh_scaled / 2F);
            g_render.DrawEllipse(Pens.Orange, get_world_sun.X - gizmo_radius, get_world_sun.Y - gizmo_radius, gizmo_radius * 2F, gizmo_radius * 2F);
            g_render.DrawLine(Pens.Yellow, get_world_sun, center);
            var look = center.Minus(get_world_sun).norm();
            g_render.DrawLine(Pens.Yellow, center, center.PlusF(look.Rotate(-135F).x(10F)));
            g_render.DrawLine(Pens.Yellow, center, center.PlusF(look.Rotate(135F).x(10F)));

            // MIRROR

            var pt = get_world_mirror;
            float x = pt.X, y = pt.Y;
            g_render.DrawLine(Pens.Violet, x, y - gizmo_radius, x, y + gizmo_radius);
            g_render.DrawLine(Pens.Violet, x - gizmo_radius, y, x + gizmo_radius, y);
        }
        void draw_shadows()
        {
            if (!ShowShadows || !refresh_shadows)
                return;

            ResetDraw(nameof(draw_shadow));
            var sun_pos = get_world_sun;
            var center = get_pos(Output).PlusF(imgw_scaled / 2F, imgh_scaled / 2F);
            var lenght = (center.Minus(sun_pos).Length() / (Maths.Diagonal(imgw_scaled, imgh_scaled) / 2F)) * 4F;
            Point look_lenght = center.Minus(sun_pos).norm().x(lenght).ToPoint();
            int i, j, _x, _y;

            for(int l=2; l<DATA_LAYERS+1; l++)
            {
                for (int y = 0; y < DATA_HEIGHT + 1; y++)
                {
                    for (int x = 0; x < DATA_WIDTH + 1; x++)
                    {
                        i = data[l, x, y];
                        if (i == 0) continue;
                        _x = x + look_lenght.X;
                        _y = y + look_lenght.Y;
                        j = data.PointedLayer(_x, _y);
                        if (j >= l || j == 0 || (x == _x && y == _y)) continue;
                        draw_shadow(pal[i], (l - 2) / (float)DATA_LAYERS, _x, _y);
                    }
                }
            }
            refresh_shadows = false;
        }
        void draw_shadow(Color c, float b, int x, int y)
        {
            g.FillRectangle(new SolidBrush(c.WithBrightness(b)), x * scale.Value, y * scale.Value, scale.Value, scale.Value);
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
