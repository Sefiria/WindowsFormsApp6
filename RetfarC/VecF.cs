using System.Drawing;

namespace RetfarC
{
    public struct VecF
    {
        public float X, Y;
        public Point Point => new Point((int)X, (int)Y);
        public Size Size => new Size((int)X, (int)Y);
        public SizeF SizeF => new SizeF(X, Y);
        public static VecF operator +(VecF L, VecF R) => new VecF
        {
            X = L.X + R.X,
            Y = L.Y + R.Y
        };
        public static VecF operator -(VecF L, VecF R) => new VecF
        {
            X = L.X - R.X,
            Y = L.Y - R.Y
        };
        public static VecF operator *(VecF L, VecF R) => new VecF
        {
            X = L.X * R.X,
            Y = L.Y * R.Y
        };
        public static VecF operator /(VecF L, VecF R) => new VecF
        {
            X = L.X / (R.X != 0 ? R.X : 1),
            Y = L.Y / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(VecF L, VecF R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(VecF L, VecF R) => L.X != R.X || L.Y != R.Y;
        public static VecF operator +(VecF L,float V) => new VecF
        {
            X = L.X + V,
            Y = L.Y + V
        };
        public static VecF operator -(VecF L, float V) => new VecF
        {
            X = L.X - V,
            Y = L.Y - V
        };
        public static VecF operator *(VecF L, float V) => new VecF
        {
            X = L.X * V,
            Y = L.Y * V
        };
        public static VecF operator /(VecF L, float V) => new VecF
        {
            X = L.X / (V != 0 ? V : 1),
            Y = L.Y / (V != 0 ? V : 1)
        };
        public static bool operator ==(VecF L, float V) => L.X == V && L.Y == V;
        public static bool operator !=(VecF L, float V) => L.X != V || L.Y != V;
        public static VecF operator +(VecF L, Point R) => new VecF
        {
            X = L.X + R.X,
            Y = L.Y + R.Y
        };
        public static VecF operator -(VecF L, Point R) => new VecF
        {
            X = L.X - R.X,
            Y = L.Y - R.Y
        };
        public static VecF operator *(VecF L, Point R) => new VecF
        {
            X = L.X * R.X,
            Y = L.Y * R.Y
        };
        public static VecF operator /(VecF L, Point R) => new VecF
        {
            X = L.X / (R.X != 0 ? R.X : 1),
            Y = L.Y / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(VecF L, Point R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(VecF L, Point R) => L.X != R.X || L.Y != R.Y;
        public static VecF operator +(VecF L, Vec R) => new VecF
        {
            X = L.X + R.X,
            Y = L.Y + R.Y
        };
        public static VecF operator -(VecF L, Vec R) => new VecF
        {
            X = L.X - R.X,
            Y = L.Y - R.Y
        };
        public static VecF operator *(VecF L, Vec R) => new VecF
        {
            X = L.X * R.X,
            Y = L.Y * R.Y
        };
        public static VecF operator /(VecF L, Vec R) => new VecF
        {
            X = L.X / (R.X != 0 ? R.X : 1),
            Y = L.Y / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(VecF L, Vec R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(VecF L, Vec R) => L.X != R.X || L.Y != R.Y;
        public static Point operator +(Point L, VecF R) => new Point
        {
            X = (int)(L.X + R.X),
            Y = (int)(L.Y + R.Y)
        };
        public static Point operator -(Point L, VecF R) => new Point
        {
            X = (int)(L.X - R.X),
            Y = (int)(L.Y - R.Y)
        };
        public static Point operator *(Point L, VecF R) => new Point
        {
            X = (int)(L.X * R.X),
            Y = (int)(L.Y * R.Y)
        };
        public static Point operator /(Point L, VecF R) => new Point
        {
            X = (int)(L.X / (R.X != 0 ? R.X : 1)),
            Y = (int)(L.Y / (R.Y != 0 ? R.Y : 1))
        };
        public static bool operator ==(Point L, VecF R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(Point L, VecF R) => L.X != R.X || L.Y != R.Y;
        public static VecF operator +(VecF L, Size R) => new VecF
        {
            X = L.X + R.Width,
            Y = L.Y + R.Height
        };
        public static VecF operator -(VecF L, Size R) => new VecF
        {
            X = L.X - R.Width,
            Y = L.Y - R.Height
        };
        public static VecF operator *(VecF L, Size R) => new VecF
        {
            X = L.X * R.Width,
            Y = L.Y * R.Height
        };
        public static VecF operator /(VecF L, Size R) => new VecF
        {
            X = L.X / (R.Width != 0 ? R.Width : 1),
            Y = L.Y / (R.Height != 0 ? R.Height : 1)
        };
        public static bool operator ==(VecF L, Size R) => L.X == R.Width && L.Y == R.Height;
        public static bool operator !=(VecF L, Size R) => L.X != R.Width || L.Y != R.Height;
        public static Size operator +(Size L, VecF R) => new Size
        {
            Width = (int)(L.Width + R.X),
            Height = (int)(L.Height + R.Y)
        };
        public static Size operator -(Size L, VecF R) => new Size
        {
            Width = (int)(L.Width - R.X),
            Height = (int)(L.Height - R.Y)
        };
        public static Size operator *(Size L, VecF R) => new Size
        {
            Width = (int)(L.Width * R.X),
            Height = (int)(L.Height * R.Y)
        };
        public static Size operator /(Size L, VecF R) => new Size
        {
            Width = (int)(L.Width / (R.X != 0 ? R.X : 1)),
            Height = (int)(L.Height / (R.Y != 0 ? R.Y : 1))
        };
        public static bool operator ==(Size L, VecF R) => L.Width == R.X && L.Height == R.Y;
        public static bool operator !=(Size L, VecF R) => L.Width != R.X || L.Height != R.Y;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString() => $"X:{X} Y:{Y}";


        public static readonly VecF Empty = new VecF { X = 0, Y = 0 };


        public static VecF FromPoint(Point target) => new VecF { X = target.X, Y = target.Y };
        public static VecF FromSize(Size target) => new VecF { X = target.Width, Y = target.Height };
        public static VecF FromSizeF(SizeF target) => new VecF { X = (float)target.Width, Y = (float)target.Height };
    }
}
