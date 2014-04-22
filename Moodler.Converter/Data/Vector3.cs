using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moodler.Converter.Data
{
    /// <summary>
    /// Defines a Point with X, Y and Z Values
    /// </summary>
    public class Vector3
    {
        /// <summary>
        /// Gets or Sets the X Value
        /// </summary>
        public double X;

        /// <summary>
        /// Gets or Sets the Y Value
        /// </summary>
		public double Y;

        /// <summary>
        /// Gets or Sets the Z Value
        /// </summary>
		public double Z;

        /// <summary>
        /// Creates a new Point and set the Values to Zero
        /// </summary>
        public Vector3()
        {
            this.X = 0.0f;
            this.Y = 0.0f;
            this.Z = 0.0f;
        }

        /// <summary>
        /// Add a Z Value to a <see cref="Vector2"/>
        /// </summary>
        /// <param name="oldPoint">The Point witch schpuld be used</param>
        /// <param name="Z">The new Z Value</param>
        public Vector3(Vector2 oldPoint, float Z)
        {
            this.X = oldPoint.X;
            this.Y = oldPoint.Y;
            this.Z = Z;
        }

        /// <summary>
        /// Creates a new Point
        /// </summary>
        /// <param name="X">The new X Value</param>
        /// <param name="Y">The new Y Value</param>
        /// <param name="Z">The new Z Value</param>
		public Vector3(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Vector3 Copy()
        {
            return new Vector3(X, Y, Z);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3)
                return (Vector3)obj == this;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z
                );
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X - b.X,
                a.Y - b.Y,
                a.Z - b.Z
                );
        }

		public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(
                a.X * b,
                a.Y * b,
                a.Z * b
                );
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            if (a.X != b.X ||
                a.Y != b.Y ||
                a.Z != b.Z)
                return false;
            return true;
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            if (a.X == b.X &&
                a.Y == b.Y &&
                a.Z == b.Z)
                return false;
            return true;
        }

        public static bool operator >=(Vector3 a, Vector3 b)
        {
            if (a.X >= b.X &&
                a.Y >= b.Y &&
                a.Z >= b.Z)
                return true;
            return false;
        }

        public static bool operator <=(Vector3 a, Vector3 b)
        {
            if (a.X <= b.X &&
                a.Y <= b.Y &&
                a.Z <= b.Z)
                return true;
            return false;
        }
    }
}
