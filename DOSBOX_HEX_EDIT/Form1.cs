using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tooling;
using Tooling.UI;

namespace DOSBOX_HEX_EDIT
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerHue = new Timer() { Enabled = true, Interval = 10 };
        List<int> palette;
        vecf cam = vecf.Zero;
        float scale = 0.5f, min_scale = 0.5f, max_scale = 16f;
        float cam_speed => (max_scale - scale) * 1F;
        vec form_size;
        Graphics g;
        SolidBrush[] b = new SolidBrush[4];
        byte w = 16, h = 16;
        int hue = 0;
        Color hue_color;
        byte[] pixels;
        byte color_selection = 0, tool_selection = 0, tool_selection_prev = 0, pen_size = 1;
        float screen_margin => 2 * scale * 8;
        byte[] snapshot;
        bool undo_available = false, paste_done = false;
        vec tool_line_first_node = vec.Null;
        vec rect_selection_start = vec.Null, rect_selection_end = vec.Null;
        vec rect_selection_init = vec.Zero;
        copypastedata data_pasting;
        long ticks = 0;

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
            TimerHue.Tick += (s, e) =>
            {
                hue = (hue + 1) % 256;
                hue_color = hue.ColorFromHue();
            };
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
            UIMgt.UI.Add(new UIImage("pal_select", palette[0], palette[0], 2 + 102 * ix + 30 * 0, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal0",          palette[0], palette[0], 2 + 102 * ix + 30 * 1, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal1",          palette[1], palette[0], 2 + 102 * ix + 30 * 2, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal2",          palette[2], palette[0], 2 + 102 * ix + 30 * 3, 2, 30, 30));
            UIMgt.UI.Add(new UIImage("pal3",          palette[3], palette[0], 2 + 102 * ix + 30 * 4, 2, 30, 30));
        }

        private void Update(object _, EventArgs e)
        {
            if (!Focused)
                return;
            if (KB.IsKeyPressed(KB.Key.Escape))
                Close();
            global_update();
            ticks++;
            while (ticks > 256 * 256) ticks -= 256*256;
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
            _resize((byte)row[nm_w], (byte)row[nm_h]);
        }
        private void _resize(byte _w, byte _h)
        {
            byte ow = w;
            byte oh = h;
            w = _w;
            h = _h;
            resize_array(ref pixels, ow, oh, w, h, 3);
            undo_available = false;
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
            undo_available = false;
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
                undo_available = false;
            }
            MouseStates.ForceReleaseAllButtons();
        }
        private void ClickSave()
        {
            var box = new SaveFileDialog()
            {
                Filter = "HEX Files|*.hex",
                InitialDirectory = Directory.GetCurrentDirectory()//Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
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
            MouseStates.ForceReleaseAllButtons();
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
            else
            {
                if (KB.IsKeyPressed(KB.Key.L))
                    ClickLoad();
                if (KB.IsKeyPressed(KB.Key.S))
                    ClickSave();
                if (KB.IsKeyPressed(KB.Key.Enter))
                    Clipboard.SetText($"{w},{h},{string.Join(",", pixels)}");
                if (KB.IsKeyPressed(KB.Key.Insert))
                {
                    string data = string.Concat(Clipboard.GetText(TextDataFormat.Text).Split(','));
                    if (data.Length >= 3)
                    {
                        var _w = (byte)(data[0] - 48);
                        var _h = (byte)(data[1] - 48);
                        data = string.Concat(data.Skip(2));
                        _resize(_w, _h);
                        using (MemoryStream stream = new MemoryStream(data.Select(c => (byte)(c - 48)).ToArray()))
                        {
                            using (BinaryReader reader = new BinaryReader(stream))
                            {
                                pixels = reader.ReadBytes(w * h);
                            }
                        }
                        undo_available = false;
                    }
                }

                if (rect_selection_start != vec.Null && rect_selection_end != vec.Null)
                {
                    if (KB.IsKeyDown(KB.Key.D))
                    {
                        rect_selection_start = vec.Null;
                        rect_selection_end = vec.Null;
                    }
                    else if (KB.IsKeyPressed(KB.Key.X)) { do_selection_cut(); }
                    else if (KB.IsKeyPressed(KB.Key.C)) { do_selection_copy(); }
                }
                if (KB.IsKeyPressed(KB.Key.V))
                {
                    do_selection_prepare_paste();
                }
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
            int old_color_selection = color_selection;
            if (KB.IsKeyDown(KB.Key.Numpad0)) color_selection = 0;
            if (KB.IsKeyDown(KB.Key.Numpad1)) color_selection = 1;
            if (KB.IsKeyDown(KB.Key.Numpad2)) color_selection = 2;
            if (KB.IsKeyDown(KB.Key.Numpad3)) color_selection = 3;
            if(old_color_selection != color_selection)
                UIMgt.GetUIByName<UIImage>("pal_select").NewImageFromArgb(palette[color_selection]);
            if (KB.IsKeyDown(KB.Key.P)) tool_selection = 0;// pen
            if (KB.IsKeyDown(KB.Key.B)) tool_selection = 1;// bucket
            if (KB.IsKeyDown(KB.Key.L)) { tool_selection = 2; tool_line_first_node = vec.Null; }// line
            if (KB.LeftCtrl && undo_available && z)
                do_undo();
        }
        void do_tool()
        {
            if (KB.LeftAlt)
            {
                do_selection();
            }
            else
            {
                if (MouseStates.IsButtonDown(MouseButtons.Right))
                {
                    color_selection = get(ms.i);
                    var ui = UIMgt.GetUIByName<UIImage>("pal_select");
                    ui.NewImageFromArgb(palette[color_selection]);
                    ui.Update();
                }
                else
                {
                    switch (tool_selection)
                    {
                        case 0: do_tool_pen(); break;
                        case 1: do_tool_bucket(); break;
                        case 2: do_tool_line(); break;
                        case 3: do_tool_paste(); break;
                    }
                }
            }
        }
        void do_tool_pen()
        {
            if (MouseStates.ButtonsDown[MouseButtons.Left])
            {
                if (MouseStates.IsButtonPressed(MouseButtons.Left))
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
        void do_tool_line()
        {
            if (!MouseStates.IsButtonPressed(MouseButtons.Left))
                return;
            if(tool_line_first_node == vec.Null)
            {
                tool_line_first_node = ms.i;
            }
            else
            {
                prepare_undo();
                float L = ms.pt.MinusF(tool_line_first_node).Length();
                for (float t = 0F; t <= 1F; t += 1F / L)
                {
                    for (int x = 0; x < pen_size; x++)
                    {
                        for (int y = 0; y < pen_size; y++)
                        {
                            set(Maths.Lerp((tool_line_first_node.x + x, tool_line_first_node.y + y).Vf(), (ms.x + x, ms.y + y).Vf(), t), color_selection);
                        }
                    }
                }
                tool_line_first_node = vec.Null;
            }
        }
        void do_selection()
        {
            if (MouseStates.ButtonsDown[MouseButtons.Left])
            {
                int x = (int)ms.x;
                int y = (int)ms.y;
                if (MouseStates.IsButtonPressed(MouseButtons.Left))
                {
                    rect_selection_start.x = rect_selection_end.x = rect_selection_init.x = x;
                    rect_selection_start.y = rect_selection_end.y = rect_selection_init.y = y;
                }
                else
                {
                    if(x == rect_selection_init.x) rect_selection_start.x = rect_selection_end.x = x;
                    else if (x < rect_selection_init.x) rect_selection_start.x = x; else rect_selection_end.x = x;
                    if(y == rect_selection_init.y) rect_selection_start.y = rect_selection_end.y = y;
                    else if (y < rect_selection_init.y) rect_selection_start.y = y; else rect_selection_end.y = y;
                }
            }
        }
        void do_selection_cut()
        {
            do_selection_copy(true);
        }
        void do_selection_copy(bool cut = false)
        {
            int x = rect_selection_start.x;
            int y = rect_selection_start.y;
            int w = rect_selection_end.x - rect_selection_start.x;
            int h = rect_selection_end.y - rect_selection_start.y;
            Clipboard.SetText(new copypastedata { w=w, h=h, data=CopyFromPixels(x, y, w, h, cut) }.ToBase64());
        }
        void do_selection_prepare_paste()
        {
            var text = Clipboard.GetText();
            if (!string.IsNullOrWhiteSpace(text) && IsBase64String(text))
            {
                data_pasting = new copypastedata(text);
                tool_selection_prev = tool_selection;
                tool_selection = 3;
                rect_selection_start = vec.Null;
                rect_selection_end = vec.Null;
                paste_done = false;
            }
        }
        void do_tool_paste()
        {
            if(KB.IsKeyPressed(KB.Key.Enter) || KB.IsKeyPressed(KB.Key.Back) || KB.IsKeyPressed(KB.Key.Space))
                tool_selection = tool_selection_prev;
            else
            if (MouseStates.IsButtonPressed(MouseButtons.Left))
            {
                CopyToPixels(data_pasting.data, (int)ms.x - data_pasting.w / 2, (int)ms.y - data_pasting.h / 2, data_pasting.w, data_pasting.h);
                paste_done = true;
            }
        }

        void prepare_undo()
        {
            snapshot = new byte[pixels.Length];
            pixels.CopyTo(snapshot, 0);
            undo_available = true;
        }
        void do_undo()
        {
            snapshot.CopyTo(pixels, 0);
            undo_available = false;
        }

        byte[] preview_bytes;
        void bucket_preview()
        {
            preview_bytes = new byte[w * h];

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    preview_bytes[y * w + x] = get(x, y);
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
                    if (isin(p) && preview_bytes[p.y * w + p.x] != color_selection && get(p) == to_replace)
                        next_nodes.Add(p);
                }

                if (isin(v) && get(v) == to_replace)
                {
                    preview_bytes[v.y * w + v.x] = color_selection;
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
        void line_preview()
        {
            preview_bytes = new byte[w * h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    preview_bytes[y * w + x] = get(x, y);

            float L = ms.pt.MinusF(tool_line_first_node).Length();
            for (float t = 0F; t <= 1F; t += 1F / L)
            {
                for (int x = 0; x < pen_size; x++)
                {
                    for (int y = 0; y < pen_size; y++)
                    {
                        var v = Maths.Lerp((tool_line_first_node.x + x, tool_line_first_node.y + y).Vf(), (ms.x + x, ms.y + y).Vf(), t).i;
                        v.x = Math.Max(0, v.x);
                        v.y = Math.Max(0, v.y);
                        v.x = Math.Min(w, v.x);
                        v.y = Math.Min(h, v.y);
                        preview_bytes[v.y * w + v.x] = color_selection;
                    }
                }
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
                        if (IsintScreen(pt))
                            g.DrawRectangle(new Pen(Color.FromArgb(100, Color.Black)), pt.X, pt.Y, scale * 8, scale * 8);
                    }
                }
            }
            else if (tool_selection == 1)
            {
                SolidBrush[] _b = new SolidBrush[4];
                for (int i = 0; i < 4; i++)
                    _b[i] = new SolidBrush(Color.FromArgb(127, b[i].Color));
                if (MouseStates.LenghtDiff > 0)
                    bucket_preview();
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        if ((x + y) % 2 != 0) continue;
                        pt = WorldToScreen(x, y).pt;
                        if (IsintScreen(pt))
                            g.FillRectangle(_b[preview_bytes[y * w + x]], pt.X, pt.Y, scale * 8, scale * 8);
                    }
                }
            }
            else if (tool_selection == 2)
            {
                if (tool_line_first_node != vec.Null)
                {
                    SolidBrush[] _b = new SolidBrush[4];
                    for (int i = 0; i < 4; i++)
                        _b[i] = new SolidBrush(Color.FromArgb(127, b[i].Color));
                    if (MouseStates.LenghtDiff > 0)
                        line_preview();
                    for (int x = 0; x < w; x++)
                    {
                        for (int y = 0; y < h; y++)
                        {
                            pt = WorldToScreen(x, y).pt;
                            if (IsintScreen(pt))
                                g.FillRectangle(_b[preview_bytes[y * w + x]], pt.X, pt.Y, scale * 8, scale * 8);
                        }
                    }
                }
            }
            else if (tool_selection == 3)
            {
                SolidBrush[] _b = new SolidBrush[4];
                byte tick = (byte)(0x11 + ((int)ticks * 10) % (0xff - 0x22)).ByteCut();
                for (int i = 0; i < 4; i++)
                    _b[i] = new SolidBrush(Color.FromArgb(tick, b[i].Color));
                vec sz = (data_pasting.w, data_pasting.h).V();
                for (int x = 0; x < data_pasting.w; x++)
                {
                    for (int y = 0; y < data_pasting.h; y++)
                    {
                        pt = WorldToScreen(ms.i.x + x - sz.x / 2, ms.i.y + y - sz.y / 2).pt;
                        if (IsintScreen(pt))
                            g.FillRectangle(_b[data_pasting.data[y * data_pasting.w + x]], pt.X, pt.Y, scale * 8, scale * 8);
                    }
                }
                pt = WorldToScreen(ms.i.x - sz.x / 2, ms.i.y - sz.y / 2).pt;
                g.DrawRectangle(Pens.Black, pt.X, pt.Y, data_pasting.w * scale * 8, data_pasting.h * scale * 8);
            }

            if (rect_selection_start != vec.Null && rect_selection_end != vec.Null)
            {
                vec screen_start = WorldToScreen(rect_selection_start.f).i;
                vec screen_end = WorldToScreen(rect_selection_end.f).i;
                float x = screen_start.x, y = screen_start.y;
                float w = screen_end.x - x, h = screen_end.y - y;
                float sz = 2F * scale;
                g.DrawRectangle(new Pen(hue_color, sz), x - sz / 2f, y - sz / 2f, w + sz / 2f, h + sz / 2f);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            form_size = Size.V();
        }
        private byte[] CopyFromPixels(int i, int j, int w, int h, bool cut = false)
        {
            byte[] result = new byte[w * h];
            int index = 0;

            for (int y = j; y < j + h; y++)
            {
                for (int x = i; x < i + w; x++)
                {
                    result[index] = pixels[y * this.w + x];
                    if (cut)
                        pixels[y * this.w + x] = 3;
                    index++;
                }
            }

            return result;
        }
        private void CopyToPixels(byte[] data, int i, int j, int w, int h)
        {
            for (int y = j; y < j + h; y++)
            {
                for (int x = i; x < i + w; x++)
                {
                    if(isin(x,y))
                        pixels[y * this.w + x] = data[(y-j) * w + (x - i)];
                }
            }
        }

        [Serializable]
        public struct copypastedata
        {
            public int w, h;
            public byte[] data;
            public copypastedata(copypastedata copy)
            {
                w = copy.w;
                h = copy.h;
                data = copy.data;
            }
            public string ToBase64() => ConvertToBase64(this);
            public copypastedata(string base64)
            {
                var obj = ConvertFromBase64<copypastedata>(base64);
                w = obj.w;
                h = obj.h;
                data = obj.data;
            }
        }
        public static string ConvertToBase64<T>(T obj)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        public static T ConvertFromBase64<T>(string base64String) where T:struct
        {
            if (string.IsNullOrWhiteSpace(base64String) || !IsBase64String(base64String))
                return default;
            var binaryFormatter = new BinaryFormatter();
            byte[] bytes = Convert.FromBase64String(base64String);
            using (var memoryStream = new MemoryStream(bytes, 0, bytes.Length))
            {
                try
                {
                    return (T)binaryFormatter.Deserialize(memoryStream);
                }
                catch (SerializationException) { return default; }
            }
        }
        public static bool IsBase64String(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}
