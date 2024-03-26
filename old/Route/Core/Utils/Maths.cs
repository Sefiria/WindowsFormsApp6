using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Core.Utils
{
    static public class Maths
    {
        static public float Epsilon = 0.0001F;
        static public bool FZero(float f)
        {
            return f >= -Epsilon && f < Epsilon;
        }
        static public float Gravity = 0.05F;

        public class Vec
        {
            public float X, Y;

            public Vec()
            {
                X = 0F;
                Y = 0F;
            }
            public Vec(float X = 0F, float Y = 0F)
            {
                this.X = X;
                this.Y = Y;
            }
            public Vec(Vec vec)
            {
                X = vec.X;
                Y = vec.Y;
            }
            public Vec(Point point)
            {
                X = point.X;
                Y = point.Y;
            }

            static public Vec Zero { get; } = new Vec();

            static public Vec operator -(Vec left, float right)
            {
                return new Vec(left.X - right, left.Y - right);
            }
            static public Vec operator -(Vec left, Vec right)
            {
                return new Vec(left.X - right.X, left.Y - right.Y);
            }
            static public Vec operator +(Vec left, float right)
            {
                return new Vec(left.X + right, left.Y + right);
            }
            static public Vec operator +(Vec left, Vec right)
            {
                return new Vec(left.X + right.X, left.Y + right.Y);
            }
            static public Vec operator *(Vec left, float right)
            {
                return new Vec(left.X * right, left.Y * right);
            }
            static public Vec operator *(Vec left, Vec right)
            {
                return new Vec(left.X * right.X, left.Y * right.Y);
            }
            static public Vec operator /(Vec left, float right)
            {
                if (FZero(right))
                    return Zero;
                return new Vec(left.X / right, left.Y / right);
            }
            static public Vec operator /(Vec left, Vec right)
            {
                return new Vec(FZero(right.X) ? 0F : left.X / right.X, FZero(right.Y) ? 0F : left.Y / right.Y);
            }
            static public bool operator ==(Vec left, Vec right)
            {
                if (Equals(left, null) || Equals(right, null))
                    return Equals(left, null) && Equals(right, null);

                return left.X == right.X && left.Y == right.Y;
            }
            static public bool operator !=(Vec left, Vec right)
            {
                return left.X != right.X || left.Y != right.Y;
            }
            /// <summary>
            /// Return if the Vec is Equal to Vec.Zero (by values, not by the instance)
            /// </summary>
            static public bool operator !(Vec vec)
            {
                return vec.X == Zero.X && vec.Y == Zero.Y;
            }

            public override bool Equals(object obj)
            {
                var vec = obj as Vec;
                return vec != null &&
                       X == vec.X &&
                       Y == vec.Y;
            }
            public override int GetHashCode()
            {
                var hashCode = 1861411795;
                hashCode = hashCode * -1521134295 + X.GetHashCode();
                hashCode = hashCode * -1521134295 + Y.GetHashCode();
                return hashCode;
            }

            public float DotProduct(Vec other)
            {
                return X * other.X + Y * other.Y;
            }
            public Vec Distance(Vec End) => new Vec(End.X - X, End.Y - Y);
            public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);
            public float GetMagnitude(Vec End) => new Vec2(this, End).Magnitude;
            public float Direction(Vec End)
            {
                Vec d = Distance(End);
                return FZero(d.X) ? 0F : (float)Math.Atan(d.Y / d.X);
            }
            public Vec GetRotated(float Angle, Vec Origin = null, bool AngleIsInDegrees = true)
            {
                if (Origin == null)
                    Origin = Zero;

                double a = Angle * Math.PI / 180D;
                double sin = Math.Sin(a);
                double cos = Math.Cos(a);

                // Translate point back to origin
                X -= Origin.X;
                Y -= Origin.Y;

                // Rotate point
                double xnew = X * cos - Y * sin;
                double ynew = X * sin + Y * cos;

                // Translate point back
                Vec newPoint = new Vec((int)xnew + Origin.X, (int)ynew + Origin.Y);
                return newPoint;
            }
            public void Rotate(float Angle, Vec Origin = null, bool AngleIsInDegrees = true)
            {
                if (Origin == null)
                    Origin = Zero;

                Vec RotatedVec = GetRotated(Angle, Origin, AngleIsInDegrees);
                X = RotatedVec.X;
                Y = RotatedVec.Y;
            }
            public void SetRotation(float Angle, Vec Origin = null, bool AngleIsInDegrees = true)
            {
                if (Origin == null)
                    Origin = Zero;

                Rotate(Origin.Direction(this) + Angle, Origin, AngleIsInDegrees);
            }
            public Vec GetNormalized()
            {
                float mag = Magnitude;
                return mag == 0F ? Zero : new Vec(X / mag, Y / mag);
            }
            public void Normalize()
            {
                Vec norm = GetNormalized();
                X = norm.X;
                Y = norm.Y;
            }

            public void SetFromPoint(Point point)
            {
                X = point.X;
                Y = point.Y;
            }
            public Point AsPoint => new Point((int)X, (int)Y);
            public PointF AsPointF => new PointF(X, Y);
            internal byte[] ToBytes()
            {
                return Encoding.ASCII.GetBytes(ToString());
            }
        }

        public class Vec2
        {
            public Vec Start, End;

            public Vec2()
            {
                Start = Vec.Zero;
                End = Vec.Zero;
            }
            public Vec2(float length_X, float length_Y)
            {
                Start = Vec.Zero;
                End = new Vec(length_X, length_Y);
            }
            public Vec2(float StartX, float StartY, float EndX, float EndY)
            {
                Start = new Vec(StartX, StartY);
                End = new Vec(EndX, EndY);
            }
            public Vec2(Vec Start, Vec End)
            {
                this.Start = Start;
                this.End = End;
            }
            public Vec2(Vec2 vec2)
            {
                Start = vec2.Start;
                End = vec2.End;
            }

            static public Vec2 Zero { get; } = new Vec2();

            static public Vec2 operator -(Vec2 left, float right)
            {
                return new Vec2(left.Start - right, left.End - right);
            }
            static public Vec2 operator -(Vec2 left, Vec right)
            {
                return new Vec2(left.Start - right, left.End - right);
            }
            static public Vec2 operator -(Vec2 left, Vec2 right)
            {
                return new Vec2(left.Start - right.Start, left.End - right.End);
            }
            static public Vec2 operator +(Vec2 left, float right)
            {
                return new Vec2(left.Start + right, left.End + right);
            }
            static public Vec2 operator +(Vec2 left, Vec right)
            {
                return new Vec2(left.Start + right, left.End + right);
            }
            static public Vec2 operator +(Vec2 left, Vec2 right)
            {
                return new Vec2(left.Start + right.Start, left.End + right.End);
            }
            static public Vec2 operator *(Vec2 left, float right)
            {
                return new Vec2(left.Start * right, left.End * right);
            }
            static public Vec2 operator *(Vec2 left, Vec right)
            {
                return new Vec2(left.Start * right, left.End * right);
            }
            static public Vec2 operator *(Vec2 left, Vec2 right)
            {
                return new Vec2(left.Start * right.Start, left.End * right.End);
            }
            static public Vec2 operator /(Vec2 left, float right)
            {
                if (FZero(right))
                    return Zero;
                return new Vec2(left.Start / right, left.End / right);
            }
            static public Vec2 operator /(Vec2 left, Vec right)
            {
                if (!right)
                    return Zero;
                return new Vec2(left.Start / right, left.End / right);
            }
            static public Vec2 operator /(Vec2 left, Vec2 right)
            {
                return new Vec2(!right.Start ? Vec.Zero : left.Start / right.Start, !right.End ? Vec.Zero : left.End / right.End);
            }
            static public bool operator ==(Vec2 left, Vec2 right)
            {
                return left.Start == right.Start && left.End == right.End;
            }
            static public bool operator !=(Vec2 left, Vec2 right)
            {
                return left.Start != right.Start || left.End != right.End;
            }
            /// <summary>
            /// Return if the Vec2 is Equal to Vec2.Zero (by the values, not by the instance)
            /// </summary>
            /// <param name="vec2"></param>
            /// <returns></returns>
            static public bool operator !(Vec2 vec2)
            {
                return !vec2.Start && !vec2.End;
            }

            public override bool Equals(object obj)
            {
                var vec = obj as Vec2;
                return vec != null &&
                       EqualityComparer<Vec>.Default.Equals(Start, vec.Start) &&
                       EqualityComparer<Vec>.Default.Equals(End, vec.End);
            }
            public override int GetHashCode()
            {
                var hashCode = -1676728671;
                hashCode = hashCode * -1521134295 + EqualityComparer<Vec>.Default.GetHashCode(Start);
                hashCode = hashCode * -1521134295 + EqualityComparer<Vec>.Default.GetHashCode(End);
                return hashCode;
            }

            public float CrossProduct(Vec2 other)
            {
                return Start.X * other.End.Y - End.Y * other.Start.X;
            }
            public Vec Distance => new Vec(End - Start);
            public float Magnitude { get { Vec d = Distance; return (float)Math.Sqrt(d.X * d.X + d.Y * d.Y); } }
            public float Direction { get { Vec d = Distance; return FZero(d.X) ? 0F : (float)Math.Atan(d.Y / d.X); } }
            public Vec2 GetRotated(float Angle, Vec Origin = null, bool AngleIsInDegrees = true)
            {
                return new Vec2(Start.GetRotated(Angle, Origin, AngleIsInDegrees), End.GetRotated(Angle, Origin, AngleIsInDegrees));
            }
            public void Rotate(float Angle, Vec Origin = null, bool AngleIsInDegrees = true)
            {
                Start.Rotate(Angle, Origin, AngleIsInDegrees);
                End.Rotate(Angle, Origin, AngleIsInDegrees);
            }
            public void SetRotation(float Angle, Vec Origin = null, bool AngleIsInDegrees = true)
            {
                Start.Rotate(Origin.Direction(Start) + Angle, Origin, AngleIsInDegrees);
                End.Rotate(Origin.Direction(End) + Angle, Origin, AngleIsInDegrees);
            }
            public Vec2 GetNormalized()
            {
                float mag = Magnitude;
                return new Vec2(Start / mag, End / mag);
            }
            public void Normalize()
            {
                Vec2 norm = GetNormalized();
                Start = norm.Start;
                End = norm.End;
            }
        }

        /// <summary>The Range class.</summary>
        /// <typeparam name="T">Generic parameter.</typeparam>
        public class Range<T> where T : IComparable<T>
        {
            /// <summary>Minimum value of the range.</summary>
            public T Minimum { get; set; }

            /// <summary>Maximum value of the range.</summary>
            public T Maximum { get; set; }

            public Range(T Minimum, T Maximum)
            {
                this.Minimum = Minimum;
                this.Maximum = Maximum;
            }

            /// <summary>Presents the Range in readable format.</summary>
            /// <returns>String representation of the Range</returns>
            public override string ToString()
            {
                return string.Format("[{0}, {1}]", this.Minimum, this.Maximum);
            }

            /// <summary>Determines if the range is valid.</summary>
            /// <returns>True if range is valid, else false</returns>
            public bool IsValid()
            {
                return this.Minimum.CompareTo(this.Maximum) <= 0;
            }

            /// <summary>Determines if the provided value is inside the range.</summary>
            /// <param name="value">The value to test</param>
            /// <returns>True if the value is inside Range, else false</returns>
            public bool ContainsValue(T value)
            {
                return (this.Minimum.CompareTo(value) <= 0) && (value.CompareTo(this.Maximum) <= 0);
            }

            /// <summary>Determines if this Range is inside the bounds of another range.</summary>
            /// <param name="Range">The parent range to test on</param>
            /// <returns>True if range is inclusive, else false</returns>
            public bool IsInsideRange(Range<T> range)
            {
                return this.IsValid() && range.IsValid() && range.ContainsValue(this.Minimum) && range.ContainsValue(this.Maximum);
            }

            /// <summary>Determines if another range is inside the bounds of this range.</summary>
            /// <param name="Range">The child range to test</param>
            /// <returns>True if range is inside, else false</returns>
            public bool ContainsRange(Range<T> range)
            {
                return this.IsValid() && range.IsValid() && this.ContainsValue(range.Minimum) && this.ContainsValue(range.Maximum);
            }
        }


        public static float Distance(Point A, Point B) => (float)Math.Sqrt((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y));
        public static float Distance(Vec A, Vec B) => Distance(A.AsPoint, B.AsPoint);
        public static Vec GetLook(Point A, Point B) => Normalized(new Vec(B) - new Vec(A));
        public static Vec GetLook(Vec A, Vec B) => Normalized(B - A);
        public static Vec Normalized(Vec A)
        {
            float distance = (float)Math.Sqrt(A.X * A.X + A.Y * A.Y);
            if (distance == 0F)
                return Vec.Zero;
            return new Vec(A.X / distance, A.Y / distance);
        }
        public static bool CollisionCircleCircle(Vec APosition, int ARadius, Vec BPosition, int BRadius)
        {
            float distance = Distance(APosition, BPosition);
            float totalRadius = ARadius + BRadius;
            return distance <= totalRadius / 2F;
        }
        public static float Lerp(float A, float B, float t) => (1F - t) * A + t * B;
        public static Vec Lerp2(Vec A, Vec B, float t) => new Vec(Lerp(A.X, B.X, t), Lerp(A.Y, B.Y, t));
        public static int Spline(float t, int x0, int x1, int? x2 = null)
        {
            return (int)(x2 == null ? Lerp(x0, x1, t) :
                (
                    x0 * Math.Pow((1 - t), 3) +
                    x1 * 3 * t * Math.Pow((1 - t), 2) +
                    x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                    x2 * Math.Pow(t, 3))
                );
        }
        public static Vec RotatedDeg(Vec vector, float degrees) => RotatedRad(vector, ToRadians(degrees));
        public static Vec RotatedRad(Vec vector, float radians)
        {
            Vec result = Vec.Zero;
            result.X = (float)(vector.X * Math.Cos(radians) - vector.Y * Math.Sin(radians));
            result.Y = (float)(vector.X * Math.Sin(radians) + vector.Y * Math.Cos(radians));
            return result;
        }
        public static float ToRadians(float degrees) => (float)Math.PI * degrees / 180F;
        public static float ToDegrees(float radians) => radians * 180F / (float)Math.PI;
        public static float AngleVector(Vec A, Vec B) => ToDegrees((float)Math.Atan2(B.Y - A.Y, B.X - A.X));

        #region IsInPolygon
        public static bool IsInPolygon(List<Point> poly, Vec point) => IsInPolygon(poly.ToArray(), point.AsPoint);
        public static bool IsInPolygon(List<Point> poly, Point point) => IsInPolygon(poly.ToArray(), point);
        public static bool IsInPolygon(Point[] poly, Vec point) => IsInPolygon(poly, point.AsPoint);
        public static bool IsInPolygon(List<Vec> poly, Vec point) => IsInPolygon(poly.ToArray(), point.AsPoint);
        public static bool IsInPolygon(List<Vec> poly, Point point) => IsInPolygon(poly.ToArray(), point);
        public static bool IsInPolygon(Vec[] poly, Point point)
        {
            List<Point> points = new List<Point>();
            foreach (Vec p in poly)
                points.Add(p.AsPoint);
            return IsInPolygon(points.ToArray(), point);
        }
        public static bool IsInPolygon(Vec[] poly, Vec point)
        {
            List<Point> points = new List<Point>();
            foreach (Vec p in poly)
                points.Add(p.AsPoint);
            return IsInPolygon(points.ToArray(), point.AsPoint);
        }
        public static bool IsInPolygon(Point[] poly, Point point)
        {
            var coef = poly.Skip(1).Select((p, i) =>
                                            (point.Y - poly[i].Y) * (p.X - poly[i].X)
                                          - (point.X - poly[i].X) * (p.Y - poly[i].Y))
                                    .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }
        #endregion

        #region Collisions
        public static int SidePointSegment(Vec Point, Vec A, Vec B)
        {
            // d = (B.X-A.X) * (Point.Y-A.Y) - (B.Y-A.Y) * (Point.X-A.X)
            // d > 0 : left
            // d < 0 : right
            // d = 0 : on it - think about epsilon, because cannot be exactly 0, close to e = 1000 or 5000

            return (int)((B.X - A.X) * (Point.Y - A.Y) - (B.Y - A.Y) * (Point.X - A.X));
        }
        public static bool LineIntersectsRect(Vec p1, Vec p2, Rectangle r)
        {
            return LineIntersectsLine(p1, p2, new Vec(r.X, r.Y), new Vec(r.X + r.Width, r.Y)) ||
                   LineIntersectsLine(p1, p2, new Vec(r.X + r.Width, r.Y), new Vec(r.X + r.Width, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Vec(r.X + r.Width, r.Y + r.Height), new Vec(r.X, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Vec(r.X, r.Y + r.Height), new Vec(r.X, r.Y)) ||
                   (r.Contains(p1.AsPoint) && r.Contains(p2.AsPoint));
        }
        private static bool LineIntersectsLine(Vec l1p1, Vec l1p2, Vec l2p1, Vec l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}