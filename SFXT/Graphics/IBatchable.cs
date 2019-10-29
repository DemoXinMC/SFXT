using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Graphics
{
    public interface IBatchable
    {
        SFML.Graphics.Texture BatchTexture { get; }
        SFML.Graphics.Vertex[] BatchVertexes { get; }
        SFML.Graphics.PrimitiveType BatchPrimitiveType { get; }
        SFML.Graphics.RenderStates? BatchRenderStates { get; }
    }
}
