using System;
using System.Drawing;

using Tooling;
namespace WindowsFormsApp28_px
{
    public class Bullet
    {
        public bool Exists = true;
        public PointF StartPosition, Position, Look;
        public float Diameter, Speed;
        public int ArgbColor, Distance;

        public float Radius => Diameter / 2F;

        public Bullet(){}
        public Bullet(PointF position, float angle, float diameter, float speed, int color, int distance)
        {
            StartPosition = Position = position;
            Diameter = diameter;
            Look = (1F,0F).P().Rotate(angle);
            Speed = speed;
            ArgbColor = color;
            Distance = distance;
        }
        public void Update()
        {
            if (!Exists)
                return;

            float wh = Common.ScreenWidth / 2F;
            float hh = Common.ScreenHeight / 2F;

            Position = Position.PlusF(Look.x(Speed));
            if(StartPosition.vecf().Distance(Position.vecf()) > Distance)
                Exists = false;
            else if (Position.X - Radius < -wh || Position.Y - Radius < -hh || Position.X + Radius > wh || Position.Y + Radius > hh)
                Exists = false;
            else
            {
                var raycast = Maths.SimpleRaycastHit(Position, Look, Speed, Distance, Common.Segments, 10F + Speed);
                if (raycast.Index != -1)
                {
                    if (!Maths.IsLeftOfSegment(Common.Segments[raycast.Index], Position, 10F + Speed))
                        Exists = false;
                }
                else
                {
                    raycast = Maths.SimpleRaycastHit(Position, Look.x(-1F), Speed, Distance, Common.Segments, 10F + Speed);
                    if (raycast.Index != -1)
                    {
                        if (!Maths.IsLeftOfSegment(Common.Segments[raycast.Index], Position, 10F + Speed))
                            Exists = false;
                    }
                }
            }
        }
        public void Draw(Graphics g, PointF? Offset = null)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(ArgbColor)), Position.X - Radius / 2 + Offset?.X ?? 0, Position.Y - Radius / 2 + Offset?.Y ?? 0, Diameter, Diameter);
        }
    }
}
