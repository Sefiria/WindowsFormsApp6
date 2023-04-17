using AssetMake.Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AssetMake
{
    public partial class FormTemplate : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        MouseEventArgs LastMouseEventArgs = null;
        int AnimationSpeed = 0, Ticks = 0;
        int TickMax => AnimationSpeed == 0 ? 10 : (AnimationSpeed == 1 ? 50 : (AnimationSpeed == 2 ? 100 : 500));
        int AnimFrame = 0, AnimFrameMax = 100;

        public FormTemplate()
        {
            InitializeComponent();

            Core.rw = Render.Width;
            Core.rh = Render.Height;
            Core.Image = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(Core.Image);

            Data.Instance.Init();
            KB.Init();
            Core.Init();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            if(LastMouseEventArgs != null)
                Render_MouseMove(null, LastMouseEventArgs);

            Tick();

            KB.Update();
        }

        private void Tick()
        {
            if (Ticks >= TickMax)
            {
                Ticks = 0;
                if (AnimFrame >= AnimFrameMax)
                {
                    AnimFrame = 0;
                }
                else
                    AnimFrame++;
            }
            else
                Ticks++;
        }

        private void Draw(object _, EventArgs e)
        {
            Core.Image = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(Core.Image);
            Core.g.Clear(Color.Black);



            Render.Image = Core.Image;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Core.MouseHolding = true;
            Render_MouseMove(null, e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            Core.MouseHolding = false;
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
