using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Windows.Forms;
using WindowsFormsApp18.Utilities;

namespace WindowsFormsApp18
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        System.Windows.Forms.MouseEventArgs LastMouseEventArgs = null;

        public Form1()
        {
            InitializeComponent();

            Core.rw = Render.Width;
            Core.rh = Render.Height;
            Core.Image = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(Core.Image);

            ResMgr.Init();

            Data.Instance.Init();
            KB.Init();
            Core.Init();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            //new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.Update());
            //new List<Item>(Data.Instance.Items).ForEach(o => o.Update());

            if(LastMouseEventArgs != null)
                Render_MouseMove(null, LastMouseEventArgs);
            Data.Instance.cars.ForEach(car => car.Update());

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl) && KB.IsKeyPressed(KB.Key.V))
                PasteData();

            KB.Update();
        }
        private void PasteData()
        {
            JsonData data = null;
            try
            {
                data = JsonSerializer.Deserialize<JsonData>(Clipboard.GetText());
            }
            catch (Exception e) { }
            if(data != null)
                Data.Instance.map = data.map;
        }

        private void Draw(object _, EventArgs e)
        {
            Core.Image = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(Core.Image);
            Core.g.Clear(Color.Black);

            ////back
            //new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.Display());
            //new List<Item>(Data.Instance.Items).ForEach(s => s.Display());
            ////front
            //new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.FrontDisplay());

            DrawMap();

            Data.Instance.cars.ForEach(car => car.Display());

            Render.Image = Core.Image;
        }

        private void DrawMap()
        {
            Data.Instance.map.Draw();
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Core.MouseHolding = true;
            Render_MouseMove(null, e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            Core.MouseHolding = false;
            Data.Instance.cars.ForEach(car => car.MouseInput(e, true));
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
                Data.Instance.cars.ForEach(car => car.MouseInput(e));
            }
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            Data.Instance.cars.ForEach(car => car.MouseWheel(e));
        }
    }
}
