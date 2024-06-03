using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Tooling;
using static WindowsFormsApp27.Reporting;

namespace WindowsFormsApp27
{
    public class Reporting
    {
        public class DataPoint
        {
            public int Population;
            public int Births, Deaths;
            public DataPoint()
            {
            }

            public void Draw(Reporting context, Graphics g, int i, float i_visible, Rectangle chart_rect)
            {
                Pen p = new Pen(Color.FromArgb(50, 50, 100, 180));
                float x = chart_rect.X + 10 + i_visible * (20 + 10);
                float y = chart_rect.Y + chart_rect.Height - 10;

                int max = context.DataPoints.Select(dp => dp.Population).Max();
                int sz = (int) ((Population / (float)max) * (chart_rect.Height - 20));
                int prev = i > 0 ? context.DataPoints[i - 1].Population : 0;
                int diff = prev == 0 ? 0 : (int)(((Math.Max(prev, Population) - Math.Min(prev, Population)) / (float)max) * (chart_rect.Height - 20));

                var x_offset = Math.Max(0, chart_rect.Left - x);
                var w_offset = -x_offset - Math.Max(0, x + 20 - chart_rect.Right);
                var rect = new RectangleF(x + x_offset, y-sz, 20 + w_offset, sz);
                var hover = rect.Contains(MouseStates.Position);
                Brush b = new SolidBrush(Color.FromArgb(Population == prev ? (hover ? 70 : 50) : (hover ? 200 : 100), 50, 100, 180));
                g.FillRectangle(b, rect);
                if (hover && diff > 0)
                    g.FillRectangle(Population > prev ? Brushes.ForestGreen : Brushes.IndianRed, new RectangleF(x + x_offset, y - sz + diff * (Population > prev ? 0 : -1), 20 + w_offset, diff));

                if (Population != prev)
                    g.DrawLine(Population > prev ? Pens.ForestGreen : Pens.IndianRed, rect.X, rect.Y, rect.X + rect.Width, rect.Y);

                if(hover)
                {
                    g.DrawString((i + 1).ToString(), Common.minifont, Brushes.Yellow, new RectangleF(rect.X, rect.Bottom - rect.Width, rect.Width, rect.Width));

                    int j = 0;
                    int h = (int)g.MeasureString("A", Common.font).Height + 5;
                    b = new SolidBrush(Color.FromArgb(100, Color.SteelBlue));

                    g.DrawString($"Population : {Population}", Common.font, Population == prev ? b : Brushes.SteelBlue, chart_rect.Left, chart_rect.Bottom + 20 + j++ * h);
                    g.DrawString($"Gained : {Math.Max(0, Population - prev)}", Common.font, Math.Max(0, Population - prev) == 0 ? b : Brushes.ForestGreen, chart_rect.Left, chart_rect.Bottom + 20 + j++ * h);
                    g.DrawString($"Lost : {Math.Max(0, prev - Population)}", Common.font, Math.Max(0, prev - Population) == 0 ? b : Brushes.IndianRed, chart_rect.Left, chart_rect.Bottom + 20 + j++ * h);
                    g.DrawString($"Births : {Math.Max(0, Births)}", Common.font, Math.Max(0, Births) == 0 ? b : Brushes.ForestGreen, chart_rect.Left, chart_rect.Bottom + 20 + j++ * h);
                    g.DrawString($"Deaths : {Math.Max(0, Deaths)}", Common.font, Math.Max(0, Deaths) == 0 ? b : Brushes.IndianRed, chart_rect.Left, chart_rect.Bottom + 20 + j++ * h);
                }
            }
        }

        public List<DataPoint> DataPoints;
        private float datapoint_start_index;

        public Rectangle get_chart_rect(int w, int h) => new RectangleF(w * 0.1F, h * 0.1F, w - w * 0.2F, h - h * 0.66F).ToIntRect();

        public Reporting()
        {
            DataPoints = new List<DataPoint>();
        }

