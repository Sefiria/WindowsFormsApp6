using ComputeSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Tooling;
using WindowsFormsApp22.Entities;
//using ComputeSharp;
using System.Drawing.Drawing2D;
using System.Numerics;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace WindowsFormsApp22
{
    internal partial class Map
    {
        void draw_grid_old_old(Graphics g)
        {
            Bitmap img = new Bitmap((int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height);
            using (Graphics _g = Graphics.FromImage(img))
            {
                Pen pen = new Pen(Color.FromArgb(250, 180, 190, 210));
                float modx = Core.Cam.X % Core.Cube;
                float mody = Core.Cam.Y % Core.Cube;
                for (float y = -1F; y < Core.RH / Core.Cube + 2; y++)
                {
                    _g.DrawLine(pen, -Core.Cube - modx, y * Core.Cube - mody, Core.RW - modx, y * Core.Cube - mody);
                }
                for (float x = -1F; x < Core.RW / Core.Cube + 2; x++)
                {
                    _g.DrawLine(pen, x * Core.Cube - modx, -Core.Cube - mody, x * Core.Cube - modx, Core.RH - mody);
                }
            }
            img.MakeTransparent();

            List<Entity> entities = new List<Entity>(Core.Map.Entities).Where(e => e.IsDrawable).OrderByDescending(b => b.Y).ToList();
            int diam, _x, _y;
            Point pt;
            float d;
            Color px;
            foreach (var entity in entities)
            {
                diam = entity.weight;
                for (int y = diam / 2; y >= -diam / 2; y--)
                {
                    for (int x = -diam / 2; x <= diam / 2; x++)
                    {
                        d = diam * 2F / (float)Math.Log(Maths.Distance(Point.Empty, new Point(x, y))) - diam / 2F - 5F;
                        if (d < 0) d = 0;
                        _x = (int)(Core.hw + (entity is Player ? 0 : entity.X - Core.Player.X) + x - Core.cam_ofs.x)/* - diam / 2*/;
                        _y = (int)(Core.hh + (entity is Player ? 0 : entity.Y - Core.Player.Y) + y - Core.cam_ofs.y)/* + diam / 2*/;
                        if (_x >= 0 && _x < Core.iRW && _y >= 0 && _y < Core.iRH)
                        {
                            px = img.GetPixel(_x, _y);
                            img.SetPixel(_x, _y, Color.Empty);
                            if (px.ToArgb().CompareTo(Color.Empty.ToArgb()) != 0 && _y + (int)d >= 0 && _y + (int)d < Core.iRH)
                                img.SetPixel(_x, _y + (int)d, px);
                        }
                    }
                }
            }

            g.DrawImage(img, Point.Empty);
        }
        void draw_grid_old(Graphics g)
        {
            Bitmap img = new Bitmap((int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height);
            using (Graphics _g = Graphics.FromImage(img))
            {
                Pen pen = new Pen(Color.FromArgb(250, 180, 190, 210));
                pen.DashStyle = DashStyle.Dot;
                pen.DashPattern = new float[] { 1F, 0.1F, 0.1F, 4F };
                float modx = Core.Cam.X % Core.Cube;
                float mody = Core.Cam.Y % Core.Cube;
                for (float y = -1F; y < Core.RH / Core.Cube + 2; y++)
                {
                    _g.DrawLine(pen, -Core.Cube - modx, y * Core.Cube - mody, Core.RW - modx, y * Core.Cube - mody);
                }
                for (float x = -1F; x < Core.RW / Core.Cube + 2; x++)
                {
                    _g.DrawLine(pen, x * Core.Cube - modx, -Core.Cube - mody, x * Core.Cube - modx, Core.RH - mody);
                }
            }
            img.MakeTransparent();


            var bData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int bitsPerPixel = 4;// Image.GetPixelFormatSize(bData.PixelFormat);
            int size = bData.Stride * bData.Height;
            byte[] data = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);

            int calcI(int x, int y) => bitsPerPixel * x + bData.Stride * y;

            Color get(int x, int y)
            {
                int i = calcI(x, y);
                var _a = data[i + 0];
                var _r = data[i + 1];
                var _g = data[i + 2];
                var _b = data[i + 3];
                return Color.FromArgb(_a, _r, _g, _b);
            }
            void set(int x, int y, Color c)
            {
                int i = calcI(x, y);
                data[i + 0] = c.A;
                data[i + 1] = c.R;
                data[i + 2] = c.G;
                data[i + 3] = c.B;
            }

            List<Entity> entities = new List<Entity>(Core.Map.Entities).Where(e => e.IsDrawable).OrderByDescending(b => b.Y).ToList();
            int diam, _x, _y;
            Point pt;
            float d;
            Color px;
            foreach (var entity in entities)
            {
                diam = entity.weight * 5;
                for (int y = diam / 2; y >= -diam / 2; y--)
                {
                    for (int x = -diam / 2; x <= diam / 2; x++)
                    {
                        d = (float)Math.Exp(diam / Maths.Distance(Point.Empty, new Point(x, y)) - 2F);
                        if (d < 0) continue;
                        _x = (int)(Core.hw + (entity is Player ? 0 : entity.X - Core.Player.X) + x - Core.cam_ofs.x)/* - diam / 2*/;
                        _y = (int)(Core.hh + (entity is Player ? 0 : entity.Y - Core.Player.Y) + y - Core.cam_ofs.y)/* + diam / 2*/;
                        if (_x >= 0 && _x < Core.iRW && _y >= 0 && _y < Core.iRH)
                        {
                            px = get(_x, _y);
                            set(_x, _y, Color.Empty);
                            if (px.ToArgb().CompareTo(Color.Empty.ToArgb()) != 0 && _y + (int)d >= 0 && _y + (int)d < Core.iRH)
                                set(_x, _y + (int)d, px);
                        }
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(data, 0, bData.Scan0, data.Length);
            img.UnlockBits(bData);
            img.MakeTransparent();
            g.DrawImage(img, Point.Empty);
        }
        void draw_grid(Graphics g)
        {
            float cb = 16F;
            float hblsz = 8F;

            var p = Core.Player;
            int tw = (int)(Core.RW / cb) + 1, th = (int)(Core.RH / cb) + 1;
            List<Entity> entities = new List<Entity>(Core.Map.Entities).Where(e => e.IsDrawable).OrderByDescending(b => b.Y).ToList();
            Bitmap img = new Bitmap((int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height);
            float modx = Core.Cam.X % cb;
            float mody = Core.Cam.Y % cb;
            Brush brush = new SolidBrush(Color.FromArgb(50, 50, 50));
            var ev = Core.EvtMgr;
            var c = ev.cooldown;
            var mc = ev.max_cooldown;
            var cn = ev.count;
            var w = ev.wave;
            var pending = ev.Pending;

            using (Graphics _g = Graphics.FromImage(img))
            {
                PointF A = PointF.Empty, B = PointF.Empty, P, P_, PP_;
                if (Core.Player.IsSpecialShoting)
                {
                    A = p.DrawPoint;
                    B = Maths.GetRayToolingLine(p.DrawPoint, p.specialAngle.AngleToPointF(), Core.VisibleBounds);
                }
                for (int y = -2; y < th + 2; y++)
                {
                    for (int x = -2; x < tw + 2; x++)
                    {
                        float node_weight = 0F;
                        foreach (var entity in entities)
                        {
                            float d = Maths.Distance(entity.DrawPoint, (x * cb - modx, y * cb - mody).P());
                            node_weight += (entity.weight * cb) / d;
                        }
                        if (Core.Player.IsSpecialShoting)
                        {
                            P = new PointF(x * cb, y * cb);
                            P_ = Maths.ProjectionSurSegment(A, B, P);
                            PP_ = P_.Minus(P);
                            var d = PP_.Length();
                            node_weight += 5000F * p.specialRaySize / d;
                        }
                        if (node_weight == 0 || (node_weight > 0.0001F && node_weight <= cb * hblsz))
                        {
                            int v = 10 + (int)(node_weight / (cb * hblsz) * 180);
                            _g.FillRectangle(Brushes.Black, x * cb - modx, y * cb + node_weight - mody, hblsz * 2F, hblsz * 2F);
                            _g.FillEllipse(new SolidBrush(Color.FromArgb(v, v, v)), x * cb - modx, y * cb + node_weight - mody, hblsz * 2F, hblsz * 2F);
                        }
                    }
                }

                void draw(Entity e)
                {
                    if (!e.IsDrawable || Core.VisibleBounds.Contains(e.iPos))
                        return;
                    var path = new GraphicsPath(new PointF[]
                    {
                    (-1F,0.5F).P(), (0F,-0.5F).P(), (1F,0.5F).P()
                    }, new byte[] {
                    0, 1, 1
                    });
                    var matrix = new Matrix();
                    var d = Maths.Distance(Core.CenterPoint, e.DrawPoint);
                    matrix.Rotate(e.DrawPoint.Minus(Core.CenterPoint).GetAngle() + 90, MatrixOrder.Prepend);
                    var lk = e.DrawPoint.Minus(Core.CenterPoint).norm();
                    var to = Maths.GetRayToolingLine(e.DrawPoint, lk, 10, 60, Core.RW - 10, Core.RH - 10);
                    matrix.Translate(to.X, to.Y, MatrixOrder.Append);
                    matrix.Scale(5F, 5F);
                    path.Transform(matrix);
                    var l = Math.Max(Core.iRW, Core.iRH) / 2;
                    RangeValue v = new RangeValue((int)(255 * ((d - l) / (l * 2))), 0, 255);
                    var b = new SolidBrush(Color.FromArgb(255, v.Value, 0));
                    _g.FillPath(b, path);
                }
                pending.ForEach(e => draw(e));

                // player's special

                if (p.IsSpecialShoting)
                {
                    var color = Color.FromArgb(100, 0, (int)(p.specialRaySize / 2F * 255), (int)(p.specialRaySize * 255));
                    var pen = new Pen(color, p.specialRaySize * p.specialRaySizeBase * 2F);
                    var ptA = p.DrawPoint;
                    PointF ptB;
                    void draw_special(float a_ofst)
                    {
                        ptB = Maths.GetRayToolingLine(p.DrawPoint, (p.specialAngle - a_ofst).AngleToPointF(), Core.VisibleBounds);
                        _g.DrawLine(pen, ptA, ptB);
                    }
                    float step = 0.2F;
                    for(float t = -step; t <= 1F; t += step)
                        draw_special(p.specialRaySizeBase * t);
                }
            }

            img = img.SetOpacity(Core.Player.IsBlind_Visibility.Value);
            img.MakeTransparent(Color.White);
            g.DrawImage(img, Point.Empty);

            var txt = $"~{w}/{cn} ({mc - cn * 5 * w - c}/{mc - cn * 5 * w}), still {{{pending.Count}}}";
            g.DrawString(txt, Core.Font, Brushes.White, (10, 20).P());
        }

            //[ThreadGroupSize(DefaultThreadGroupSizes.X)]
            //[GeneratedComputeShaderDescriptor]
            //public readonly partial struct MultiplyByTwo(ReadWriteBuffer<int> buffer) : IComputeShader
            //{
            //    public void Execute()
            //    {
            //        buffer[ThreadIds.X] *= 2;
            //    }
            //}
    }
}