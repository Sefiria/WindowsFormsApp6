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

            Position = Position.PlusF(Look.x(Speed));
            if(StartPosition.vecf().Distance(Position.vecf()) > Distance)
                Exists = false;
            else if (Position.X - Radius < 0 || Position.Y - Radius < 0 || Position.X + Radius > ParticlesManager.Bounds.Width || Position.Y + Radius > ParticlesManager.Bounds.Height)
                Exists = false;
            else
            {
                var raycast = Maths.SimpleRaycastHit(Position, Look, Speed, Distance, Common.Segments);
                Console.WriteLine(Position);
                if(raycast.Index != -1 && Maths.IsLeftOfSegment(Common.Segments[raycast.Index].A, Common.Segments[raycast.Index].B, Position))
                    Exists = false;
            }
        }
        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(ArgbColor)), Position.X - Radius / 2, Position.Y - Radius / 2, Diameter, Diameter);
        }
    }
}
