using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Graphics
{
    public interface ITexels
    {
        SFML.Graphics.Texture Texture { get; }
        ushort OffsetX { get; }
        ushort OffsetY { get; }
        ushort Width { get; }
        ushort Height { get; }
        SFML.System.Vector2i TopLeft { get; }
        SFML.System.Vector2i TopRight { get; }
        SFML.System.Vector2i BottomLeft { get; }
        SFML.System.Vector2i BottomRight { get; }
    }
}
