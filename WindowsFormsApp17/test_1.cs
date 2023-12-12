using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tooling;
using WindowsFormsApp17.items;

namespace WindowsFormsApp17
{
    public partial class test_1 : Form
    {
        Bitmap img;

        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        UserUI user = null;
        MouseEventArgs LastMouseEventArgs = null;
        int ticks, tickmax = 10;
        bool canuse_addfluid;

        public test_1()
        {
            InitializeComponent();

            Core.rw = Render.Width;
            Core.rh = Render.Height;
            img = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(img);

            ResMgr.Init();

            Data.Instance.Init();
            KB.Init();
            Core.Init();

            ticks = 0;
            canuse_addfluid = true;

            user = new UserUI();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            if (LastMouseEventArgs != null)
                Render_MouseMove(null, LastMouseEventArgs);

            if (ticks >= tickmax)
            {
                ticks = 0;
                canuse_addfluid = true;
            }
            else ticks++;

            Data.Instance.fluidmgr.Update();

            user.Update();

            KB.Update();
        }

        private void Draw(object _, EventArgs e)
        {
            img = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(img);
            Core.g.Clear(Color.Black);

            draw_murs();
            draw_fluids();

            user.Display();

            Render.Image = img;
        }

        private void draw_murs()
        {
            var murs = new List<mur>(Data.Instance.murs);
            foreach (var mur in murs)
                Core.g.FillRectangle(Brushes.White, mur.rect);
        }
        private void draw_fluids()
        {
            var fluids = new List<fluid>(Data.Instance.fluidmgr.fluids);
            foreach (var fluid in fluids)
                fluid.Display();
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
            if (Core.MouseHolding && canuse_addfluid)
            {
                Data.Instance.fluidmgr.AddFluid(e.X-8, e.Y-8);
                Data.Instance.fluidmgr.AddFluid(e.X+8, e.Y);
                Data.Instance.fluidmgr.AddFluid(e.X, e.Y+8);
                canuse_addfluid = false;
            }
            else
                ticks++;
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            user.MouseWheel(e);
        }
    }
}
