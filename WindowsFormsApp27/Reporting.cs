using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp27
{
    public class Reporting
    {
        public class DataPoint
        {
            public int Population;
            public DataPoint()
            {
            }

            public void Draw(Reporting context, Graphics g, int i, Rectangle chart_rect)
            {
                var hover = MouseStates.Position;
                Pen p = new Pen(Color.FromArgb(50, 50, 100, 180));
                int x = chart_rect.X + 10 + i * (20 + 10);
                int y = chart_rect.Y + chart_rect.Height - 10;

                int max = context.DataPoints.Select(dp => dp.Population).Max();
                int sz = (int) ((Population / (float)max) * (chart_rect.Height - 20));

                var rect = x, y-sz, 20, sz;
                Brush b = new SolidBrush(Color.FromArgb(hover ? 200 : 100, 50, 100, 180));
                g.FillRectangle(b, rect);
                g.DrawLine(p, chart_rect.X, chart_rect.Y + chart_rect.Height / 2, chart_rect.X + chart_rect.Width, chart_rect.Y + chart_rect.Height / 2);
            }
        }

        public List<DataPoint> DataPoints;

        public Reporting()
        {
            DataPoints = new List<DataPoint>();
        }

        public void AddDataPoint(DataPoint pt)
        {
            animated_bg_data.Clear();
            DataPoints.Add(pt);
        }
        public void GenerateDraw(Graphics g)
        {
            int w = (int)g.VisibleClipBounds.Width;
            int h = (int)g.VisibleClipBounds.Height;
            Rectangle chart_rect = new RectangleF(w * 0.1F, h * 0.1F, w - w * 0.2F, h - h * 0.66F).ToIntRect();

            float t = (Common.ticks % 400) < 200 ? (Common.ticks % 200) / 200F : 1F - (Common.ticks % 200) / 200F;
            g.Clear(Color.FromArgb((byte)(10 * t), (byte)(20 * t), (byte)(100 * t)));

            draw_animated_bg(g, w, h);

            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 50, 130)), chart_rect);
            g.DrawRectangle(new Pen(Color.FromArgb(200, 50, 100, 180)), chart_rect);

            for (int i = 0; i < DataPoints.Count; i++)
                DataPoints[i].Draw(this, g, i, chart_rect);
        }
        private List<float> animated_bg_data = new List<float>();
        private List<float> animated_bg_data2 = new List<float>();
        private void draw_animated_bg(Graphics g, int w, int h)
        {
            int count = 5;
            int count2 = 50;

            for (int i = 0; i < animated_bg_data.Count; i += 5)
            {
                float ax = animated_bg_data[i + 0];
                float ay = animated_bg_data[i + 1];
                float bx = animated_bg_data[i + 2];
                float by = animated_bg_data[i + 3];
                float hp = animated_bg_data[i + 4];

                animated_bg_data[i + 0] -= 2F;
                animated_bg_data[i + 2] -= 2F;
                animated_bg_data[i + 4] -= 0.005F;
                if (animated_bg_data[i + 4] <= 0F)
                {
                    animated_bg_data.RemoveRange(i, 5);
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

            List<vecf> nodes = new List<vecf>();
            for (int i = 0; i < animated_bg_data2.Count; i += 4)
            {
                float x = animated_bg_data2[i + 0];
                float y = animated_bg_data2[i + 1];
                float hp = animated_bg_data2[i + 2];
                float speed = animated_bg_data2[i + 3];
                nodes.Add((x, y).Vf());

                animated_bg_data2[i + 0] -= speed * 2F;
                animated_bg_data2[i + 2] -= 0.001F;
                if (animated_bg_data2[i + 2] <= 0F)
                {
                    animated_bg_data2.RemoveRange(i, 4);
                    i -= 4;
                }

                var c = Color.FromArgb((byte)(hp * byte.MaxValue).ByteCut(), 130, 180, 255);
                float r = 4;
                g.FillEllipse(new SolidBrush(c), x - hp * r, y - hp * r, hp * r * 2, hp * r * 2);
            }
            float node_max_distance = 100F;
            foreach (var node in nodes)
            {
                var list = nodes.Except(node).Where(n => n.Distance(node) < node_max_distance);
                foreach (var next_node in list)
                {
                    var d = 1F - next_node.Distance(node) / node_max_distance;
                    var c = Color.FromArgb((byte)(d * byte.MaxValue / 2F).ByteCut(), 130, 180, 255);
                    g.DrawLine(new Pen(c), node.x, node.y, next_node.x, next_node.y);
                }
            }

            while (animated_bg_data.Count < count * 5)
            {
                animated_bg_data.Add(RandomThings.rnd1() * w);
                animated_bg_data.Add(RandomThings.rnd1() * h);
                animated_bg_data.Add(RandomThings.rnd1() * w);
                animated_bg_data.Add(RandomThings.rnd1() * h);
                animated_bg_data.Add(RandomThings.rnd1());
            }
            while (animated_bg_data2.Count < count2 * 4 && RandomThings.rnd1() < 0.2F)
            {
                animated_bg_data2.Add(RandomThings.rnd1() * w);
                animated_bg_data2.Add(RandomThings.rnd1() * h);
                animated_bg_data2.Add(RandomThings.rnd1());
                animated_bg_data2.Add(RandomThings.rnd1());
            }
        }
    }
}
