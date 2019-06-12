using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Graphics
{
    public interface IBatchable
    {
        SFML.Graphics.Texture BatchTexture { get; }
        SFML.Graphics.VertexArray BatchVertexes { get; }
        SFML.Graphics.RenderStates? BatchRenderStates { get; }
    }
}
