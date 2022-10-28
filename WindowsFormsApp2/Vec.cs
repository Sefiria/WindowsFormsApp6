using System.Drawing;

namespace WindowsFormsApp2
{
    public struct Vec
    {
        public int X, Y;
        public System.Drawing.Point Point => new System.Drawing.Point(X, Y);
        public System.Drawing.Size Size => new System.Drawing.Size(X, Y);
        public static Vec operator +(Vec L, Vec R) => new Vec
        {
            X = L.X + R.X,
            Y = L.Y + R.Y
        };
        public static Vec operator -(Vec L, Vec R) => new Vec
        {
            X = L.X - R.X,
            Y = L.Y - R.Y
        };
        public static Vec operator *(Vec L, Vec R) => new Vec
        {
            X = L.X * R.X,
            Y = L.Y * R.Y
        };
        public static Vec operator /(Vec L, Vec R) => new Vec
        {
            X = L.X / (R.X != 0 ? R.X : 1),
            Y = L.Y / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(Vec L, Vec R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(Vec L, Vec R) => L.X != R.X || L.Y != R.Y;
        public static Vec operator +(Vec L, int V) => new Vec
        {
            X = L.X + V,
            Y = L.Y + V
        };
        public static Vec operator -(Vec L, int V) => new Vec
        {
            X = L.X - V,
            Y = L.Y - V
        };
        public static Vec operator *(Vec L, int V) => new Vec
        {
            X = L.X * V,
            Y = L.Y * V
        };
        public static Vec operator /(Vec L, int V) => new Vec
        {
            X = L.X / (V != 0 ? V : 1),
            Y = L.Y / (V != 0 ? V : 1)
        };
        public static bool operator ==(Vec L, int V) => L.X == V && L.Y == V;
        public static bool operator !=(Vec L, int V) => L.X != V || L.Y != V;
        public static Vec operator +(Vec L, Point R) => new Vec
        {
            X = L.X + R.X,
            Y = L.Y + R.Y
        };
        public static Vec operator -(Vec L, Point R) => new Vec
        {
            X = L.X - R.X,
            Y = L.Y - R.Y
        };
        public static Vec operator *(Vec L, Point R) => new Vec
        {
            X = L.X * R.X,
            Y = L.Y * R.Y
        };
        public static Vec operator /(Vec L, Point R) => new Vec
        {
            X = L.X / (R.X != 0 ? R.X : 1),
            Y = L.Y / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(Vec L, Point R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(Vec L, Point R) => L.X != R.X || L.Y != R.Y;
        public static Vec operator +(Vec L, VecF R) => new Vec
        {
            X = (int)(L.X + R.X),
            Y = (int)(L.Y + R.Y)
        };
        public static Vec operator -(Vec L, VecF R) => new Vec
        {
            X = (int)(L.X - R.X),
            Y = (int)(L.Y - R.Y)
        };
        public static Vec operator *(Vec L, VecF R) => new Vec
        {
            X = (int)(L.X * R.X),
            Y = (int)(L.Y * R.Y)
        };
        public static Vec operator /(Vec L, VecF R) => new Vec
        {
            X = (int)(L.X / (R.X != 0 ? R.X : 1)),
            Y = (int)(L.Y / (R.Y != 0 ? R.Y : 1))
        };
        public static bool operator ==(Vec L, VecF R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(Vec L, VecF R) => L.X != R.X || L.Y != R.Y;
        public static Point operator +(Point L, Vec R) => new Point
        {
            X = L.X + R.X,
            Y = L.Y + R.Y
        };
        public static Point operator -(Point L, Vec R) => new Point
        {
            X = L.X - R.X,
            Y = L.Y - R.Y
        };
        public static Point operator *(Point L, Vec R) => new Point
        {
            X = L.X * R.X,
            Y = L.Y * R.Y
        };
        public static Point operator /(Point L, Vec R) => new Point
        {
            X = L.X / (R.X != 0 ? R.X : 1),
            Y = L.Y / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(Point L, Vec R) => L.X == R.X && L.Y == R.Y;
        public static bool operator !=(Point L, Vec R) => L.X != R.X || L.Y != R.Y;
        public static Vec operator +(Vec L, Size R) => new Vec
        {
            X = L.X + R.Width,
            Y = L.Y + R.Height
        };
        public static Vec operator -(Vec L, Size R) => new Vec
        {
            X = L.X - R.Width,
            Y = L.Y - R.Height
        };
        public static Vec operator *(Vec L, Size R) => new Vec
        {
            X = L.X * R.Width,
            Y = L.Y * R.Height
        };
        public static Vec operator /(Vec L, Size R) => new Vec
        {
            X = L.X / (R.Width != 0 ? R.Width : 1),
            Y = L.Y / (R.Height != 0 ? R.Height : 1)
        };
        public static bool operator ==(Vec L, Size R) => L.X == R.Width && L.Y == R.Height;
        public static bool operator !=(Vec L, Size R) => L.X != R.Width || L.Y != R.Height;
        public static Size operator +(Size L, Vec R) => new Size
        {
            Width = L.Width + R.X,
            Height = L.Height + R.Y
        };
        public static Size operator -(Size L, Vec R) => new Size
        {
            Width = L.Width - R.X,
            Height = L.Height - R.Y
        };
        public static Size operator *(Size L, Vec R) => new Size
        {
            Width = L.Width * R.X,
            Height = L.Height * R.Y
        };
        public static Size operator /(Size L, Vec R) => new Size
        {
            Width = L.Width / (R.X != 0 ? R.X : 1),
            Height = L.Height / (R.Y != 0 ? R.Y : 1)
        };
        public static bool operator ==(Size L, Vec R) => L.Width == R.X && L.Height == R.Y;
        public static bool operator !=(Size L, Vec R) => L.Width != R.X || L.Height != R.Y;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString() => $"X:{X} Y:{Y}";


        public static readonly Vec Empty = new Vec { X = 0, Y = 0 };


        public static Vec FromPoint(Point target) => new Vec { X = target.X, Y = target.Y };
        public static Vec FromSize(Size target) => new Vec { X = target.Width, Y = target.Height };
        public static Vec FromSizeF(SizeF target) => new Vec { X = (int)target.Width, Y = (int)target.Height };
    }
}
