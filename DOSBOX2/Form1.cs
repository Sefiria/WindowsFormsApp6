using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Tooling;

namespace DOSBOX2
{
    public partial class DOSBOX2 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        int marge = 4;
        int buttonSz = 12;
        Rectangle rectButtonMinimize => new Rectangle(marge, marge, buttonSz, buttonSz);
        Rectangle rectButtonExit => new Rectangle(Width - marge - buttonSz, marge, buttonSz, buttonSz);
        Pen penButtons = new Pen(Color.Black, 2F);
        int State = 0, launch_timer = 0, launch_timer_max = 50;


        public DOSBOX2()
        {
            InitializeComponent();

            KB.Init();
            MouseStates.Initialize(Render);
            Graphic.Initialize(new Graphic.Config() { palette = new Palette(new[] {
                (35, 48, 64).ToArgb(),
                (88, 139, 112).ToArgb(),
                (175, 207, 151).ToArgb(),
                (219, 229, 219).ToArgb() })
            });

            Graphic.Clear(3);

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
                Close();

            if (KB.IsKeyPressed(KB.Key.Enter))
            {
                if (State == 0)
                    State++;
                else
                    State = 0;
            }
            if (State == 1)
            {
                if (launch_timer < launch_timer_max)
                    launch_timer++;
                else
                {
                    State++;
                    launch_timer = 0;
                }
            }
            if (MouseStates.ButtonsDown[MouseButtons.Left])
            {
                var ms = MouseStates.Position.ToPoint();
                if (rectButtonMinimize.Contains(ms))
                {
                    WindowState = FormWindowState.Minimized;
                    return;
                }
                if (rectButtonExit.Contains(ms))
                {
                    Close();
                    return;
                }
            }

            if(State == 2)
                Core.Update();

            KB.Update();
            MouseStates.Update();
        }

        private void Draw(object _, EventArgs e)
        {
            Bitmap Image = new Bitmap(520, 520);
            using (Graphics g = Graphics.FromImage(Image))
            {
                g.Clear(Color.FromArgb(100, 100, 100));
                if (State < 2)
                {
                    if (State == 0)
                        g.FillRectangle(new SolidBrush(Color.FromArgb(20, 15,30)), 20, 20, 480, 480);
                    else if(State == 1)
                        g.FillRectangle(new SolidBrush(Graphic.config.palette[0]), 20, 20, 480, 480);
                }
                else
                {
                    Graphic.Draw();
                    g.DrawImage(Graphic.GetBitmap(), (20, 20).P());
                }
                DrawHeader(g);
            }
            Render.Image = Image;
        }
        private void DrawHeader(Graphics g)
        {
            var rect = rectButtonMinimize;
            g.DrawRectangle(penButtons, rectButtonMinimize);
            g.DrawLine(penButtons, rect.X + rect.Width / 2 - buttonSz / 3, rect.Y + rect.Height / 2, rect.X + rect.Width / 2 + buttonSz / 3, rect.Y + rect.Height / 2);

            rect = rectButtonExit;
            g.DrawRectangle(penButtons, rectButtonExit);
            g.DrawLine(penButtons, rect.X + rect.Width / 2 - buttonSz / 3, rect.Y + rect.Height / 2 - buttonSz / 3, rect.X + rect.Width / 2 + buttonSz / 3, rect.Y + rect.Height / 2 + buttonSz / 3);
            g.DrawLine(penButtons, rect.X + rect.Width / 2 - buttonSz / 3, rect.Y + rect.Height / 2 + buttonSz / 3, rect.X + rect.Width / 2 + buttonSz / 3, rect.Y + rect.Height / 2 - buttonSz / 3);

            int led_size = 8;
            g.FillEllipse(State == 0 ? Brushes.Red : Brushes.Lime, Width / 2 - led_size / 2, 10 - led_size / 2, led_size, led_size);
            g.DrawEllipse(Pens.Black, Width / 2 - led_size / 2, 10 - led_size / 2, led_size, led_size);
        }

        protected override void OnClosed(EventArgs e)
        {
            TimerUpdate.Tick -= Update;
            TimerDraw.Tick -= Draw;
            System.Threading.Thread.Sleep(200);
            base.OnClosed(e);
        }
    }
}
