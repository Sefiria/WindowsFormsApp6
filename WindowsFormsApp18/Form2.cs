using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using WindowsFormsApp18.utilities;
using WindowsFormsApp18.Utilities;

namespace WindowsFormsApp18
{
    public partial class Form2 : Form
    {
        Bitmap img;

        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        MouseEventArgs LastMouseEventArgs = null;
        Map map;
        Point start;

        public Form2()
        {
            InitializeComponent();

            Core.rw = Render.Width;
            Core.rh = Render.Height;
            img = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(img);

            ResMgr.Init();

            map = new Map();

            KB.Init();
            Core.Init();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            if (LastMouseEventArgs != null)
                Render_MouseMove(null, LastMouseEventArgs);

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl))
            {
                if (KB.IsKeyPressed(KB.Key.C))
                    CopyData();
                if (KB.IsKeyPressed(KB.Key.V))
                    PasteData();
            }

            KB.Update();
        }

        private void CopyData()
        {
            var data = new JsonData() { map = map };
            Clipboard.SetText(JsonSerializer.Serialize(data));
        }
        private void PasteData()
        {
            JsonData data = null;
            try
            {
                data = JsonSerializer.Deserialize<JsonData>(Clipboard.GetText());
            }
            catch (Exception e) { }
            if (data != null)
                map = data.map;
        }

        private void Draw(object _, EventArgs e)
        {
            img = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(img);
            Core.g.Clear(Color.Black);

            DrawMap();

            Render.Image = img;
        }

        private void DrawMap()
        {
            map.Draw();
            if (!start.IsEmpty)
                Core.g.DrawLine(new Pen(Color.Gray, 2F), start, LastMouseEventArgs.Location);
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Core.MouseHolding)
                start = e.Location;
            Core.MouseHolding = true;
            Render_MouseMove(null, e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            if (Core.MouseHolding)
            {
                var end = e.Location;
                AddMur(start.vecf(), end.vecf());
                start = Point.Empty;
            }
            Core.MouseHolding = false;
        }

        private void AddMur(vecf a, vecf b)
        {
            float d = Maths.Distance(a, b);
            if (d <= 50F)
            {
                map.murs.Add(new Mur(a, b));
                return;
            }

            for(int i=0; i<(int)(d / 50); i++)
            {
                var v = b - a;
                var l = v.Normalized();
                var q = l * 50F;
                if(i == 0)
                    map.murs.Add(new Mur(a + q * i, a + q * (i+1) * 1.1F));
                else if(i == (int)(d / 50) - 1)
                    map.murs.Add(new Mur(a + q * i * 0.9F, b));
                else
                    map.murs.Add(new Mur(a + q * i * 0.9F, a + q * (i+1) * 1.1F));
            }
        }

        private void Render_MouseLeave(object sender, EventArgs e)
        {
            Core.MouseHolding = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            LastMouseEventArgs = e;
            Core.MouseLocation = e.Location;

            if (Core.MouseHolding)
            {
            }
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
        }
    }
}
