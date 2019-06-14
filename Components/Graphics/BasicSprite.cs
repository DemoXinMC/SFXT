using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFXT.Graphics;

namespace SFXT.Components.Graphics
{
    public class BasicSprite : Graphic, IBatchable
    {
        private ITexels texture;
        private SFML.Graphics.VertexBuffer vbo;
        private SFML.Graphics.Sprite sfSprite;

        public BasicSprite(Entity entity, ITexels texture) : base(entity)
        {
            this.texture = texture;
            this.vbo = new VertexBuffer(6, PrimitiveType.Triangles, VertexBuffer.UsageSpecifier.Dynamic);
        }

        public override void Draw(RenderTarget target)
        {
            if(this.sfSprite == null)
            {
                this.sfSprite = new SFML.Graphics.Sprite(this.texture.Texture);
                this.sfSprite.Origin = new SFML.System.Vector2f(this.texture.Width / 2, this.texture.Height / 2);
            }

            sfSprite.TextureRect = new IntRect(texture.TopLeft, new SFML.System.Vector2i(texture.Width, texture.Height));
            sfSprite.Position = entity.Position + this.OriginOffset;
            sfSprite.Rotation = entity.Rotation;
            sfSprite.Scale = new SFML.System.Vector2f(entity.Scale, entity.Scale);
            target.Draw(sfSprite);
        }

        public RenderStates? BatchRenderStates { get => null; }
        public Texture BatchTexture { get => this.texture.Texture; }
        public VertexArray BatchVertexes
        {
            get
            {
                var verts = new VertexArray(PrimitiveType.Triangles, 6);
                
                return verts;
            }
        }
    }
}
