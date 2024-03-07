using System;
using System.Numerics;

namespace Voxels {
    /// <summary>
    /// Represents a value with X,Y and Z floateger values.
    /// This can represent an floateger based 3D coordinate or an area of 3D space.
    /// NOTE: XY is the horizontal plane and Z is the vertical axis.
    /// </summary>
    public struct XYZf : IEquatable<XYZf> {
        public float X;
        public float Y;
        public float Z;

        public XYZf(float x, float y, float z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public float MaxDimension {
            get { return Math.Max(X, Math.Max(Y, Z)); }
        }

        public float Volume {
            get { return X * Y * Z; }
        }

        public static readonly XYZf Zero = new XYZf(0, 0, 0);
        public static readonly XYZf One = new XYZf(1,1,1);
        public static readonly XYZf OneX = new XYZf(1, 0, 0);
        public static readonly XYZf OneY = new XYZf(0, 1, 0);
        public static readonly XYZf OneZ = new XYZf(0, 0, 1);
        public static readonly XYZf OneXY = new XYZf(1, 1, 0);
        public static readonly XYZf OneXZ = new XYZf(1, 0, 1);
        public static readonly XYZf OneYZ = new XYZf(0, 1, 1);

        public static readonly XYZf Min = new XYZf(float.MinValue, float.MinValue, float.MinValue);
        public static readonly XYZf Max = new XYZf(float.MaxValue, float.MaxValue, float.MaxValue);

        public static XYZf operator *(XYZf a, float b) {
            return new XYZf(a.X * b, a.Y * b, a.Z * b);
        }
        public static XYZf operator /(XYZf a, float b) {
            return new XYZf(a.X / b, a.Y / b, a.Z / b);
        }
        public static XYZf operator %(XYZf a, float b) {
            return new XYZf(a.X % b, a.Y % b, a.Z % b);
        }
        public static XYZf operator +(XYZf a, XYZf b) {
            return new XYZf(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static XYZf operator -(XYZf a, XYZf b) {
            return new XYZf(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static XYZf operator -(XYZf a) {
            return new XYZf(-a.X, -a.Y, -a.Z);
        }

        public static bool operator ==(XYZf a, XYZf b) {
            return a.Equals(b);
        }

        public static bool operator !=(XYZf a, XYZf b) {
            return !a.Equals(b);
        }

        public bool Equals(XYZf other) {
            return this.X == other.X
                && this.Y == other.Y
                && this.Z == other.Z;
        }

        public override bool Equals(object other) {
            return Equals((XYZf) other);
        }

        public override int GetHashCode() {
            return ((int)(X * 100) << 16) ^ ((int)(Y * 100) << 8) ^ (int)(Z * 100);
        }

        public override string ToString() {
            return string.Format("{0}|{1}|{2}", X, Y, Z);
        }

        public XYZf Transform(Matrix4x4 matrix) {
            return XYZf.FromVector3(Vector3.Transform(ToVector3(), matrix));
        }


        public XYZ ToXYZ()
        {
            return new XYZ((int)X, (int)Y, (int)Z);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public static XYZf FromVector3(Vector3 v) {
            return new XYZf((float)v.X, (float)v.Y, (float)v.Z);
        }
    }

    public static class XYZfExt
    {
        public static XYZf ToXYZf(this Vector3 v) => new XYZf(v.X, v.Y, v.Z);
    }
}
