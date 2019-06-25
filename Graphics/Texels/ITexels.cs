using SFXT.Util;
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
        Vector2 TopLeft { get; }
        Vector2 TopRight { get; }
        Vector2 BottomLeft { get; }
        Vector2 BottomRight { get; }
    }
}
