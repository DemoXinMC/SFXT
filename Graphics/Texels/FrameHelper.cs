using SFML.System;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Graphics.Texels
{
    public class FrameHelper
    {
        private uint frameWidth;
        private uint frameHeight;
        public FrameHelper(uint frameWidth, uint frameHeight)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
        }

        public virtual TexelsAtlas GetFrameTexel(ITexels texture, uint frameId)
        {
            var framesWide = texture.Width / frameWidth;

            var frameRow = frameId / framesWide;
            var frameColumn = frameId % framesWide;

            var frameTopLeft = texture.TopLeft + new Vector2(frameRow * frameHeight, frameColumn * frameWidth);

            return new TexelsAtlas(texture.Texture, (ushort)frameTopLeft.X, (ushort)frameTopLeft.Y, (ushort)frameWidth, (ushort)frameHeight);
        }
    }
}
