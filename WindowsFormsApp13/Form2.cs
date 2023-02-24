using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsFormsApp13.utilities;

namespace WindowsFormsApp13
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void _Update()
        {
            if (Keyboard.IsKeyDown(Key.Z))
                Core.CamHide.MoveForward();
            if (Keyboard.IsKeyDown(Key.S))
                Core.CamHide.MoveBackward();
            if (Keyboard.IsKeyDown(Key.Q))
                Core.CamHide.TurnLeft();
            if (Keyboard.IsKeyDown(Key.D))
                Core.CamHide.TurnRight();

            if (Keyboard.IsKeyDown(Key.X))
                Core.CamHide.GoSuperHidden();

            Core.CamHide.Update();
        }

        public void _Draw()
        {
            Bitmap img = new Bitmap(Core.Map.Image);
            using (Graphics g = Graphics.FromImage(img))
            {
                var form1img = new Bitmap(Core.Form1Image);
                form1img.MakeTransparent(Color.Black);
                g.DrawImage(form1img, 0, 0);

                Core.CamHide.Draw(g, Color.Red);
                if(Core.CamHide.found)
                    Core.CamHide.Draw(Core.g, Color.Red);

                DrawUI(g);
            }
            Render.Image = img;
        }

        private void DrawUI(Graphics g)
        {
            int w = (int)g.VisibleClipBounds.Width;
            int h = (int)g.VisibleClipBounds.Height;
            Bitmap img = new Bitmap(w, h);
            using (Graphics gui = Graphics.FromImage(img))
            {
                int y = 0;
                UIHelp.DrawSuper(gui, w, Core.CamHide.SuperHidden, Color.Lime, y++);
            }
            g.DrawImage(img, 0, 0);
        }
    }
}