        public void AddDataPoint(DataPoint pt)
        {
            animated_bg_data1.Clear();
            DataPoints.Add(pt);
        }
        public void Init(int w, int h)
        {
            Rectangle chart_rect = get_chart_rect(w, h);
            int max = (chart_rect.Width - 20) / 30 + 2;
            datapoint_start_index = Math.Max(0, DataPoints.Count - max);
        }
        public void Update(int w, int h)
        {
            var ms = MouseStates.Position.ToPoint();
            Rectangle chart_rect = get_chart_rect(w, h);
            if (!chart_rect.Contains(ms))
                return;
            var msr = ms.Minus(chart_rect.Location);
            int max = (chart_rect.Width - 20) / 30 + 2;
            if (msr.X < chart_rect.Width / 5)
                datapoint_start_index = Math.Max(0, datapoint_start_index - 0.1F);
            if (msr.X > chart_rect.Width - chart_rect.Width / 5 && DataPoints.Count - 1 - datapoint_start_index - max > 0)
                datapoint_start_index += 0.1F;
        }
        public void GenerateDraw(Graphics g)
        {
            int w = (int)g.VisibleClipBounds.Width;
            int h = (int)g.VisibleClipBounds.Height;
            Rectangle chart_rect = get_chart_rect(w, h);

            float t = (Common.ticks % 400) < 200 ? (Common.ticks % 200) / 200F : 1F - (Common.ticks % 200) / 200F;
            g.Clear(Color.FromArgb((byte)(5 * t), (byte)(10 * t), (byte)(40 * t)));

            draw_animated_bg(g, w, h);

            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 50, 130)), chart_rect);
            g.DrawRectangle(new Pen(Color.FromArgb(200, 50, 100, 180)), chart_rect);

            int max = (chart_rect.Width - 20) / 30 + 2;
            for (int i = (int)datapoint_start_index; i - (int)datapoint_start_index < Math.Min(max, DataPoints.Count); i++)
                DataPoints[i].Draw(this, g, i, i - datapoint_start_index, chart_rect);

            g.DrawLine(new Pen(Color.FromArgb(50, 50, 100, 180)), chart_rect.X, chart_rect.Y + chart_rect.Height / 2, chart_rect.X + chart_rect.Width, chart_rect.Y + chart_rect.Height / 2);
        }
        public void PreGenerate(int w, int h)
        {
            animated_bg_data3.Add(0);
            animated_bg_data3.Add(0);
            animated_bg_data3.Add(0);

            for (int i = 0; i < 1; i++)
            {
                var qw = w / 4F;
                var qh = h / 4F;
                var qd = w / 4F;// depth (z)
                animated_bg_data3.Add(RandomThings.rnd1Around0() * qw);
                animated_bg_data3.Add(RandomThings.rnd1Around0() * qh);
                animated_bg_data3.Add(RandomThings.rnd1Around0() * qd);
            }
        }
        public void PostGenerate()
        {
            animated_bg_data1.Clear();
            animated_bg_data2.Clear();
        }
        private List<float> animated_bg_data1 = new List<float>();
        private List<float> animated_bg_data2 = new List<float>();
        private List<float> animated_bg_data3 = new List<float>();
        private void draw_animated_bg(Graphics g, int w, int h)
        {
            bool draw_data1 = false;
            bool draw_data2 = true;
            bool draw_data3 = false;

            int count = 5;
            int count2 = 50;
            float node_max_distance = w / 2F;

            if (draw_data1)
            {
                for (int i = 0; i < animated_bg_data1.Count; i += 5)
                {
                    float ax = animated_bg_data1[i + 0];
                    float ay = animated_bg_data1[i + 1];
                    float bx = animated_bg_data1[i + 2];
                    float by = animated_bg_data1[i + 3];
                    float hp = animated_bg_data1[i + 4];

                    animated_bg_data1[i + 0] -= 2F;
                    animated_bg_data1[i + 2] -= 2F;
                    animated_bg_data1[i + 4] -= 0.005F;
                    if (animated_bg_data1[i + 4] <= 0F)
                    {
                        animated_bg_data1.RemoveRange(i, 5);
                        i -= 5;
                    }

                    var c = Color.FromArgb((byte)(hp * byte.MaxValue).ByteCut(), 130, 180, 255);
                    g.DrawLine(new Pen(c), ax, ay, bx, by);
                    vecf v, v2, a = (ax, ay).Vf(), b = (bx, by).Vf();
                    for (float t = 0F; t <= 1F; t += 3F / a.Distance(b))
                    {
                        v = Maths.Lerp(a, b, t);
                        v2 = (v.x + 60, v.y).Vf();
                        for (float t2 = 0F; t2 <= 1F; t2 += 10F / v.Distance(v2))
                            g.DrawLine(new Pen(Color.FromArgb((byte)(hp * 5), 130, 180, 255), t2 * 5F), v.pt, Maths.Lerp(v, v2, t2).pt);
                    }
                }
            }

            if (draw_data2)
            {
                List<(vecf v, float hp)> nodes = new List<(vecf v, float hp)>();
                for (int i = 0; i < animated_bg_data2.Count; i += 4)
                {
                    float x = animated_bg_data2[i + 0];
                    float y = animated_bg_data2[i + 1];
                    float hp = animated_bg_data2[i + 2];
                    float speed = animated_bg_data2[i + 3];
                    nodes.Add(((x, y).Vf(), hp));

                    animated_bg_data2[i + 0] -= speed * 2F;
                    animated_bg_data2[i + 2] -= 0.005F;
                    if (animated_bg_data2[i + 2] <= 0F || x < -node_max_distance)
                    {
                        animated_bg_data2.RemoveRange(i, 4);
                        i -= 4;
                    }

                    var c = Color.FromArgb((byte)(hp * byte.MaxValue / 2F).ByteCut(), 130, 180, 255);
                    float r = 4;
                    g.FillEllipse(new SolidBrush(c), x - hp * r, y - hp * r, hp * r * 2, hp * r * 2);
                }
                foreach (var node in nodes)
                {
                    var list = nodes.Except(node).Where(n => n.v.Distance(node.v) < node_max_distance);
                    foreach (var next_node in list)
                    {
                        var d = 1F - next_node.v.Distance(node.v) / node_max_distance;
                        var c = Color.FromArgb((byte)(d * (node.hp / 2F + next_node.hp / 2F) * byte.MaxValue / 4F).ByteCut(), 130, 180, 255);
                        g.DrawLine(new Pen(c), node.v.x, node.v.y, next_node.v.x, next_node.v.y);
                    }
                }
            }

            while (animated_bg_data1.Count < count * 5)
            {
                animated_bg_data1.Add(w + RandomThings.rnd1() * w);
                animated_bg_data1.Add(RandomThings.rnd1() * h);
                animated_bg_data1.Add(w + RandomThings.rnd1() * w);
                animated_bg_data1.Add(RandomThings.rnd1() * h);
                animated_bg_data1.Add(RandomThings.rnd1() * 2F);
            }
            while (animated_bg_data2.Count < count2 * 4 && RandomThings.rnd1() < 0.2F)
            {
                animated_bg_data2.Add(w / 4 + RandomThings.rnd1() * w);
                animated_bg_data2.Add(RandomThings.rnd1() * h);
                animated_bg_data2.Add(RandomThings.rnd1());
                animated_bg_data2.Add(RandomThings.rnd1());
            }




            if (draw_data3)
            {
                List<(Point3D p, int id)> nodes3d = new List<(Point3D p, int id)>();
                for (int i = 0; i < animated_bg_data3.Count; i += 3)
                {
                    float x = animated_bg_data3[i + 0];
                    float y = animated_bg_data3[i + 1];
                    float z = animated_bg_data3[i + 2];
                    nodes3d.Add((new Point3D(x, y, z), i));
                    var c = Color.FromArgb(100, 130, 180, 255);
                    float r = 4;
                    float scale = (z + w / 4F) / (w / 2F);
                    float rScaled = r * scale;
                    g.FillEllipse(new SolidBrush(c), w / 2F + x - rScaled, h / 2F + y - rScaled, rScaled * 2, rScaled * 2);
                }
                var nodes3d_copy = new List<(Point3D p, int id)>(nodes3d);
                foreach (var node in nodes3d_copy)
                {
                    (animated_bg_data3[node.id + 0], animated_bg_data3[node.id + 1], animated_bg_data3[node.id + 2]) = UpdatedPointPosition(node.p);
                    var list = nodes3d_copy.Except(node).Where(n => n.p.Distance(node.p) < node_max_distance);
                    foreach (var next_node in list)
                    {
                        var d = 1F - next_node.p.Distance(node.p) / node_max_distance;
                        d = d < 0 ? 0 : d;
                        //float penWidth = 1F / (1F + (float)(node.Z + next_node.Z) / 2000F);
                        var c = Color.FromArgb((byte)(d * byte.MaxValue / 2F).ByteCut(), 130, 180, 255);
                        g.DrawLine(new Pen(c/*, penWidth*/), w / 2F + (float)node.p.X, h / 2F + (float)node.p.Y, w / 2F + (float)next_node.p.X, h / 2F + (float)next_node.p.Y);
                    }
                }
            }
        }
        private (float x, float y, float z) UpdatedPointPosition(Point3D point)
        {
            float newX = (float)(point.X * (float)Math.Cos(0.0174) - point.Y * (float)Math.Sin(0.0174));
            float newY = (float)(point.X * (float)Math.Sin(0.0087) + point.Y * (float)Math.Cos(0.0087));
            float newZ = (float)(point.X * (float)Math.Sin(0.0174) + point.Z * (float)Math.Cos(0.0174));
            return ((float)newX, (float)newY, (float)newZ);
        }
    }
}
