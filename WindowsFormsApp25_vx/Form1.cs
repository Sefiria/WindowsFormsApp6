using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using Voxels;

namespace WindowsFormsApp25_vx
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        public Form1(string[] args)
        {
            InitializeComponent();

            KB.Init();
            MouseStates.Initialize();

            Core.Palette = new Palette(new MagicaVoxel().Palette.DefaultIfEmpty().ToList(), true);
            colorGrid1.Colors.Clear();
            colorGrid1.Colors.AddRange(Core.Palette.ToConformArray().Select(c => System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B)));
            colorGrid1.ColorIndex = Core.SelectedIndex;
            colorGrid1.ColorIndexChanged += (_, e) => Core.SelectedIndex = colorGrid1.ColorIndex;

            if (args.Length > 0 && Path.GetExtension(args[0]) == ".vox")
            {
                LoadVox(args[0]);
            }
            else
            {
                int sx = 12, sy = 12, sz = 8;
                Core.Voxels = new VoxelDataBytes(new XYZ(sx, sy, sz), Core.Palette.ToConformArray());
                for (int x = 0; x < sx; x++)
                {
                    for (int y = 0; y < sy; y++)
                    {
                        Core.Voxels[new XYZ(x, y, 0)] = new Voxel(1);
                    }
                    for (int z = 0; z < sz - 1; z++)
                    {
                        Core.Voxels[new XYZ(x, 0, z)] = new Voxel(1);
                        Core.Voxels[new XYZ(x, sy - 1, z)] = new Voxel(1);
                    }
                }
                for (int y = 0; y < sy; y++)
                {
                    for (int z = 0; z < sz - 1; z++)
                    {
                        Core.Voxels[new XYZ(0, y, z)] = new Voxel(1);
                        Core.Voxels[new XYZ(sx - 1, y, z)] = new Voxel(1);
                    }
                }
            }

            Core.Renderer = new Renderer(512, Core.Palette);

            Core.ms_prev_loc = pictureBox1.PointToClient(MousePosition);

            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseUp += PictureBox1_MouseUp;

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            if (Core.pause) return;

            if (KB.IsKeyDown(KB.Key.LeftCtrl) && KB.IsKeyPressed(KB.Key.S)) SaveVox();
            if (KB.IsKeyPressed(KB.Key.Substract) && Core.SelectedIndex > 1) Core.SelectedIndex--;
            if (KB.IsKeyPressed(KB.Key.Add) && Core.SelectedIndex < 255) Core.SelectedIndex++;

            //if(Core.Renderer.Bitmap == null || old_Core.yaw != Core.yaw || old_Core.pitch != Core.pitch)
            Core.Renderer.RenderVoxels(Core.Voxels, Core.yaw, Core.pitch);
            if (Core.mode == 0)
            {
                XYZ cur = new XYZ(Core.msXYZ.X, Core.msXYZ.Y, Core.msXYZ.Z);
                while (cur.Z < Core.Voxels.Size.Z - 1 && Core.Voxels[cur].Index > 0)
                    cur.Z++;
                Core.Renderer.RenderCursor3D(Core.Voxels, cur);
            }
            else if (Core.mode == 1)
            {
                Core.Renderer.RenderCursor3D(Core.Voxels, Core.msXYZ);
            }

            Core.Renderer.Canvas.DrawText($"yaw: {Core.yaw}    pitch: {Core.pitch}", 10, 20, new SKPaint());
            bool _x = Core.msXYZ.X >= 0 && Core.msXYZ.X < Core.Voxels.Size.X;
            bool _y = Core.msXYZ.Y >= 0 && Core.msXYZ.Y < Core.Voxels.Size.Y;
            bool _z = Core.msXYZ.Z >= 0 && Core.msXYZ.Z < Core.Voxels.Size.Z;
            var defaultPaint = new SKPaint();
            Core.Renderer.Canvas.DrawText($"Cursor:", 10, 50, defaultPaint);
            Core.Renderer.Canvas.DrawText(Core.msXYZ.X.ToString(), 60, 50, new SKPaint { Color = _x ? SKColors.Black : SKColors.Red });
            Core.Renderer.Canvas.DrawText(Core.msXYZ.Y.ToString(), 80, 50, new SKPaint { Color = _y ? SKColors.Black : SKColors.Red });
            Core.Renderer.Canvas.DrawText(Core.msXYZ.Z.ToString(), 100, 50, new SKPaint { Color = _z ? SKColors.Black : SKColors.Red });
            Core.Renderer.Canvas.DrawText($"Palette Selected Index : {Core.SelectedIndex}", 10, 80, defaultPaint);

            KB.Update();
            MouseStates.Update();
        }

        private void Draw(object sender, EventArgs e)
        {
            pictureBox1.Image = Core.Renderer.Bitmap.ToBitmap();
        }



        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = false;

            Core.ms_down = false;
            if (Core.mode == 0)
                Core.msXYZ.Z = 0;
        }
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = true;

            Core.ms_down = true;

            if (Core.mode == 0)
            {
                var temp = Core.Renderer.GetCursorVox(Core.mode, Core.Voxels, e.Location);
                Core.msXYZ = new XYZ(temp.X, temp.Y, Core.msXYZ.Z);
                if (!Core.Voxels.BoundsContains(Core.msXYZ))
                    return;
                while (Core.msXYZ.Z < Core.Voxels.Size.Z - 1 && Core.Voxels[Core.msXYZ].Index > 0)
                    Core.msXYZ.Z++;
            }

            PictureBox1_MouseMove(sender, e);
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Movements(e.Location);
            Core.ms_prev_loc = e.Location;

            if (KB.LeftAlt)
                return;

            if (Core.mode == 0)
            {
                var temp = Core.Renderer.GetCursorVox(Core.mode, Core.Voxels, e.Location);
                Core.msXYZ = new XYZ(temp.X, temp.Y, Core.msXYZ.Z);
            }
            else if (Core.mode == 1)
            {
                Core.msXYZ = Core.Renderer.GetCursorVox(Core.mode, Core.Voxels, e.Location);
            }

            if (!Core.ms_down) return;
            if (!Core.Voxels.BoundsContains(Core.msXYZ))
                return;
            if (e.Button == MouseButtons.Left)
            {
                if (Core.Voxels[Core.msXYZ].Index == 0)
                    Core.Voxels[Core.msXYZ] = new Voxel((uint)Core.SelectedIndex);
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Core.mode == 0)
                {
                    var xyz = new XYZ(Core.msXYZ.X, Core.msXYZ.Y, Core.msXYZ.Z == 0 ? 0 : Core.msXYZ.Z - 1);
                    if (Core.Voxels[xyz].Index > 0)
                        Core.Voxels[xyz] = new Voxel(0);
                }
                else if (Core.mode == 1)
                {
                    if (Core.Voxels[Core.msXYZ].Index > 0)
                        Core.Voxels[Core.msXYZ] = new Voxel(0);
                }
            }
        }

        private void Movements(Point ms)
        {
            var amount = ms.MinusF(Core.ms_prev_loc).x(Core.spd);

            if (KB.LeftAlt)
            {
                if (MouseStates.ButtonsDown[MouseButtons.Left])
                {
                    Core.Cam = new XYZf(Core.Cam.X + amount.X, Core.Cam.Y + amount.Y, Core.Cam.Z);
                }
                else if (MouseStates.ButtonsDown[MouseButtons.Middle])
                {
                    Core.yaw += amount.X * Core.spd;
                    Core.pitch -= amount.Y * Core.spd * 0.1F;
                }
            }
        }

        private void SaveVox()
        {
            if (Core.Voxels != null)
            {
                Core.pause = true;
                using (var stream = File.Create("test_output.vox"))
                {
                    new MagicaVoxel((VoxelDataBytes)Core.Voxels).Write(stream);
                }
                MessageBox.Show(this, "Saved successfully.", "Save to .vox (MagicaVoxel)", MessageBoxButtons.OK);
                Core.pause = false;
            }
        }
        private void LoadVox(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                var magicaVoxel = new MagicaVoxel();
                magicaVoxel.Read(stream);
                Core.Voxels = magicaVoxel.Flatten();
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
        }
    }
}
