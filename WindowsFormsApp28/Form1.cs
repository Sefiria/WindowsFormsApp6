using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp28
{
    public partial class Form1 : Form
    {
        List<Color> Palette = new List<Color>{
                Color.FromArgb(219, 229, 219),
                Color.FromArgb(175, 207, 151),
                Color.FromArgb(88, 139, 112),
                Color.FromArgb(35, 48, 64),
                Color.FromArgb(100, 57, 0) };
        List<SolidBrush> PaletteBrushes;

        const int w = 256;
        const int h = 256;
        float z = 10F;
        float x=w / 2, y= h/ 2;
        float mvspd = 0.5F;
        byte[] Pixels = new byte[w * h];
        Fluid[] Fluids = new Fluid[w * h];
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image;
        byte px_pal_sel = 0;
        Font font;
        Size CharSize;
        double time = 0;
        bool busy = false;
        byte block_selection = 3;

        const int BRICK = 3;
        const int PIPE = 4;

        public Form1()
        {
            InitializeComponent();

            font = new Font("Courrier New", 12F, FontStyle.Regular);
            CharSize = TextRenderer.MeasureText("A", font);

            PaletteBrushes = new List<SolidBrush>();
            foreach (Color c in Palette)
                PaletteBrushes.Add(new SolidBrush(c));

            for(int x=0;x<w; x++)
                for(int y=0;y<h;y++)
                    Fluids[y*w+x] = new Fluid();

            KB.Init();
            MouseStates.Initialize(Render);

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            if (busy || !Focused) return;
            busy = true;
            var (z, q, s, d) = KB.ZQSD();
            var kc = KB.IsKeyPressed(KB.Key.C);
            var kx = KB.IsKeyPressed(KB.Key.X);
            var left_pressed = MouseStates.IsButtonPressed(MouseButtons.Left);
            var right_pressed = MouseStates.IsButtonPressed(MouseButtons.Right);
            bool pressed = left_pressed || right_pressed;
            var left_down = MouseStates.IsButtonDown(MouseButtons.Left) && Focused;
            var right_down = MouseStates.IsButtonDown(MouseButtons.Right) && Focused;
            var down = left_down || right_down;
            float adjustedSpeed = mvspd / this.z * 20F;

            if (z) y -= adjustedSpeed;
            if (q) x -= adjustedSpeed;
            if (s) y += adjustedSpeed;
            if (d) x += adjustedSpeed;

            if (KB.IsKeyDown(KB.Key.Num1))
                block_selection = BRICK;
            if (KB.IsKeyDown(KB.Key.Num2))
                block_selection = PIPE;

            if (kx) ResetPixels(KB.LeftShift);
            if (kc) ResetFluids();

            var ms = MouseStates.Position.ToPoint();
            var oldMs = MouseStates.OldPosition.ToPoint();

            if (pressed)
            {
                int worldX = (int)((ms.X - Width / 2) / this.z + x);
                int worldY = (int)((ms.Y - Height / 2) / this.z + y);
                if (worldX >= 0 && worldX < w && worldY >= 0 && worldY < h)
                {
                    int index = worldY * w + worldX;
                    px_pal_sel = Pixels[index] == 0 ? (byte)(left_pressed? block_selection : 1) : (byte)0;
                }
            }
            else if (down)
            {
                for (float t = 0; t <= 1; t += 0.01f)
                {
                    PointF interpolatedPoint = Maths.Lerp(oldMs, ms, t);
                    int worldX = (int)((interpolatedPoint.X - Width / 2) / this.z + x);
                    int worldY = (int)((interpolatedPoint.Y - Height / 2) / this.z + y);
                    if (worldX >= 0 && worldX < w && worldY >= 0 && worldY < h)
                    {
                        int index = worldY * w + worldX;
                        if (right_down) Fluids[index].Q = Math.Min(10F, Fluids[index].Q+0.01F);
                        else Pixels[index] = px_pal_sel;
                    }
                }
            }

            if (MouseStates.Delta != 0)
                this.z *= 1 + MouseStates.Delta / 1000F;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ManageFluids();
            time = stopwatch.Elapsed.TotalSeconds;

            KB.Update();
            MouseStates.Update();
            busy = false;
        }

        void ManageFluids()
        {
            Fluid[] new_fluids = Fluids.Select(fluid => new Fluid(fluid)).ToArray();
            CellInfo.Init(Pixels, Fluids, w, h, BRICK, PIPE);

            int x, y;
            int startX = Math.Max(0, (int)(this.x - Width / (2 * z)));
            int startY = Math.Max(0, (int)(this.y - Height / (2 * z)));
            int endX = Math.Min(w - 1, (int)(this.x + Width / z));
            int endY = Math.Min(h - 1, (int)(this.y + Height / z));
            CellInfo CurrentCell = null;

            bool px(int _x, int _y) => Pixels[_y * w + _x] < BRICK && get(_x, _y) < 0.2F;
            float get(int _x, int _y) => new_fluids[_y * w + _x].Q;
            void move(int ofst_x, int ofst_y, float _d)
            {
                float q = Math.Min(Fluids[y * w + x].Q, _d);
                Fluids[y * w + x].Q -= q;
                if (Fluids[y * w + x].Q <= 0F)
                {
                    Fluids[y * w + x].Q = 0F;
                    if(CurrentCell?.linked_pipes.Count == 0 || (CurrentCell?.linked_pipes.Count == 1 && CurrentCell?.linked_pipes.First() != CurrentCell.cell_fluid.From))
                        Fluids[y * w + x].From = vec.Null;
                }
                else if(CurrentCell?.linked_pipes.Count == 0) Fluids[y * w + x].From = vec.Null;
                    Fluids[y * w + x].From = vec.Null;
                Fluids[(y + ofst_y) * w + x + ofst_x].Q += q;
                Fluids[(y + ofst_y) * w + x + ofst_x].previous_look_x = Maths.Round(ofst_x, 3);
            }

            for (y = startY; y < endY; y++)
            {
                for (x = startX; x < endX; x++)
                {
                    CurrentCell = new CellInfo(x, y);
                    Fluid f = Fluids[y*w+x];

                    if (f.Q == 0F)
                        continue;

                    var CellLeft = new CellInfo(x-1, y);
                    var CellRight = new CellInfo(x+1, y);
                    var CellTop = new CellInfo(x, y-1);
                    var CellBottom = new CellInfo(x, y+1);
                    bool bottom = y < h - 1 && px(x, y + 1);
                    bool left = x > 0 && px(x - 1, y);
                    bool right = x < w - 1 && px(x + 1, y);
                    bool top = y > 0 && !left && !right && Pixels[y * w + x] < BRICK && get(x, y) > 1F;
                    float spd = 0.7F;

                    if (CurrentCell.cell_px != PIPE && (CurrentCell.cell_fluid.From == vec.Null || Pixels[CurrentCell.cell_fluid.From.y * w + CurrentCell.cell_fluid.From.x] != PIPE)) // pas dans tuyau
                    {
                        if(CurrentCell.pipesCount > 0) // au moins un tuyau autour
                        {
                            List<vec> extremities = new List<vec>();
                            CellInfo pipe;
                            foreach (vec pipe_pos in CurrentCell.linked_pipes)
                            {
                                pipe = new CellInfo(pipe_pos);
                                if (pipe.linked_pipes.Count == 1) // extrémité
                                    extremities.Add(pipe_pos);
                            }
                            float q = Math.Min(spd * 2F, f.Q / extremities.Count);
                            foreach (vec pipe_pos in extremities) // pour chaque extrémité
                            {
                                Fluids[pipe_pos.y * w + pipe_pos.x].Q += q;
                                Fluids[pipe_pos.y * w + pipe_pos.x].From = (x, y).V();
                                Fluids[y * w + x].Q -= q;
                            }
                        }
                    }
                    else
                {
                        List<vec> pipes = new List<vec>();
                        foreach (vec pipe_pos in CurrentCell.linked_pipes)
                        {
                            if (pipe_pos != CurrentCell.cell_fluid.From)
                                pipes.Add(pipe_pos);
                            else
                            {
                                int _x = x + (x - CurrentCell.cell_fluid.From.x) * 1;
                                int _y = y + (y - CurrentCell.cell_fluid.From.y) * 1;
                                if (_y <= y && CurrentCell.linked_pipes.Count == 1 && Pixels[_y*w+_x] < BRICK)
                                    pipes.Add((_x, _y).V());
                            }
                        }
                        float q = Math.Min(spd * 2F, f.Q / pipes.Count);
                        foreach (vec pipe_pos in pipes) // pour chaque tuyau
                        {
                            Fluids[pipe_pos.y * w + pipe_pos.x].Q += q;
                            Fluids[pipe_pos.y * w + pipe_pos.x].From = (x, y).V();
                            Fluids[y * w + x].Q -= q;
                        }
                        if(pipes.Count > 0)
                            continue; // ==== cellule suivante ====
                    }

                    if ((!left && right && f.previous_look_x < 0F) || left && !right && f.previous_look_x > 0F) f.previous_look_x *= 0.8F;

                    if (bottom)
                    {
                        if (get(x, y + 1) >= 0.2F)
                        {
                            int c = Convert.ToInt32(left) + Convert.ToInt32(right);
                            if (c > 0)
                            {
                                if (right && f.previous_look_x <= 0F) move(1, 0, f.Q / c * spd);
                                if (left && f.previous_look_x >= 0F) move(-1, 0, f.Q / c * spd);
                            }
                        }
                        else
                        {
                            move(0, 1, f.Q * spd);
                        }
                    }
                    else if (left && right)
                    {
                        float d = f.Q * spd;
                        move(-1, 0, d / 2F);
                        move(1, 0, d / 2F);
                    }
                    else if (left || right)
                    {
                        if (right) move(1, 0, f.Q * spd);
                        if (left) move(-1, 0, f.Q * spd);
                    }
                    else if (top) move(0, -1, f.Q * spd);

                    int digits = 3;
                    bool fix(int _x, int _y)
                    {
                        if (Maths.Round(Fluids[(y+_y) * w + x + _x].Q, digits) == 0F)
                        {
                            Fluids[(y + _y) * w + x + _x].Q = 0F;
                            return true;
                        }
                        return false;
                    }
                    if (fix(0,0))
                    {
                        fix(-1, 0);
                        fix(1, 0);
                        fix(0, -1);
                        fix(0, 1);
                    }
                }
            }
        }
        void ResetFluids()
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Fluids[y * w + x].Reset();
                }
            }
        }
        void ResetPixels(bool only_pipes = true)
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if(!only_pipes || Pixels[y * w + x] == PIPE)
                        Pixels[y * w + x] = 0;
                }
            }
        }

        private void Draw(object sender, EventArgs e)
        {
            Image = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(Image))
            {
                int startX = Math.Max(0, (int)(x - Width / (2 * z)));
                int startY = Math.Max(0, (int)(y - Height / (2 * z)));
                int endX = Math.Min(w-1, (int)(x + Width / z));
                int endY = Math.Min(h-1, (int)(y + Height / z));

                for (int y = startY; y < endY; y++)
                    for (int x = startX; x < endX; x++)
                    {
                        int index = y * w + x;
                        
                        if (Pixels[index] < BRICK)
                        {
                            //    if(Fluids[index].Q == 0F)
                            //        g.FillRectangle(Brushes.White, (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                            //    else
                            //    g.FillRectangle(new SolidBrush(Color.FromArgb((byte)(100 - Fluids[index].Q * 100), (byte)(200 - Fluids[index].Q * 200), (byte)(255 - Fluids[index].Q * 255))), (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                            if (Fluids[index].Q > 0F)
                                g.FillRectangle(PaletteBrushes[2], (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                            else if (Fluids[index].LF > 0F)
                            {
                                g.FillRectangle(PaletteBrushes[1], (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                                Fluids[index].LF -= Fluid.GraphicalDeathSpeed;
                            }
                    }
                        else
                            g.FillRectangle(PaletteBrushes[Pixels[index]], (x - this.x + Width / (2 * z)) * z, (y - this.y + Height / (2 * z)) * z, z, z);
                    }

                var ms = MouseStates.Position.ToPoint();
                int worldX = (int)((ms.X - Width / 2) / z + x);
                int worldY = (int)((ms.Y - Height / 2) / z + y);
                if (worldX >= 0 && worldX < w && worldY >= 0 && worldY < h)
                {
                    int index = worldY * w + worldX;
                    var tx = $"{{{worldX},{worldY}}} {Maths.Round(Fluids[index].Q, 2)}";
                    var txw = g.MeasureString(tx, font).Width;
                    g.DrawString(tx, font, Brushes.Black, ms.X - txw / 2, ms.Y - CharSize.Height * 1.5F);
                    g.DrawString(tx, font, Brushes.White, ms.X - txw / 2 + 1, ms.Y - CharSize.Height * 1.5F + 1);
                }

                g.DrawString(time.ToString(), font, Brushes.Black, 10, 10);
            }
            Render.Image = Image;
        }

    }
}
