using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFormsApp20
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image;
        Graphics g;
        Matrix4x4 projectionMatrix;
        float fieldOfView = 1.0472F, aspectRatio = 1F, nearPlaneDistance = 0.1F, farPlaneDistance = 1000F;
        Font font = new Font("Segoe UI", 10F);
        Vector3 cameraPosition = new Vector3(0, 0, -10);
        Vector3 cameraTarget = new Vector3(0, 0, 0);


        public Form1()
        {
            InitializeComponent();

            aspectRatio = (float)ClientSize.Width / ClientSize.Height;
            cameraPosition.X = Width / 2;
            cameraPosition.Y = Height / 2;
            cameraTarget.X = Width / 2;
            cameraTarget.Y = Height / 2;

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += DrawRender;
        }

        private void DrawRender(object sender, EventArgs e)
        {
            Image = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(Image);
            g.Clear(Color.Black);

            Draw();

            Render.Image = Image;
        }

        private void Draw()
        {
            float x = 10F;
            float y;
            for (y = -5F; y < 6F; y+=0.01F)
            {
                Draw3DLine(new Vector3(-x, 0, y * 5), new Vector3(x, 0, y * 5));
                Draw3DLine(new Vector3(y * 5, 0, -x), new Vector3(y * 5, 0, x));
            }


            #region infos
            y = 10;
            g.DrawString($"X:{cameraPosition.X}", font, Brushes.White, 10, y); y += 20;
            g.DrawString($"Y:{cameraPosition.Y}", font, Brushes.White, 10, y); y += 20;
            g.DrawString($"Z:{cameraPosition.Z}", font, Brushes.White, 10, y); y += 20;
            g.DrawString($"fov:{fieldOfView}", font, Brushes.White, 10, y); y += 20;
            g.DrawString($"aspectRatio:{aspectRatio}", font, Brushes.White, 10, y); y += 20;
            g.DrawString($"near:{nearPlaneDistance}", font, Brushes.White, 10, y); y += 20;
            g.DrawString($"far:{farPlaneDistance}", font, Brushes.White, 10, y); y += 20;
            #endregion
        }

        private void Update(object sender, EventArgs e)
        {
            float amount = 0.05F;
            if (Keyboard.IsKeyDown(Key.T)) fieldOfView += amount;
            if (Keyboard.IsKeyDown(Key.G)) fieldOfView -= amount;
            if (Keyboard.IsKeyDown(Key.Y)) aspectRatio += amount;
            if (Keyboard.IsKeyDown(Key.H)) aspectRatio -= amount;
            if (Keyboard.IsKeyDown(Key.U)) nearPlaneDistance += amount * 2;
            if (Keyboard.IsKeyDown(Key.J)) nearPlaneDistance -= amount * 2;
            if (Keyboard.IsKeyDown(Key.I)) farPlaneDistance += amount * 10;
            if (Keyboard.IsKeyDown(Key.K)) farPlaneDistance -= amount * 10;

            if (fieldOfView < amount) fieldOfView = amount;
            if (fieldOfView > 3F) fieldOfView = 3F;
            if (aspectRatio < 0F) aspectRatio = 0F;
            if (aspectRatio > 1F) aspectRatio = 1F;
            if (nearPlaneDistance < amount) nearPlaneDistance = amount;
            if (farPlaneDistance < nearPlaneDistance) farPlaneDistance = nearPlaneDistance;

            projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);

            amount = 0.01F;

            if (Keyboard.IsKeyDown(Key.Z)) cameraPosition.Y -= amount;
            if (Keyboard.IsKeyDown(Key.S)) cameraPosition.Y += amount;
            if (Keyboard.IsKeyDown(Key.Q)) cameraPosition.X -= amount;
            if (Keyboard.IsKeyDown(Key.D)) cameraPosition.X += amount;
            if (Keyboard.IsKeyDown(Key.Down)) cameraPosition.Z -= amount * (Keyboard.IsKeyDown(Key.LeftCtrl) ? 10F : 1F);
            if (Keyboard.IsKeyDown(Key.Up)) cameraPosition.Z += amount * (Keyboard.IsKeyDown(Key.LeftCtrl) ? 10F : 1F);
        }
        private void Draw3DLine(Vector3 vertex1, Vector3 vertex2)
        {
            // Define the camera position and orientation
            Vector3 cameraUp = new Vector3(0, 1, 0);

            // Define the view matrix
            Matrix4x4 viewMatrix = Matrix4x4.CreateLookAt(cameraPosition, cameraTarget, cameraUp);

            // Combine the projection and view matrices into a single transformation matrix
            Matrix4x4 transformationMatrix = projectionMatrix * viewMatrix;

            // Transform vertices into screen space
            Vector3 screenPos1 = transformationMatrix.MultiplyPoint(vertex1);
            Vector3 screenPos2 = transformationMatrix.MultiplyPoint(vertex2);

            // Project screen space vertices onto 2D surface
            Point projectedPos1 = new Point((int)screenPos1.X, (int)screenPos1.Y);
            Point projectedPos2 = new Point((int)screenPos2.X, (int)screenPos2.Y);

            int max = 10;
            if (projectedPos1.X < -int.MaxValue / max) projectedPos1.X = -int.MaxValue / max;
            if (projectedPos1.X > int.MaxValue / max) projectedPos1.X = int.MaxValue / max;
            if (projectedPos1.Y < -int.MaxValue / max) projectedPos1.Y = -int.MaxValue / max;
            if (projectedPos1.Y > int.MaxValue / max) projectedPos1.Y = int.MaxValue / max;
            if (projectedPos2.X < -int.MaxValue / max) projectedPos2.X = -int.MaxValue / max;
            if (projectedPos2.X > int.MaxValue / max) projectedPos2.X = int.MaxValue / max;
            if (projectedPos2.Y < -int.MaxValue / max) projectedPos2.Y = -int.MaxValue / max;
            if (projectedPos2.Y > int.MaxValue / max) projectedPos2.Y = int.MaxValue / max;

            // Draw line between projected vertices
            g.DrawLine(Pens.White, projectedPos1, projectedPos2);
        }


    }
}
