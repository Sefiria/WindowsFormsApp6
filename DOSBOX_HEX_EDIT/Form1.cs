using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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
        byte w = 255, h = 255;
        byte[] pixels;
        byte color_selection = 0, tool_selection = 0, pen_size = 1;
        float screen_margin => 2 * scale * 8;
        byte[] snapshot;
        bool undo_available = false, tool_pen_prevLeftDown = false;

        bool IsOutScreen(vec v) => v.x < -screen_margin || v.y < -screen_margin || v.x >= form_size.x + screen_margin || v.y >= form_size.y + screen_margin;
        bool IsOutScreen(PointF pt) => pt.X < -screen_margin || pt.Y < -screen_margin || pt.X >= form_size.x + screen_margin || pt.Y >= form_size.y + screen_margin;
        bool IsintScreen(vec v) => !IsOutScreen(v);
        bool IsintScreen(PointF pt) => !IsOutScreen(pt);
        vecf WorldToScreen(float x, float y) => new vecf(x * scale * 8 - (cam.x * scale - form_size.x / 2), y * scale * 8 - (cam.y * scale - form_size.y / 2));
        vecf WorldToScreen(vecf v) => WorldToScreen(v.x, v.y);
        vecf ScreenToWorld(float x, float y) => new vecf((x + (cam.x * scale - form_size.x / 2)) / (scale * 8), (y + (cam.y * scale - form_size.y / 2)) / (scale * 8));
        vecf ScreenToWorld(vecf v) => ScreenToWorld(v.x, v.y);
        vecf ms => ScreenToWorld(MouseStates.Position.vecf());
        vecf msold => ScreenToWorld(MouseStates.OldPosition.vecf());
        bool isout(int x, int y) => x < 0 || y < 0 || x >= w || y >= h;
        bool isout(vec v) => isout(v.x, v.y);
        bool isin(int x, int y) => !isout(x, y);
        bool isin(vec v) => isin(v.x, v.y);
        byte get(int x, int y) => isin(x, y) ? pixels[y * w + x] : (byte)3;
        byte get(vec v) => get(v.x, v.y);
        void set(int x, int y, int v) { if(isin(x, y)) pixels[y * w + x] = (byte) v; }
        void set(vec vec, int v) => set(vec.x, vec.y, v);
        void set(vecf vec, int v) => set(vec.i, v);
        void clear()
        {
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    pixels[y * w + x] = 3;
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
            cam = form_size.f / 2f;
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
            pixels = new byte[w * h];
            snapshot = new byte[w * h];
            clear();
            pixels.CopyTo(snapshot, 0);

            init_ui();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }
        void init_ui()
        {
            UIButton UIButtonFactory_Create(int x, int y, int w, int h, string name, Action<UI> onClick)
            {
                return new UIButton()
                {
                    Position = (x, y).Vf(),
                    Size = (w, h).Vf(),
                    IsDrawingName = true,
                    BackgroundColor = Color.LightGray,
                    BoundsColor = Color.Black,
                    Name = name,
                    OnClick = onClick
                };
            }

            int ix = 0;
            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * ix++, 2, 100, 30, "Palette", (e) => ClickPalette()));
            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * ix++, 2, 100, 30, "Size", (e) => ClickSize()));
            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * ix++, 2, 100, 30, "New", (e) => ClickNew()));
            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * ix++, 2, 100, 30, "Load", (e) => ClickLoad()));
            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * ix++, 2, 100, 30, "Save", (e) => ClickSave()));
            UIMgt.UI.Add(UIButtonFactory_Create(2 + 102 * ix++, 2, 100, 30, "Recenter", (e) => cam = vecf.Zero));
            UIMgt.UI.Add(new UIImage("pal0", palette[0], palette[0], 2 + 102 * ix + 30 * 0, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal1", palette[1], palette[0], 2 + 102 * ix + 30 * 1, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal2", palette[2], palette[0], 2 + 102 * ix + 30 * 2, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal3", palette[3], palette[0], 2 + 102 * ix + 30 * 3, 2, 30, 30));
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
        private void ClickSize()
        {
            string nm_w = "Width", nm_h = "Height";
            var db = new DataTable();
            db.Columns.Add(nm_w, typeof(byte));
            db.Columns.Add(nm_h, typeof(byte));
            db.Rows.Add(w, h);
            var box = new DGVBox(db);
            box.ShowDialog();
            var row = db.Rows[0];
            byte ow = w;
            byte oh = h;
            w = (byte)row[nm_w];
            h = (byte)row[nm_h];
            resize_array(ref pixels, ow, oh, w, h, 3);
        }
        void resize_array(ref byte[] array, int ow, int oh, int nw, int nh, byte defaultValue)
        {
            byte[] new_array = new byte[nw * nh];
            for (int i = 0; i < new_array.Length; i++)
            {
                int x = i % nw;
                int y = i / nw;
                new_array[i] = (x < ow && y < oh) ? array[y * ow + x] : defaultValue;
            }
            array = new_array;
        }
        private void ClickNew()
        {
            pixels.CopyTo(snapshot, 0);
            undo_available = true;
            pixels = new byte[w * h];
            clear();
        }
        private void ClickLoad()
        {
            var box = new OpenFileDialog()
            {
                Filter = "HEX Files|*.hex",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (box.ShowDialog() == DialogResult.OK)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(box.FileName, FileMode.Open)))
                {
                    w = reader.ReadByte();
                    h = reader.ReadByte();
                    pixels = reader.ReadBytes(w * h);
                }
            }
        }
        private void ClickSave()
        {
            var box = new SaveFileDialog()
            {
                Filter = "HEX Files|*.hex",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (box.ShowDialog() == DialogResult.OK)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(box.FileName, FileMode.Create)))
                {
                    writer.Write(w);
                    writer.Write(h);
                    writer.Write(pixels);
                }
            }
        }

        void global_update()
        {
            do_controls();
            do_tool();
        }
        void do_controls()
        {
            var (z, q, s, d) = KB.ZQSD();
            if (!KB.LeftCtrl)
            {
                if (z) cam.y -= cam_speed;
                if (s) cam.y += cam_speed;
                if (q) cam.x -= cam_speed;
                if (d) cam.x += cam_speed;
            }
            if (MouseStates.Delta != 0f)
            {
                if (KB.LeftShift) // pen size
                {
                    pen_size = (byte)(pen_size + MouseStates.Delta / 100F).ByteCut();
                }
                else // zoom
                {
                    scale = Math.Min(max_scale, Math.Max(min_scale, scale + MouseStates.Delta / 1000F));
                }
            }
            if (KB.IsKeyDown(KB.Key.Numpad0)) color_selection = 0;
            if (KB.IsKeyDown(KB.Key.Numpad1)) color_selection = 1;
            if (KB.IsKeyDown(KB.Key.Numpad2)) color_selection = 2;
            if (KB.IsKeyDown(KB.Key.Numpad3)) color_selection = 3;
            if (KB.IsKeyDown(KB.Key.P)) tool_selection = 0;// pen
            if (KB.IsKeyDown(KB.Key.B)) tool_selection = 1;// bucket
            if (KB.LeftCtrl && undo_available && z)
                do_undo();
        }
        void do_tool()
        {
            switch(tool_selection)
            {
                case 0: do_tool_pen(); break;
                case 1: do_tool_bucket(); break;
            }
        }
        void do_tool_pen()
        {
            if (MouseStates.ButtonsDown[MouseButtons.Left])
            {
                tool_pen_prevLeftDown = true;
                prepare_undo();
                float L = MouseStates.LenghtDiff;
                for (float t = 0F; t <= 1F; t += 1F / L)
                {
                    for (int x = 0; x < pen_size; x++)
                    {
                        for (int y = 0; y < pen_size; y++)
                        {
                            set(Maths.Lerp((msold.x + x, msold.y + y).Vf(), (ms.x + x, ms.y + y).Vf(), t), color_selection);
                        }
                    }
                }
            }
            else
            {
                tool_pen_prevLeftDown = false;
            }
        }
        void do_tool_bucket()
        {
            if (MouseStates.IsButtonPressed(MouseButtons.Left))
            {
                prepare_undo();
                List<vec> nodes = new List<vec> { ms.i };
                List<vec> next_nodes = new List<vec>();
                byte to_replace = get(nodes[0]);

                if (to_replace == color_selection)
                    return;

                void recursive_4_ways(vec v)
                {
                    void check(int x, int y)
                    {
                        var p = (v.x + x, v.y + y).V();
                        if (isin(p) && get(p) == to_replace)
                            next_nodes.Add(p);
                    }

                    if (get(v) == to_replace)
                    {
                        set(v, color_selection);
                        check(-1, 0);
                        check(1, 0);
                        check(0, -1);
                        check(0, 1);
                    }
                }

                int timout = 255;
                while (nodes.Count > 0 && timout > 0)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        recursive_4_ways(nodes[i]);
                        nodes.RemoveAt(i--);
                    }
                    nodes.AddRange(next_nodes);
                    next_nodes.Clear();
                    timout--;
                }
            }
        }
        void prepare_undo()
        {
            pixels.CopyTo(snapshot, 0);
            undo_available = true;
        }
        void do_undo()
        {
            snapshot.CopyTo(pixels, 0);
            undo_available = false;
        }

        byte[] bucket_preview_bytes;
        void bucket_preview()
        {
            bucket_preview_bytes = new byte[w * h];

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    bucket_preview_bytes[y * w + x] = get(x, y);
            List<vec> nodes = new List<vec> { ms.i };
            List<vec> next_nodes = new List<vec>();
            byte to_replace = get(nodes[0]);

            if (to_replace == color_selection)
                return;

            void recursive_4_ways(vec v)
            {
                void check(int x, int y)
                {
                    var p = (v.x + x, v.y + y).V();
                    if (isin(p) && bucket_preview_bytes[p.y * w + p.x] != color_selection && get(p) == to_replace)
                        next_nodes.Add(p);
                }

                if (isin(v) && get(v) == to_replace)
                {
                    bucket_preview_bytes[v.y * w + v.x] = color_selection;
                    check(-1, 0);
                    check(1, 0);
                    check(0, -1);
                    check(0, 1);
                }
            }

            int timout = 64;
            while (nodes.Count > 0 && timout > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    recursive_4_ways(nodes[i]);
                    nodes.RemoveAt(i--);
                }
                nodes.AddRange(next_nodes.Distinct().ToList());
                next_nodes.Clear();
                timout--;
            }
        }

        void global_draw()
        {
            PointF pt;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    pt = WorldToScreen(x, y).pt;
                    if(IsintScreen(pt))
                        g.FillRectangle(b[get(x, y)], pt.X, pt.Y, scale * 8, scale * 8);
                }
            }

            if (tool_selection == 0)
            {
                for (int x = 0; x < pen_size; x++)
                {
                    for (int y = 0; y < pen_size; y++)
                    {
                        pt = WorldToScreen((int)ms.x + x, (int)ms.y + y).pt;
                        if(IsintScreen(pt))
                            g.DrawRectangle(new Pen(Color.FromArgb(100, Color.Black)), pt.X, pt.Y, scale * 8, scale * 8);
                    }
                }
            }
            else if (tool_selection == 1)
            {
                SolidBrush[] _b = new SolidBrush[4];
                for (int i = 0; i < 4; i++)
                    _b[i] = new SolidBrush(Color.FromArgb(127, b[i].Color));
                if(MouseStates.LenghtDiff > 0)
                    bucket_preview();
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        if ((x + y) % 2 != 0) continue;
                        pt = WorldToScreen(x, y).pt;
                        if(IsintScreen(pt))
                            g.FillRectangle(_b[bucket_preview_bytes[y * w + x]], pt.X, pt.Y, scale * 8, scale * 8);
                    }
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            form_size = Size.V();
        }
    }
}
