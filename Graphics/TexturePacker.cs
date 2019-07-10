using SFML.Graphics;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Graphics
{
    public class TexturePacker
    {
        private List<Texture> finishedTextures;
        public Texture WorkingTexture { get; private set; }

        private uint rowHeight;
        private Vector2 nextOffset;
        private uint size;

        public TexturePacker(uint size)
        {
            this.size = size;
            finishedTextures = new List<Texture>();
            WorkingTexture = new Texture(this.size, this.size);
            rowHeight = 0;
            nextOffset = new Vector2(0, 0);
        }

        public TexturePacker() : this(Texture.MaximumSize) { }

        public ITexels PackTexture(Texture texture)
        {
            if (this.nextOffset.X + texture.Size.X >= this.WorkingTexture.Size.X)
            {
                this.nextOffset = new Vector2(0, this.nextOffset.Y + rowHeight);
                rowHeight = 0;
            }

            if (this.nextOffset.Y + texture.Size.Y >= this.WorkingTexture.Size.Y)
                this.finishTexture();

            var ret = new TexelsAtlas(this.WorkingTexture, (ushort)this.nextOffset.X, (ushort)this.nextOffset.Y, (ushort)texture.Size.X, (ushort)texture.Size.Y);

            this.WorkingTexture.Update(texture.CopyToImage(), (uint)this.nextOffset.X, (uint)this.nextOffset.Y);

            this.nextOffset.X += texture.Size.X;

            this.rowHeight = Math.Max(this.rowHeight, texture.Size.Y);

            return ret;
        }

        public ITexels PackTexels(ITexels texels)
        {
            var ret = new TexelsAtlas(WorkingTexture, 0, 0, texels.Width, texels.Height);
            return ret;
        }

        private void finishTexture()
        {
            Console.WriteLine("Baking Texture");
            this.finishedTextures.Add(this.WorkingTexture);
            this.WorkingTexture = new Texture(this.size, this.size);
            this.nextOffset = new Vector2(0, 0);
        }
    }
}
