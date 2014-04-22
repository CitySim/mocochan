using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moodler.Converter.Data
{
    /// <summary>
    /// Defines a Point with X and Z Values
    /// </summary>
    public class Vector2
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
        /// Creates a new Point and set the Values to Zero
        /// </summary>
        public Vector2()
        {
            this.X = 0.0f;
            this.Y = 0.0f;
        }

        /// <summary>
        /// Creates a new Point
        /// </summary>
        /// <param name="X">The new X Value</param>
        /// <param name="Y">The new Y Value</param>
		public Vector2(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Vector2 Copy()
        {
            return new Vector2(X, Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
                return (Vector2)obj == this;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(
                a.X + b.X,
                a.Y + b.Y
                );
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(
                a.X - b.X,
                a.Y - b.Y
                );
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            if (a.X != b.X ||
                a.Y != b.Y)
                return false;
            return true;
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            if (a.X == b.X &&
                a.Y == b.Y)
                return false;
            return true;
        }

        public static bool operator >=(Vector2 a, Vector2 b)
        {
            if (a.X >= b.X &&
                a.Y >= b.Y)
                return true;
            return false;
        }

        public static bool operator <=(Vector2 a, Vector2 b)
        {
            if (a.X <= b.X &&
                a.Y <= b.Y)
                return true;
            return false;
        }
    }
}
