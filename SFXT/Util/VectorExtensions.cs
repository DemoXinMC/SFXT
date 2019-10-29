using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Util
{
    public static class VectorExtensions
    {
        public static double DistanceTo(this SFML.System.Vector2i obj, SFML.System.Vector2i other)
        {
            var x = other.X - obj.X;
            x *= x;
            var y = other.Y - obj.Y;
            y *= y;
            return Math.Sqrt(x + y);
        }

        public static double DistanceTo(this SFML.System.Vector2u obj, SFML.System.Vector2u other)
        {
            var x = other.X - obj.X;
            x *= x;
            var y = other.Y - obj.Y;
            y *= y;
            return Math.Sqrt(x + y);
        }

        public static double DistanceTo(this SFML.System.Vector2f obj, SFML.System.Vector2f other)
        {
            var x = other.X - obj.X;
            x *= x;
            var y = other.Y - obj.Y;
            y *= y;
            return Math.Sqrt(x + y);
        }

        public static double DistanceTo(this SFML.System.Vector3f obj, SFML.System.Vector3f other)
        {
            var x = other.X - obj.X;
            x *= x;
            var y = other.Y - obj.Y;
            y *= y;
            var z = other.Z - obj.Z;
            z *= z;
            return Math.Sqrt(x + y + z);
        }

        public static double AngleTo(this SFML.System.Vector2f vector, Vector2 other) => Vector2.AngleTo(vector, other);
        public static double AngleTo(this SFML.System.Vector2i vector, Vector2 other) => Vector2.AngleTo(vector, other);
        public static double AngleTo(this SFML.System.Vector2u vector, Vector2 other) => Vector2.AngleTo(vector, other);
        public static Vector2 Normalize(this SFML.System.Vector2f vector) => Vector2.Normalize(vector);
        public static Vector2 Normalize(this SFML.System.Vector2i vector) => Vector2.Normalize(vector);
        public static Vector2 Normalize(this SFML.System.Vector2u vector) => Vector2.Normalize(vector);
        public static Vector2 DirectionTo(this SFML.System.Vector2f vector, Vector2 other) => Vector2.DirectionTo(vector, other);
        public static Vector2 DirectionTo(this SFML.System.Vector2i vector, Vector2 other) => Vector2.DirectionTo(vector, other);
        public static Vector2 DirectionTo(this SFML.System.Vector2u vector, Vector2 other) => Vector2.DirectionTo(vector, other);
    }
}
