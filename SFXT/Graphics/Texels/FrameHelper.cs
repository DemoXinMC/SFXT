using SFML.System;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Graphics.Texels
{
    public class FrameHelper
    {
        public uint Width { get; protected set; }
        public uint Height { get; protected set; }
        public FrameHelper(uint frameWidth, uint frameHeight)
        {
            this.Width = frameWidth;
            this.Height = frameHeight;
        }

        public virtual TexelsAtlas GetFrameTexel(ITexels texture, uint frameId)
        {
            var framesWide = texture.Width / this.Width;

            var frameRow = frameId / framesWide;
            var frameColumn = frameId % framesWide;

            var frameTopLeft = texture.TopLeft + new Vector2(frameColumn * this.Width, frameRow * this.Height);

            return new TexelsAtlas(texture.Texture, (ushort)frameTopLeft.X, (ushort)frameTopLeft.Y, (ushort)this.Width, (ushort)this.Height);
        }
    }
}
