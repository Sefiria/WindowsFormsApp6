using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFormsApp14
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

            Core.Initialize();
        }

        private void Update(object _, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.A))
                Core.Cam.StrafeLeft();
            if (Keyboard.IsKeyDown(Key.E))
                Core.Cam.StrafeRight();
            if (Keyboard.IsKeyDown(Key.Z))
                Core.Cam.MoveForward();
            if (Keyboard.IsKeyDown(Key.S))
                Core.Cam.MoveBackward();
            if (Keyboard.IsKeyDown(Key.Q))
                Core.Cam.angle += 4F;
            if (Keyboard.IsKeyDown(Key.D))
                Core.Cam.angle -= 4F;

            Core.Cam.Update();
        }

        private void Draw(object _, EventArgs e)
        {
            Displayer.Display(Core.g, Core.Map, Core.Cam);
            Render.Image = Image;
        }
    }
}
