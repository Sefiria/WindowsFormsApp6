using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using Tooling.UI;

namespace DOSBOX_HEX_EDIT
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        List<int> palette;
        vecf cam = vecf.Zero;
        float scale = 0.5f, min_scale = 0.5f, max_scale = 16f;
        float cam_speed => (max_scale - scale) * 1F;
        vec form_size;
        Graphics g;
        SolidBrush[] b = new SolidBrush[4];
        byte[] pixels = new byte[256 * 256];
        byte color_selection = 0, tool_selection = 0, pen_size = 1;

        vecf WorldToScreen(float x, float y) => new vecf(x * scale * 8 - (cam.x * scale - form_size.x / 2), y * scale * 8 - (cam.y * scale - form_size.y / 2));
        vecf WorldToScreen(vecf v) => WorldToScreen(v.x, v.y);
        vecf ScreenToWorld(float x, float y) => new vecf((x + (cam.x * scale - form_size.x / 2)) / (scale * 8), (y + (cam.y * scale - form_size.y / 2)) / (scale * 8));
        vecf ScreenToWorld(vecf v) => ScreenToWorld(v.x, v.y);
        vecf ms => ScreenToWorld(MouseStates.Position.vecf());
        vecf msold => ScreenToWorld(MouseStates.OldPosition.vecf());
        bool isout(int x, int y) => x < 0 || y < 0 || x > 255 || y > 255;
        bool isin(int x, int y) => !isout(x, y);
        byte get(int x, int y) => isin(x, y) ? pixels[y * 256 + x] : (byte)3;
        void set(int x, int y, int v) { if(isin(x, y)) pixels[y * 256 + x] = (byte) v; }
        void set(vecf vec, int v) => set((int)vec.x, (int)vec.y, v);
        void clear()
        {
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 256; y++)
                    pixels[y * 256 + x] = 3;
        }
        void set_brushes()
        {
            for (int i = 0; i < 4; i++)
                b[i] = new SolidBrush(Color.FromArgb(palette[i]));
        }

        public Form1()
        {
            InitializeComponent();
            form_size = Size.V();
            cam = form_size.f / 2f / scale;
            KB.Init();
            MouseStates.Initialize(Render);
            UIMgt.MouseStatesVersion = 2;
            palette = new List<int>
            {
                (35, 48, 64).ToArgb(),
                (88, 139, 112).ToArgb(),
                (175, 207, 151).ToArgb(),
                (219, 229, 219).ToArgb()
            };
            set_brushes();
            clear();

            UIButton UIButtonFactory_Create(int x, int y, int w, int h, string name, Action<UI> onClick)
            {
                return new UIButton() {
                    Position = (x, y).Vf(),
                    Size = (w, h).Vf(),
                    IsDrawingName = true,
                    BackgroundColor = Color.LightGray,
                    BoundsColor = Color.Black,
                    Name = name,
                    OnClick = onClick
                };
            }

            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * 0, 2, 100, 30, "Palette", (e) => ClickPalette()));
            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * 1, 2, 100, 30, "New", (e) => ClickNew()));
            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * 2, 2, 100, 30, "Load", (e) => ClickLoad()));
            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * 3, 2, 100, 30, "Save", (e) => ClickSave()));
            UIMgt.UI.Add(new UIImage("pal0", palette[0], palette[0], 2 + 102 * 4 + 30 * 0, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal1", palette[1], palette[0], 2 + 102 * 4 + 30 * 1, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal2", palette[2], palette[0], 2 + 102 * 4 + 30 * 2, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal3", palette[3], palette[0], 2 + 102 * 4 + 30 * 3, 2, 30, 30));

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
                Close();
            global_update();
            KB.Update();
            UIMgt.Update();
            MouseStates.Update();
        }
        private void Draw(object _, EventArgs e)
        {
            Bitmap Image = new Bitmap(form_size.x, form_size.y);
            g = Graphics.FromImage(Image);
            g.Clear(Color.Black);
            global_draw();
            UIMgt.Draw(g);
            Render.Image = Image;
        }
        protected override void OnClosed(EventArgs e)
        {
            TimerUpdate.Tick -= Update;
            TimerDraw.Tick -= Draw;
            System.Threading.Thread.Sleep(200);
            base.OnClosed(e);
        }

        private void ClickPalette()
        {
            string nm_color = "Color Name", nm_argb = "ARGB (int32)", nm_r = "Hex R (byte)", nm_g = "Hex G (byte)", nm_b = "Hex B (byte)";
            var db = new DataTable();
            db.Columns.Add(nm_color, typeof(string));
            db.Columns.Add(nm_argb, typeof(int));
            db.Columns.Add(nm_r, typeof(byte));
            db.Columns.Add(nm_g, typeof(byte));
            db.Columns.Add(nm_b, typeof(byte));
            for (int i = 0; i < 4; i++)
            {
                Color c = Color.FromArgb(palette[i]);
                string name;
                switch(i)
                {
                    default: case 0: name = "black"; break;
                    case 1: name = "middle"; break;
                    case 2: name = "bright"; break;
                    case 3: name = "clear"; break;
                }
                db.Rows.Add(name, palette[i], c.R, c.G, c.B);
            }
            var box = new DGVBox(db);
            box.DGV.Columns[nm_argb].ReadOnly = true;
            box.DGV.Columns[nm_argb].DefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightGray };
            box.DGV.CellValueChanged += (s, e) =>
            {
                var r = (byte)box.DGV.Rows[e.RowIndex].Cells[nm_r].Value;
                var g = (byte)box.DGV.Rows[e.RowIndex].Cells[nm_g].Value;
                var b = (byte)box.DGV.Rows[e.RowIndex].Cells[nm_b].Value;
                var argb = Color.FromArgb(r, g, b).ToArgb();
                box.DGV.Rows[e.RowIndex].Cells[nm_argb].Value = argb;
            };
            box.ShowDialog();
            for (int i = 0; i < 4; i++)
            {
                palette[i] = (int)db.Rows[i][nm_argb];
                set_brushes();
                UIMgt.GetUIByName<UIImage>($"pal{i}").NewImageFromArgb(palette[i]);
            }
        }
        private void ClickNew()
        {
        }
        private void ClickLoad()
        {
        }
        private void ClickSave()
        {
        }


        void global_update()
        {
            do_controls();
            do_tool();
        }
        void do_controls()
        {
            var (z, q, s, d) = KB.ZQSD();
            if (z) cam.y -= cam_speed;
            if (s) cam.y += cam_speed;
            if (q) cam.x -= cam_speed;
            if (d) cam.x += cam_speed;
            if (MouseStates.Delta != 0f) scale = Math.Min(max_scale, Math.Max(min_scale, scale + MouseStates.Delta / 100F));
            if (KB.IsKeyDown(KB.Key.Numpad0)) color_selection = 0;
            if (KB.IsKeyDown(KB.Key.Numpad1)) color_selection = 1;
            if (KB.IsKeyDown(KB.Key.Numpad2)) color_selection = 2;
            if (KB.IsKeyDown(KB.Key.Numpad3)) color_selection = 3;
            if (KB.IsKeyDown(KB.Key.P)) tool_selection = 0;// pen
            if (KB.IsKeyDown(KB.Key.B)) tool_selection = 1;// bucket
        }
        void do_tool()
        {
            if (MouseStates.ButtonsDown[MouseButtons.Left])
            {
                float L = MouseStates.LenghtDiff;
                for (float t = 0F; t <= 1F; t += 1F / L)
                    set(Maths.Lerp(msold, ms, t), color_selection);
            }
        }

        void global_draw()
        {
            PointF pt;
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    pt = WorldToScreen(x, y).pt;
                    g.FillRectangle(b[get(x, y)], pt.X, pt.Y, scale * 8, scale * 8);
                }
            }
            pt = WorldToScreen(ms.i.f).pt;  
            g.DrawRectangle(new Pen(Color.FromArgb(100, Color.Black)), pt.X, pt.Y, scale * 8, scale * 8);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            form_size = Size.V();
        }
    }
}
