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

        public Vector2f TopLeft { get => new Vector2f(0, 0); }
        public Vector2f TopRight { get => new Vector2f(this.Width, 0); }
        public Vector2f BottomLeft { get => new Vector2f(0, this.Height); }
        public Vector2f BottomRight { get => new Vector2f(this.Width, this.Height); }
    }
}
