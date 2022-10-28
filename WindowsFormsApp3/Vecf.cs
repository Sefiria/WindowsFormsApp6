using System.Drawing;

namespace WindowsFormsApp3
{
    public class Vecf
    {
        public Vecf()
        {
            X = 0F;
            Y = 0F;
        }
        public Vecf(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public float X, Y;
        public Point Point => new Point((int)X, (int)Y);
        public Size Size => new Size((int)X, (int)Y);
        public SizeF SizeF => new SizeF(X, Y);
        public static Vecf operator +(Vecf L, Vecf R) => new Vecf
        {
            X = L.X + R.X,
            Y = L.Y + R.Y
        };
        public static Vecf operator -(Vecf L, Vecf R) => new Vecf
        {
            X = L.X - R.X,
            Y = L.Y - R.Y
        };
        public static Vecf operator *(Vecf L, Vecf R) => new Vecf
        {
            X = L.X * R.X,
            Y = L.Y * R.Y
        };
        public static Vecf operator /(Vecf L, Vecf R) => new Vecf
        {
            X = L.X / (R.X != 0 ? R.X : 1),
            Y = L.Y / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(Vecf L, Vecf R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(Vecf L, Vecf R) => L.X != R.X || L.Y != R.Y;
        public static Vecf operator +(Vecf L,float V) => new Vecf
        {
            X = L.X + V,
            Y = L.Y + V
        };
        public static Vecf operator -(Vecf L, float V) => new Vecf
        {
            X = L.X - V,
            Y = L.Y - V
        };
        public static Vecf operator *(Vecf L, float V) => new Vecf
        {
            X = L.X * V,
            Y = L.Y * V
        };
        public static Vecf operator /(Vecf L, float V) => new Vecf
        {
            X = L.X / (V != 0 ? V : 1),
            Y = L.Y / (V != 0 ? V : 1)
        };
        public static bool operator ==(Vecf L, float V) => L.X == V && L.Y == V;
        public static bool operator !=(Vecf L, float V) => L.X != V || L.Y != V;
        public static Vecf operator +(Vecf L, Point R) => new Vecf
        {
            X = L.X + R.X,
            Y = L.Y + R.Y
        };
        public static Vecf operator -(Vecf L, Point R) => new Vecf
        {
            X = L.X - R.X,
            Y = L.Y - R.Y
        };
        public static Vecf operator *(Vecf L, Point R) => new Vecf
        {
            X = L.X * R.X,
            Y = L.Y * R.Y
        };
        public static Vecf operator /(Vecf L, Point R) => new Vecf
        {
            X = L.X / (R.X != 0 ? R.X : 1),
            Y = L.Y / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(Vecf L, Point R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(Vecf L, Point R) => L.X != R.X || L.Y != R.Y;
        public static Vecf operator +(Vecf L, Vec R) => new Vecf
        {
            X = L.X + R.X,
            Y = L.Y + R.Y
        };
        public static Vecf operator -(Vecf L, Vec R) => new Vecf
        {
            X = L.X - R.X,
            Y = L.Y - R.Y
        };
        public static Vecf operator *(Vecf L, Vec R) => new Vecf
        {
            X = L.X * R.X,
            Y = L.Y * R.Y
        };
        public static Vecf operator /(Vecf L, Vec R) => new Vecf
        {
            X = L.X / (R.X != 0 ? R.X : 1),
            Y = L.Y / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(Vecf L, Vec R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(Vecf L, Vec R) => L.X != R.X || L.Y != R.Y;
        public static Point operator +(Point L, Vecf R) => new Point
        {
            X = (int)(L.X + R.X),
            Y = (int)(L.Y + R.Y)
        };
        public static Point operator -(Point L, Vecf R) => new Point
        {
            X = (int)(L.X - R.X),
            Y = (int)(L.Y - R.Y)
        };
        public static Point operator *(Point L, Vecf R) => new Point
        {
            X = (int)(L.X * R.X),
            Y = (int)(L.Y * R.Y)
        };
        public static Point operator /(Point L, Vecf R) => new Point
        {
            X = (int)(L.X / (R.X != 0 ? R.X : 1)),
            Y = (int)(L.Y / (R.Y != 0 ? R.Y : 1))
        };
        public static bool operator ==(Point L, Vecf R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(Point L, Vecf R) => L.X != R.X || L.Y != R.Y;
        public static Vecf operator +(Vecf L, Size R) => new Vecf
        {
            X = L.X + R.Width,
            Y = L.Y + R.Height
        };
        public static Vecf operator -(Vecf L, Size R) => new Vecf
        {
            X = L.X - R.Width,
            Y = L.Y - R.Height
        };
        public static Vecf operator *(Vecf L, Size R) => new Vecf
        {
            X = L.X * R.Width,
            Y = L.Y * R.Height
        };
        public static Vecf operator /(Vecf L, Size R) => new Vecf
        {
            X = L.X / (R.Width != 0 ? R.Width : 1),
            Y = L.Y / (R.Height != 0 ? R.Height : 1)
        };
        public static bool operator ==(Vecf L, Size R) => L.X == R.Width && L.Y == R.Height;
        public static bool operator !=(Vecf L, Size R) => L.X != R.Width || L.Y != R.Height;
        public static Size operator +(Size L, Vecf R) => new Size
        {
            Width = (int)(L.Width + R.X),
            Height = (int)(L.Height + R.Y)
        };
        public static Size operator -(Size L, Vecf R) => new Size
        {
            Width = (int)(L.Width - R.X),
            Height = (int)(L.Height - R.Y)
        };
        public static Size operator *(Size L, Vecf R) => new Size
        {
            Width = (int)(L.Width * R.X),
            Height = (int)(L.Height * R.Y)
        };
        public static Size operator /(Size L, Vecf R) => new Size
        {
            Width = (int)(L.Width / (R.X != 0 ? R.X : 1)),
            Height = (int)(L.Height / (R.Y != 0 ? R.Y : 1))
        };
        public static bool operator ==(Size L, Vecf R) => L.Width == R.X && L.Height == R.Y;
        public static bool operator !=(Size L, Vecf R) => L.Width != R.X || L.Height != R.Y;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString() => $"X:{X} Y:{Y}";


        public static readonly Vecf Empty = new Vecf { X = 0, Y = 0 };


        public static Vecf FromPoint(Point target) => new Vecf { X = target.X, Y = target.Y };
        public static Vecf FromSize(Size target) => new Vecf { X = target.Width, Y = target.Height };
        public static Vecf FromSizeF(SizeF target) => new Vecf { X = (float)target.Width, Y = (float)target.Height };
    }
}
