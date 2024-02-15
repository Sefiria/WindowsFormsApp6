using System;
using System.Drawing;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp17
{
    public partial class FormCatch : Form
    {
        Bitmap render_img, img;
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        public FormCatch()
        {
            Cursor.Hide();
            InitializeComponent();

            Core.rw = Render.Width;
            Core.rh = Render.Height;
            Core.res = 0.7F;
            reset_img();
            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            Data.Instance.Init(true);
            KB.Init();
            Core.Init();
        }

        void reset_img()
        {
            render_img = new Bitmap(Core.rw, Core.rh);
            Core.render_g = Graphics.FromImage(render_img);
            Core.render_g.Clear(Color.Black);

            img = new Bitmap(Core.w, Core.h);
            Core.g = Graphics.FromImage(img);
            Core.g.Clear(Color.Black);
        }

        void Update(object sender, EventArgs e)
        {
            MouseStatesV1.Position = Render.PointToClient(MousePosition).MinusF((Core.mw - Core.TSZ * 1F, (float)Core.mh).P());

            Data.Instance.map.FluidUpdate();

            Data.Instance.UpdateRun();

            KB.Update();
        }

        void Draw(object sender, EventArgs e)
        {
            reset_img();
            Data.Instance.DrawRun();
            DrawUI();
            Core.render_g.DrawImage(img, Core.mw, Core.mh);
            Render.Image = render_img;
        }

        private void DrawUI()
        {
        }
    }
}
