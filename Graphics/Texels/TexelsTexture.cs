using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace SFXT.Graphics.Texels
{
    public class TexelsTexture : ITexels
    {
        public TexelsTexture(SFML.Graphics.Texture texture) => this.Texture = texture;

        public Texture Texture { get; protected set; }

        public ushort OffsetX { get => 0; }
        public ushort OffsetY { get => 0; }

        public ushort Width { get => (ushort)this.Texture.Size.X; }
        public ushort Height { get => (ushort)this.Texture.Size.Y; }

        public Vector2i TopLeft { get => new Vector2i(0, 0); }
        public Vector2i TopRight { get => new Vector2i(this.Width, 0); }
        public Vector2i BottomLeft { get => new Vector2i(0, this.Height); }
        public Vector2i BottomRight { get => new Vector2i(this.Width, this.Height); }
    }
}
