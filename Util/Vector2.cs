using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Util
{
    public class Vector2
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }

        public Vector2(double x, double Y)
        {
            this.X = x;
            this.Y = Y;
        }

        public static Vector2 FromVector2f(Vector2f other) => new Vector2(other.X, other.Y);
        public static implicit operator Vector2(Vector2f other) => new Vector2(other.X, other.Y);
        public static Vector2f ToVector2f(Vector2 other) => new Vector2f((float)other.X, (float)other.Y);
        public static implicit operator Vector2f(Vector2 other) => new Vector2f((float)other.X, (float)other.Y);

        public static Vector2 FromVector2i(Vector2i other) => new Vector2(other.X, other.Y);
        public static implicit operator Vector2(Vector2i other) => new Vector2(other.X, other.Y);
        public static Vector2i ToVector2i(Vector2 other) => new Vector2i((int)other.X, (int)other.Y);
        public static implicit operator Vector2i(Vector2 other) => new Vector2i((int)other.X, (int)other.Y);

        public static Vector2 FromVector2u(Vector2u other) => new Vector2(other.X, other.Y);
        public static implicit operator Vector2(Vector2u other) => new Vector2(other.X, other.Y);
        public static Vector2u ToVector2u(Vector2 other) => new Vector2u((uint)other.X, (uint)other.Y);
        public static implicit operator Vector2u(Vector2 other) => new Vector2u((uint)other.X, (uint)other.Y);

        public static double AngleTo(Vector2 vector, Vector2 other) => vector.AngleTo(other);

        public double AngleTo(Vector2 other)
        {
            double angle = Math.Atan2(this.Y - other.Y, this.X - other.X);
            angle *= 180f / Math.PI;

            if (angle < 0)
                angle = 360.0 - (-angle);

            return angle;
        }

        public static Vector2 Normalize(Vector2 vector) => vector.Normalize();

        public Vector2 Normalize()
        {
            var length = Math.Sqrt(this.X * this.X + this.Y * this.Y);
            this.X /= length;
            this.Y /= length;
            return this;
        }

        public static Vector2 DirectionTo(Vector2 vector, Vector2 other) => vector.DirectionTo(other);

        public Vector2 DirectionTo(Vector2 other)
        {
            var x = this.X - other.X;
            var y = this.Y - other.Y;
            return Vector2.Normalize(new Vector2(x, y));
        }

        public Vector2 RotateAround(Vector2 origin, double angle)
        {
            angle = (angle) * (Math.PI / 180);

            var rotatedX = Math.Cos(angle) * (this.X - origin.X) - Math.Sin(angle) * (this.Y - origin.Y) + origin.X;
            var rotatedY = Math.Sin(angle) * (this.X - origin.X) + Math.Cos(angle) * (this.Y - origin.Y) + origin.Y;

            return new Vector2(rotatedX, rotatedY);
        }
    }
}
