using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp23
{
    public partial class Form1 : Form
    {
        Bitmap render_img, img;
        Timer TimerUpdate = new Timer();
        Timer TimerDraw = new Timer();

        public Form1()
        {
            InitializeComponent();

            Core.RenderW = Render.Width;
            Core.RenderH = Render.Height;
            reset_img();
            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        void reset_img()
        {
            render_img = new Bitmap(Core.RenderW, Core.RenderH);
            Core.render_g = Graphics.FromImage(render_img);

            img = new Bitmap(Core.SZ, Core.SZ);
            Core.g = Graphics.FromImage(img);
        }

        void Update(object sender, EventArgs e)
        {
        }

        void Draw(object sender, EventArgs e)
        {
            reset_img();
            DrawUI();
            Core.render_g.DrawImage(img, Core.RenderW - Core.SZ * 1.5F, Core.RenderH - Core.SZ * 1.5F);
            Render.Image = render_img;
        }

        private void DrawUI()
        {
        }
    }
}
