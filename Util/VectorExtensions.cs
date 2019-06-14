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
    }
}
