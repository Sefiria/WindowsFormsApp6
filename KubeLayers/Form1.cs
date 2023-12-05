using System.Windows.Forms;
using System.Drawing;
using Tooling;

namespace KubeLayers
{
    public partial class Form1 : Form
    {
        private GCore core => GCore.Instance;

        public Form1()
        {
            InitializeComponent();

            core.Init(Render, Update, Draw);
        }

        public void Update()
        {
            core.Map.Update();
            core.Cam.Update();
        }
        public void Draw()
        {
            core.g.Clear(Color.FromArgb(10, 10, 15));
            core.gHUDUp.Clear(Color.Black);
            core.gHUDDown.Clear(Color.Black);

            core.Map.Draw();
            core.Cam.Draw();

            int yhudup = 0, yimg = core.ImageHUDUp.Height;
            int yhuddown = yimg + core.Image.Height;
            Bitmap img = new Bitmap(core.iRW, core.ImageHUDUp.Height + core.Image.Height + core.ImageHUDDown.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(core.ImageHUDUp, 0, yhudup);
                g.DrawImage(core.Image, 0, yimg);
                g.DrawImage(core.ImageHUDDown, 0, yhuddown);
            }
            Render.Image = img;
        }
    }
}
