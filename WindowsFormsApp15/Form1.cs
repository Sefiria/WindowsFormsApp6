using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp15.utilities.tiles;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15
{
    public partial class Form1 : Form
    {
        Bitmap img;

        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        UserUI user = null;
        MouseEventArgs LastMouseEventArgs = null;

        public Form1()
        {
            InitializeComponent();

            Core.rw = Render.Width;
            Core.rh = Render.Height;
            Core.cam = new vecf(Core.rw / 2F, Core.rh / 2F);
            img = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(img);

            Data.Instance.Init();
            KB.Init();
            AnimRes.Init();
            Conveyor.Init();
            Core.Init();

            user = new UserUI();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            AnimRes.Tick();

            new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.Update());
            new List<Item>(Data.Instance.Items).ForEach(o => o.Update());

            if(LastMouseEventArgs != null)
                Render_MouseMove(null, LastMouseEventArgs);

            user.Update();

            KB.Update();
        }

        private void Draw(object _, EventArgs e)
        {
            img = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(img);
            Core.g.Clear(Color.Black);

            //back
            new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.Display());
            new List<Item>(Data.Instance.Items).ForEach(s => s.Display());
            //front
            new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.FrontDisplay());

            user.Display();

            Render.Image = img;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Core.MouseHolding = true;
            Render_MouseMove(null, e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            Core.MouseHolding = false;
            user.MouseInput(e, true);
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
                user.MouseInput(e);
            }
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            user.MouseWheel(e);
        }
    }
}
