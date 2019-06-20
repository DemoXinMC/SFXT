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
        SFML.System.Vector2f TopLeft { get; }
        SFML.System.Vector2f TopRight { get; }
        SFML.System.Vector2f BottomLeft { get; }
        SFML.System.Vector2f BottomRight { get; }
    }
}
