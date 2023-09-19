using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFormsApp21
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        static RawInputDevice[] Devices = RawInputDevice.GetDevices();
        static List<RawInputKeyboard> Keyboards = Devices.OfType<RawInputKeyboard>().ToList();

        public Form1()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            Core.RW = Render.Width;
            Core.RH = Render.Height;
            Core.Image = new Bitmap(Core.iRW, Core.iRH);
            Core.g = Graphics.FromImage(Core.Image);
        }

        private void Update(object _, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Escape))
                Core.Initialize();

            Core.Map.Update();

            //if (Keyboard.IsKeyDown(Key.Space))
            //{
            //    Console.WriteLine(Keyboards[0]);
            //}


            //if (Keyboard.IsKeyDown(Key.Up))
            //    Core.Cam.MoveForward();
            //if (Keyboard.IsKeyDown(Key.Down))
            //    Core.Cam.MoveBackward();
            //if (Keyboard.IsKeyDown(Key.Left))
            //    Core.Cam.TurnLeft();
            //if (Keyboard.IsKeyDown(Key.Right))
            //    Core.Cam.TurnRight();

            //if (Keyboard.IsKeyDown(Key.NumPad0))
            //    Core.Cam.GoSuperBack();

            //Core.Cam.Update();

            //Core.FormHide._Update();
        }

        private void Draw(object _, EventArgs e)
        {
            Core.g.Clear(Color.Black);
            Core.Map.Draw();
            //DrawUI(Core.g);
            
            Render.Image = Core.Image;

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
                //UIHelp.DrawSuper(gui, w, Core.Cam.SuperBack, Color.Lime, y++);
            }
            g.DrawImage(img, 0, 0);
        }
    }
}
