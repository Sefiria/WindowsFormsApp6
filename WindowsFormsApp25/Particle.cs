using System.Drawing;
using Tooling;

namespace WindowsFormsApp25
{
    public class Particle
    {
        public static readonly float Gravity = 0.05f;
        public bool Exists = true;
        public vecf Position, Look;
        public float Size, Speed;
        public int ArgbColor, Duration, Ticks;
        public bool ApplyGravity = true;
        public Particle(){}
        public Particle(vecf position, float size, vecf look, float force, int color, int duration)
        {
            Position = position;
            Size = size;
            Look = look;
            Speed = RandomThings.rnd1() * force;
            ArgbColor = color;
            Duration = duration;
            Ticks = 0;
        }
        public void Update()
        {
            if (!Exists)
                return;

            Position += Look * Speed;
            if (Position.x - Size < 0 || Position.x + Size > Core.Instance.ScreenWidth || Position.y + Size > Core.Instance.ScreenHeight)
                Exists = false;
            if(ApplyGravity)
                Look.y += Gravity;

            Ticks++;
            if (Ticks > Duration)
                Exists = false;
        }
        public void Draw(Graphics g)
        {
            if(Position.y - Size >= 0)
                g.FillRectangle(new SolidBrush(Color.FromArgb((byte)((Duration - Ticks) / (float)Duration * byte.MaxValue), Color.FromArgb(ArgbColor))), Position.x, Position.y, Size, Size);
        }
    }
}
