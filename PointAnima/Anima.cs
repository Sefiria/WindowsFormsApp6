using PointAnima.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace PointAnima
{
    public class Anima
    {
        public string Name;
        public PointF Position;
        public APoint Head;
        public Pen Pen = new Pen(Color.White, 2F);

        public Anima()
        {
        }
        public Anima(Anima copy)
        {
            Name = copy.Name;
            Position = new PointF(copy.Position.X, copy.Position.Y);
            Head = new APoint(copy.Head);
            Pen = new Pen(copy.Pen.Color, copy.Pen.Width);
        }

        public void Create()
        {
            Head = new APoint() { Name = "Head", Position = new PointF(0, -0.75F - 0.6250F) };
            var BodyUp = new APoint() { Name = "BodyUp", Position = new PointF(0, 0 - 0.6250F) };
            var BodyDown = new APoint() { Name = "BodyUp", Position = new PointF(0, 1 - 0.6250F) };
            var ArmLeft = new APoint() { Name = "ArmLeft", Position = new PointF(-0.75F, 0.75F - 0.6250F) };
            var ArmRight = new APoint() { Name = "ArmRight", Position = new PointF(0.75F, 0.75F - 0.6250F) };
            var LegLeft = new APoint() { Name = "LegLeft", Position = new PointF(-0.5F, 2F - 0.6250F) };
            var LegRight = new APoint() { Name = "LegRight", Position = new PointF(0.5F, 2F - 0.6250F) };

            BodyDown.Linked.AddRange(new List<APoint>() { LegLeft, LegRight});
            BodyUp.Linked.AddRange(new List<APoint>() { ArmLeft, ArmRight, BodyDown });
            Head.Linked.AddRange(new List<APoint>() { BodyUp });
        }

        public APoint Get(string name)
        {
            APoint recursive_search(List<APoint> list)
            {
                var found = list.FirstOrDefault(ap => ap.Name == name);
                if (found != null) return found;
                foreach(var ap in list)
                {
                    found = recursive_search(ap.Linked);
                    if (found != null) return found;
                }
                return null;
            }
            if (Head?.Name == name) return Head;
            return recursive_search(Head.Linked);
        }
        public APoint Get(PointF position, float scale)
        {
            float x, y, r = 0.2F * scale;
            APoint recursive_search(List<APoint> list)
            {
                foreach (var ap in list)
                {
                    x = Position.X + ap.Position.X * scale;
                    y = Position.Y + ap.Position.Y * scale;
                    if (position.X < x - r || position.X >= x + r || position.Y < y - r || position.Y >= y + r)
                    {
                        var found = recursive_search(ap.Linked);
                        if(found != null) return found;
                        continue;
                    }
                    return ap;
                }
                return null;
            }

            x = Position.X + Head.Position.X * scale;
            y = Position.Y + Head.Position.Y * scale;
            if (position.X < x - r || position.X >= x + r || position.Y < y - r || position.Y >= y + r)
                return recursive_search(Head.Linked);
            return Head;
        }

        public void Draw(Graphics g, float scale)
        {
            Head.Draw(g, Pen, Position, scale, true);
        }
        internal void DrawLerp(Graphics g, float scale, Anima frameB, float t)
        {
            Head.DrawLerp(g, Pen, Maths.Lerp(Position, frameB.Position, t), scale, true, frameB.Head, t);
        }
    }

    public class APoint
    {
        public string Name;
        public PointF Position;
        public List<APoint> Linked;

        public APoint()
        {
            Linked = new List<APoint>();
        }
        public APoint(APoint copy)
        {
            Position = new PointF(copy.Position.X, copy.Position.Y);
            Linked = new List<APoint>(copy.Linked);
        }

        public void Draw(Graphics g, Pen pen, PointF globalPosition, float scale = 1F, bool draw_node_circle = false)
        {
            var x = globalPosition.X + Position.X * scale;
            var y = globalPosition.Y + Position.Y * scale;
            if (draw_node_circle)
            {
                var r = 0.2F * scale;
                g.DrawEllipse(pen, x - r, y - r, r * 2, r * 2);
            }

            foreach (var ap in Linked)
            {
                g.DrawLine(pen, x, y, globalPosition.X + ap.Position.X * scale, globalPosition.Y + ap.Position.Y * scale);
                ap.Draw(g, pen, globalPosition, scale, draw_node_circle);
            }
        }
        internal void DrawLerp(Graphics g, Pen pen, PointF globalPosition, float scale, bool draw_node_circle, APoint frameB, float t)
        {
            var x = globalPosition.X + Maths.Lerp(Position.X, frameB.Position.X, t) * scale;
            var y = globalPosition.Y + Maths.Lerp(Position.Y, frameB.Position.Y, t) * scale;
            if (draw_node_circle)
            {
                var r = 0.2F * scale;
                g.DrawEllipse(pen, x - r, y - r, r * 2, r * 2);
            }

            float _x, _y;
            APoint apA, apB;
            for (int i=0; i<Linked.Count; i++)
            {
                apA = Linked[i];
                apB = frameB.Linked.Count > i ? frameB.Linked[i] : null;
                if(apB == null)
                {
                    _x = globalPosition.X + apA.Position.X * scale;
                    _y = globalPosition.Y + apA.Position.Y * scale;
                }
                else
                {
                    _x = globalPosition.X + Maths.Lerp(apA.Position.X, apB.Position.X, t) * scale;
                    _y = globalPosition.Y + Maths.Lerp(apA.Position.Y, apB.Position.Y, t) * scale;
                }
                g.DrawLine(pen, x, y, _x, _y);
                apA.DrawLerp(g, pen, globalPosition, scale, draw_node_circle, apB, t);
            }
        }

        public void DrawNode(Graphics g, Pen pen, PointF globalPosition, float scale)
        {
            var x = globalPosition.X + Position.X * scale;
            var y = globalPosition.Y + Position.Y * scale;
            var r = 0.2F * scale;
            g.DrawEllipse(pen, x - r, y - r, r * 2, r * 2);
        }
    }
}
