using Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFormsApp21vox
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image;
        Graphics g;
        VoxelObject? vox = null;
        Font font = new Font("Segoe UI", 10F);
        Vector3 cameraPosition = new Vector3(0, 0, 1);
        Vector2 look = new Vector2(0, 0);
        int W, H, CW, CH;

        public Form1()
        {
            InitializeComponent();

            W = Render.Width;
            H = Render.Height;
            CW = W / 2;
            CH = H / 2;

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += DrawRender;

            vox = new Reader().ReadFile("C:\\Users\\dominguezs\\OneDrive - ZF Friedrichshafen AG\\Desktop\\saw\\mv\\vox\\test.vox");
        }

        private void DrawRender(object sender, EventArgs e)
        {
            Image = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(Image);
            g.Clear(Color.Black);

            Draw();

            Render.Image = Image;
        }

        private void Update(object sender, EventArgs e)
        {
            float amount = Keyboard.IsKeyDown(Key.LeftCtrl) ? 20F : 0.1F;

            if (Keyboard.IsKeyDown(Key.Z)) cameraPosition.Y -= amount;
            if (Keyboard.IsKeyDown(Key.S)) cameraPosition.Y += amount;
            if (Keyboard.IsKeyDown(Key.Q)) cameraPosition.X -= amount;
            if (Keyboard.IsKeyDown(Key.D)) cameraPosition.X += amount;
            if (Keyboard.IsKeyDown(Key.F)) cameraPosition.Z -= amount;
            if (Keyboard.IsKeyDown(Key.R)) cameraPosition.Z += amount;
            if (Keyboard.IsKeyDown(Key.Up)) look.Y -= amount;
            if (Keyboard.IsKeyDown(Key.Down)) look.Y += amount;
            if (Keyboard.IsKeyDown(Key.Left)) look.X -= amount;
            if (Keyboard.IsKeyDown(Key.Right)) look.X += amount;
        }

        private void Draw()
        {
            if(vox != null)
            {
                float cx = cameraPosition.X;
                float cy = cameraPosition.Y;
                float cz = cameraPosition.Z;
                foreach(var model in vox.Value.Models)
                {
                    //int sx = (int)model.Size.X;
                    //int sy = (int)model.Size.Y;
                    //int sz = (int)model.Size.Z;
                    foreach (var voxel in model.Voxels)
                    {
                        Color color = Color.FromArgb((int)vox.Value.ColorPalette[voxel.ColorIndex]);
                        using (Pen pen = new Pen(color))
                        using (Brush brush = new SolidBrush(color))
                        {
                            g.FillRectangle(brush, CW + (voxel.X - cx) * cz, CH - (voxel.Z + cy) * cz, cz, cz);
                        }
                    }
                }
            }



            #region infos
            int y = 10;
            g.DrawString($"X:{cameraPosition.X}", font, Brushes.White, 10, y); y += 20;
            g.DrawString($"Y:{cameraPosition.Y}", font, Brushes.White, 10, y); y += 20;
            g.DrawString($"Z:{cameraPosition.Z}", font, Brushes.White, 10, y); y += 20;
            #endregion
        }
    }
}
