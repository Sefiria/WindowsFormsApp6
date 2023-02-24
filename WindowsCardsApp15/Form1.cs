using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsCardsApp15.scenes;

namespace WindowsCardsApp15
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image;

        public Form1()
        {
            InitializeComponent();

            Core.W = Render.Width;
            Core.H = Render.Height;
            Image = new Bitmap(Core.W, Core.H);
            Core.g = Graphics.FromImage(Image);
            Core.Scene = new SceneMenu();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            Core.Scene.Update();

            if (Core.Quitter)
                Close();
        }

        private void Draw(object _, EventArgs e)
        {
            Image = new Bitmap(Core.W, Core.H);
            Core.g = Graphics.FromImage(Image);

            Core.Scene.Draw();
            
            Render.Image = Image;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            var clickedUI = Core.Scene.UI.Where(i => e.X >= i.X && e.X < i.X + i.W && e.Y >= i.Y && e.Y < i.Y + i.H).ToList();
            if (clickedUI.Count > 0)
                clickedUI.ForEach(i => i.Clicked());
        }
    }
}
