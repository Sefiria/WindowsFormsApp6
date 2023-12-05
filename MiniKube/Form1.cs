using MiniKube.Items;
using MiniKube.Structures;
using MiniKube.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace MiniKube
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };
        UserSettings Settings = new UserSettings();

        public Form1()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            Core.RW = Render.Width;
            Core.RH = Render.Height;
            Core.Image = new Bitmap(Core.iRW, Core.iRH);
            Core.g = Graphics.FromImage(Core.Image);

            Core.Inventory = new Inventory();
            Core.Inventory.AddItem<Input<ItemIronPlate>>(1);
            Core.Inventory.AddItem<Input<ConveyorMk1>>(1);
            Core.Inventory.AddItem<ConveyorMk1>(18);
        }

        private void Update(object _, EventArgs e)
        {
            AnimMgr.Update();

            Core.MS = PointToClient(MousePosition);

            if (Keyboard.IsKeyDown(Key.Z))
                Core.Cam.Y -= Core.CamSpeed;
            if (Keyboard.IsKeyDown(Key.S))
                Core.Cam.Y += Core.CamSpeed;
            if (Keyboard.IsKeyDown(Key.Q))
                Core.Cam.X -= Core.CamSpeed;
            if (Keyboard.IsKeyDown(Key.D))
                Core.Cam.X += Core.CamSpeed;

            if (KB.IsKeyPressed(KB.Key.E))
                Core.Inventory.Toggle();

            if (Core.Inventory.IsOpen)
            {
            }
            else
            {
                Core.Map.Update();
            }

            KB.Update();
        }

        private void Draw(object _, EventArgs e)
        {
            Core.g.Clear(Settings.BackgroundColor);
            DrawUI(Core.g);
            Core.Map.Draw();
            if (Core.Inventory.IsOpen)
                Core.Inventory.Draw();

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
                var tex = new Bitmap(Core.Cube, Core.Cube);
                using (Graphics gtex = Graphics.FromImage(tex))
                    if (MouseStates.IsDown)
                    {
                        switch(MouseStates.ButtonDown)
                        {
                            case MouseButtons.Left: gtex.Clear(Color.FromArgb(150, Color.White)); break;
                            case MouseButtons.Right: gtex.Clear(Color.FromArgb(50, Color.Blue)); break;
                            case MouseButtons.Middle: gtex.Clear(Color.FromArgb(50, Color.Yellow)); break;
                        }
                    }
                    else
                    {
                        gtex.Clear(Color.FromArgb(50, Color.Black));
                    }
                gui.DrawImage(tex, Core.TargetPoint);
            }
            g.DrawImage(img, 0, 0);
        }

        private void Render_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseStates.ButtonDown = e.Button;
        }
        private void Render_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseStates.ButtonDown = MouseButtons.None;
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            MouseStates.ButtonDown = MouseButtons.None;
        }
    }
}
