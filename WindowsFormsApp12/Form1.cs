using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form1 : Form
    {
        Bitmap Image;
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };

        public Form1()
        {
            InitializeComponent();

            Core.W = Render.Width;
            Core.H = Render.Height;
            Core.X = Core.W / 2;
            Core.Y = Core.H / 2;
            Image = new Bitmap(Render.Width, Render.Height);
            Core.g = Graphics.FromImage(Image);
            Core.GlobalTime = new Stopwatch();
            Core.MatchTime = new Stopwatch();
            Core.GlobalTime.Start();
            Core.Dialog = new RPDialog(Render.Bounds);
            //temp
            Core.Dialog.SetConversation(new RPDIalogConv(new List<RPDIalogMsg>()
            {
                new RPDIalogMsg(null, "toto", RPDIalogMsg.Responses.OK),
            }));
            //endtemp

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            ManageMouse.Restart();
        }

        private void Update(object sender, EventArgs e)
        {
            Core.FormPosition = new Point(Location.X + Render.Left + 9, Location.Y + Render.Top + 31);

            if (ManageMouse.FirstMouseLocSet)
            {
                ManageMouse.SetMouseCur();
                ManageMouse.FirstMouseLocSet = false;
            }

            if (Data.Ended)
            {
                Data.EndProcessUpdate();
            }
            else
            {
                ManageKeyboard.Update();
                ManageMouse.Update();
                ManageEntities.Update();
                Core.Dialog.Update();
            }
        }

        private void Draw(object sender, EventArgs e)
        {
            Image = new Bitmap(Render.Width, Render.Height);
            Core.g = Graphics.FromImage(Image);

            if (Core.RecordBot != null)
                Core.g.DrawRectangle(new Pen(Color.Red, 4F), 0, 0, Render.Width - 5, Render.Height - 5);

            if (Data.Ended)
            {
                Data.EndProcessDraw();
            }
            else
            {
                ManageMouse.Draw();
                ManageEntities.Draw();
                Core.Dialog.Draw(Core.g);
            }

            ManageMouse.DrawUI();

            Render.Image = Image;
        }





        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (Data.Ended) return;

            ManageMouse.MouseDown(e);
        }

        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            if (Data.Ended) return;

            ManageMouse.MouseUp(e);
        }

        private void Render_MouseLeave(object sender, EventArgs e)
        {
            if (Data.Ended) return;

            ManageMouse.MouseLeave(e);
            Core.FormFocused = false;
        }

        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            if (Data.Ended) return;

            ManageMouse.MouseMove(e);
        }

        private void Render_MouseEnter(object sender, EventArgs e)
        {
            if (Data.Ended) return;

            Core.FormFocused = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            ManageKeyboard.KeyDown(e.KeyCode);
            Core.Dialog.KeyDown(e.KeyCode);
            KeysReleased.Dict[e.KeyCode] = false;
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            ManageKeyboard.KeyUp(e.KeyCode);
            KeysReleased.Dict[e.KeyCode] = true;
        }
    }
}
