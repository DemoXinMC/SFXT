using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Util
{
    public static class VertexArrayExtensions
    {
        public static VertexBuffer ToVertexBuffer(this VertexArray array)
        {
            var ret = new VertexBuffer(array.VertexCount, array.PrimitiveType, VertexBuffer.UsageSpecifier.Dynamic);

            ret.Update(array.ToArray());

            return ret;
        }

        public static Vertex[] ToArray(this VertexArray array)
        {
            var ret = new Vertex[array.VertexCount];

            for (uint i = 0; i < array.VertexCount; i++)
                ret[i] = array[i];

            return ret;
        }
    }
}
