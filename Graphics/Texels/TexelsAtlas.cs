using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace SFXT.Graphics
{
    public class TexelsAtlas : ITexels
    {
        public TexelsAtlas(SFML.Graphics.Texture atlas, ushort offsetX, ushort offsetY, ushort width, ushort height)
        {
            this.Texture = atlas;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            this.Height = height;
            this.Width = width;
        }

        public Texture Texture { get; protected set; }

        public ushort OffsetX { get; protected set; }
        public ushort OffsetY { get; protected set; }

        public ushort Width { get; protected set; }
        public ushort Height { get; protected set; }

        public Vector2f TopLeft { get => new SFML.System.Vector2f(this.OffsetX, this.OffsetY); }
        public Vector2f TopRight { get => new SFML.System.Vector2f(this.OffsetX + this.Width, this.OffsetY); }
        public Vector2f BottomLeft { get => new SFML.System.Vector2f(this.OffsetX, this.OffsetY + this.Height); }
        public Vector2f BottomRight { get => new SFML.System.Vector2f(this.OffsetX + this.Width, this.OffsetY + this.Height); }
    }
}
