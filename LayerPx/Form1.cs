using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
        PointF Center = PointF.Empty, Cam = PointF.Empty, sun, mirror_src, begin_line = PointF.Empty;
        Tools Tool = Tools.PenSquare;
        MirrorMode mirror_mode = MirrorMode.None;
        bool ShowGrid = false, ShowSunAndMirror = false, ShowShadows = true, refresh_shadows = false, holding_layer_released = true, mouseleft_released = true;
        DATA data;
        Color[] pal = new Color[16];// 0 is transparent
        int pal_index_primary = 1, m_pen_size = 1, holding_layer_target = DATA_LAYERS / 2, fixed_layer = DATA_LAYERS / 2, layer_gap = 1;

        const int DATA_LAYERS = 8, DATA_WIDTH = 127, DATA_HEIGHT = 127;
        const float CAM_MOV_SPD = 3F, SUN_MOV_SPD = 2F;
        const float SCALE_GAP = 0.5F;

        RangeValueF scale = new RangeValueF(1F, 2F, 10F);
        PointF get_world_sun => get_pos(sun); 
        PointF get_world_mirror => get_pos(mirror_src); 

        PointF get_pos(PointF pt) => Center.PlusF(pt.MinusF(Cam).x(scale.Value));
        PointF get_pos(Bitmap _img) => Center.MinusF(imgw_scaled / 2, imgh_scaled / 2).MinusF(Cam.x(scale.Value));
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
        int get_mirror_x(float x) => (int)(mirror_src.X - 0.5F + imgw / 2F + (mirror_src.X - 0.5F + imgw / 2F - x));
        int get_mirror_y(float y) => (int)(mirror_src.Y - 0.5F + imgh / 2F + (mirror_src.Y - 0.5F + imgh / 2F - y));
        Point get_mirror_pt(int x, int y) => new Point(get_mirror_x(x), get_mirror_y(y));
        int pen_size => m_pen_size == 1 ? 1 : m_pen_size + m_pen_size % 2;

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

            if (IsKeyDown(Key.Z)) { Cam = Cam.MinusF(0F, CAM_MOV_SPD * Amplitude); refresh_shadows = true; }
            if (IsKeyDown(Key.Q)) { Cam = Cam.MinusF(CAM_MOV_SPD * Amplitude, 0F); refresh_shadows = true; }
            if (IsKeyDown(Key.S)) { Cam = Cam.PlusF(0F, CAM_MOV_SPD * Amplitude); refresh_shadows = true; }
            if (IsKeyDown(Key.D)) { Cam = Cam.PlusF(CAM_MOV_SPD * Amplitude, 0F); refresh_shadows = true; }

            if(ShowSunAndMirror)
            {
                if (IsKeyDown(Key.LeftCtrl))
                {
                    if (IsKeyDown(Key.Numpad8)) { sun = sun.MinusF(0F, SUN_MOV_SPD * Amplitude); refresh_shadows = true; }
                    if (IsKeyDown(Key.Numpad4)) { sun = sun.MinusF(SUN_MOV_SPD * Amplitude, 0F); refresh_shadows = true; }
                    if (IsKeyDown(Key.Numpad2)) { sun = sun.PlusF(0F, SUN_MOV_SPD * Amplitude); refresh_shadows = true; }
                    if (IsKeyDown(Key.Numpad6)) { sun = sun.PlusF(SUN_MOV_SPD * Amplitude, 0F); refresh_shadows = true; }
                }
                else
                {
                    if (IsKeyDown(Key.Numpad8)) mirror_src = mirror_src.MinusF(0F, SUN_MOV_SPD * 0.2F * Amplitude);
                    if (IsKeyDown(Key.Numpad4)) mirror_src = mirror_src.MinusF(SUN_MOV_SPD * 0.2F * Amplitude, 0F);
                    if (IsKeyDown(Key.Numpad2)) mirror_src = mirror_src.PlusF(0F, SUN_MOV_SPD * 0.2F * Amplitude);
                    if (IsKeyDown(Key.Numpad6)) mirror_src = mirror_src.PlusF(SUN_MOV_SPD * 0.2F * Amplitude, 0F);
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
                    fixed_layer = data.PointedLayer(get_pos_ms());
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
                    m_pen_size += (int)((MouseStates.Delta < 0 ? -1 : 1) * Amplitude);
                    if (m_pen_size < 1) m_pen_size = 1;
                    if (m_pen_size > Math.Min(DATA_WIDTH, DATA_HEIGHT) / 2) m_pen_size = Math.Min(DATA_WIDTH, DATA_HEIGHT) / 2;
                }
            }

            if (IsKeyPressed(Key.LeftAlt))
                begin_line = get_pos_ms();
            else if (!IsKeyDown(Key.LeftAlt))
                begin_line = PointF.Empty;

            if (MouseStates.IsDown == false)
            {
                mouseleft_released = true;
                holding_layer_released = true;
            }
            else if (MouseStates.ButtonDown != MouseButtons.Middle)
            {
                var ms = get_pos_ms();
                if (mouseleft_released)
                    layer_at_first_press = data.PointedLayer(ms);
                mouseleft_released = false;
                int v = MouseStates.ButtonDown == MouseButtons.Left ? pal_index_primary : 0;

                if (begin_line != PointF.Empty)
                {
                    RangeValue x, y;
                    PointF lon = ms.MinusF(begin_line), rotated_lon;
                    float l = lon.Length();
                    if (IsKeyDown(Key.LeftCtrl)) // draw circle
                    {
                        for (float t = 0F; t <= 360F; t += 100F / l)
                        {
                            rotated_lon = Maths.Rotate(lon, t);
                            x = new RangeValue((int)(begin_line.X + rotated_lon.X / 2), 0, DATA_WIDTH);
                            y = new RangeValue((int)(begin_line.Y + rotated_lon.Y / 2), 0, DATA_HEIGHT);
                            UseTool((int)lon.X / 2 + x.Value, (int)lon.Y / 2 + y.Value, v);
                        }
                    }
                    else // draw line
                    {
                        for (float t = 0F; t <= 1F; t += 1F / l)
                        {
                            x = new RangeValue((int)Maths.Lerp(begin_line.X, ms.X, t), 0, DATA_WIDTH);
                            y = new RangeValue((int)Maths.Lerp(begin_line.Y, ms.Y, t), 0, DATA_HEIGHT);
                            UseTool(x.Value, y.Value, v);
                        }
                    }
                }

                if (MouseStates.OldPosition != Point.Empty && MouseStates.PositionChanged)
                {
                    var old = get_pos_oldms();
                    var m = ms;
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

            if (IsKeyPressed(Key.C) && IsKeyDown(Key.LeftCtrl))
                ExportClipboard();

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

            void draw_target(float x, float y, float begin_line_x=0, float begin_line_y = 0)
            {
                void internal_draw(int __x, int __y)
                {
                    if (Tool == Tools.PenCircle)
                        g_render.DrawEllipse(Pens.Gray, pos.X + (__x - (pen_size - 1) / 2) * scale.Value, pos.Y + (__y - (pen_size - 1) / 2) * scale.Value, pen_size * scale.Value, pen_size * scale.Value);
                    else if (Tool == Tools.PenSquare)
                        g_render.DrawRectangle(Pens.Gray, pos.X + (__x - (pen_size-1) / 2) * scale.Value, pos.Y + (__y - (pen_size - 1) / 2) * scale.Value, pen_size * scale.Value, pen_size * scale.Value);
                }

                if (begin_line != PointF.Empty) // draw a line / circle
                {
                    if (IsKeyDown(Key.LeftCtrl) == false) // line
                    {
                        int i, j;
                        float l = (x, y).P().MinusF(begin_line_x, begin_line_y).Length();
                        for (float t = 0F; t <= 1F; t += 1F / l)
                        {
                            i = (int)Maths.Range(0, DATA_WIDTH, Maths.Lerp(begin_line_x, x, t));
                            j = (int)Maths.Range(0, DATA_HEIGHT, Maths.Lerp(begin_line_y, y, t));
                            internal_draw(i, j);
                        }
                    }
                    else // circle
                    {
                        int __x, __y;
                        PointF lon = (x, y).P().MinusF(begin_line_x, begin_line_y), rotated_lon;
                        float l = lon.Length();
                        for (float t = 0F; t <= 360F; t += 100F / l)
                        {
                            rotated_lon = Maths.Rotate(lon, t);
                            __x = (int)Maths.Range(0, DATA_WIDTH * scale.Value, (int)lon.X / 2 + (int)(begin_line_x + rotated_lon.X / 2));
                            __y = (int)Maths.Range(0, DATA_HEIGHT * scale.Value, (int)lon.Y / 2 + (int)(begin_line_y + rotated_lon.Y / 2));
                            if (pen_size == 1)
                                g_render.DrawRectangle(Pens.Gray, pos.X + __x * scale.Value, pos.Y + __y * scale.Value, scale.Value, scale.Value);
                            else
                            {
                                for (int j = -pen_size / 2; j <= pen_size / 2; j++)
                                    for (int i = -pen_size / 2; i <= pen_size / 2; i++)
                                        if (data.check_pass(__x - i, __y - j))
                                            g_render.DrawRectangle(Pens.Gray, pos.X + (__x - i) * scale.Value, pos.Y + (__y - j) * scale.Value, scale.Value, scale.Value);
                            }
                        }
                    }
                }
                internal_draw((int)x, (int)y);
            }

            var mirror_pt = get_mirror_pt(_x, _y).ToPointF().PlusF(1,1);
            var mirror_bl = get_mirror_pt((int)begin_line.X, (int)begin_line.Y).ToPointF().PlusF(1, 1);
            switch (mirror_mode)
            {
                case MirrorMode.None: if (begin_line != PointF.Empty) draw_target(_x, _y, begin_line.X, begin_line.Y); else draw_target(_x, _y); break;
                case MirrorMode.X:
                    if (begin_line != PointF.Empty)
                    {
                        draw_target(_x, _y, begin_line.X, begin_line.Y);
                        draw_target(mirror_pt.X, _y, mirror_bl.X, begin_line.Y);
                    }
                    else
                    {
                        draw_target(_x, _y);
                        draw_target(mirror_pt.X, _y);
                    }
                    break;
                case MirrorMode.Y:
                    if (begin_line != PointF.Empty)
                    {
                        draw_target(_x, _y, begin_line.X, begin_line.Y);
                        draw_target(_x, mirror_pt.Y, begin_line.X, mirror_bl.Y);
                    }
                    else
                    {
                        draw_target(_x, _y);
                        draw_target(_x, mirror_pt.Y);
                    }
                    break;
                case MirrorMode.XY:
                    if (begin_line != PointF.Empty)
                    {
                        draw_target(_x, _y, begin_line.X, begin_line.Y);
                        draw_target(mirror_pt.X, _y, mirror_bl.X, begin_line.Y);
                        draw_target(_x, mirror_pt.Y, begin_line.X, mirror_bl.Y);
                        draw_target(mirror_pt.X, mirror_pt.Y, mirror_bl.X, mirror_bl.Y);
                    }
                    else
                    {
                        draw_target(_x, _y);
                        draw_target(mirror_pt.X, _y);
                        draw_target(_x, mirror_pt.Y);
                        draw_target(mirror_pt.X, mirror_pt.Y);
                    }
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
            var look = center.MinusF(get_world_sun).norm();
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
            var lenght = (center.MinusF(sun_pos).Length() / (Maths.Diagonal(imgw_scaled, imgh_scaled) / 2F)) * 4F;
            Point look_lenght = center.MinusF(sun_pos).norm().x(lenght).ToPoint();
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


        // IMPORT / EXPORT

        private void ExportClipboard()
        {
            Clipboard.SetImage(Output);
        }
    }
}
