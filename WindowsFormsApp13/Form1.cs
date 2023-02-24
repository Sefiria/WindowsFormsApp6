using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsFormsApp13.utilities;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace WindowsFormsApp13
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image;

        public Form1()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            Core.RW = Render.Width;
            Core.RH = Render.Height;
            Image = new Bitmap(Core.iRW, Core.iRH);
            Core.g = Graphics.FromImage(Image);
            Core.FormHide = new Form2();
            Core.FormHide.Show();

            Core.Initialize();
        }

        private void Update(object _, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Escape))
                Core.Initialize();

            if (Keyboard.IsKeyDown(Key.Up))
                Core.Cam.MoveForward();
            if (Keyboard.IsKeyDown(Key.Down))
                Core.Cam.MoveBackward();
            if (Keyboard.IsKeyDown(Key.Left))
                Core.Cam.TurnLeft();
            if (Keyboard.IsKeyDown(Key.Right))
                Core.Cam.TurnRight();

            if (Keyboard.IsKeyDown(Key.NumPad0))
                Core.Cam.GoSuperBack();

            Core.Cam.Update();

            Core.FormHide._Update();
        }

        private void Draw(object _, EventArgs e)
        {
            Displayer.Display(Core.g, Core.Map, Core.Cam);
            if(Core.Cam.IsSuperBack())
                Displayer.Display(Core.g, Core.Map, Core.Cam, _look:Core.Cam.look.Rotate(180F));
            Core.Form1Image = Image;
            DrawUI(Core.g);
            Render.Image = Image;

            Core.FormHide._Draw();

            Core.Ticks++;
        }

        private void DrawUI(Graphics g)
        {
            int w = (int)g.VisibleClipBounds.Width;
            int h = (int)g.VisibleClipBounds.Height;
            Bitmap img = new Bitmap(w, h);
            using (Graphics gui = Graphics.FromImage(img))
            {
                int y = 0;
                UIHelp.DrawSuper(gui, w, Core.Cam.SuperBack, Color.Lime, y++);
            }
            g.DrawImage(img, 0, 0);
        }
    }
}
