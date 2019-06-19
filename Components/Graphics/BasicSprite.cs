using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFXT.Graphics;
using SFXT.Util;

namespace SFXT.Components.Graphics
{
    public class BasicSprite : Graphic, IBatchable
    {
        protected ITexels texture;
        protected VertexArray vao;

        public Color Color{ get; set; }
        /*
        private SFML.Graphics.VertexBuffer vbo;
        private SFML.Graphics.Sprite sfSprite;
        */

        public BasicSprite(Entity entity, ITexels texture) : base(entity)
        {
            this.texture = texture;
            this.vao = new VertexArray(PrimitiveType.Triangles, 6);
        }

        public override void Draw(RenderTarget target)
        {
            this.updateVAO();
            target.Draw(this.vao);
            /*
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
            */
        }

        protected void updateVAO()
        {
            var pos = this.entity.Position + this.OriginOffset;
            var width = this.texture.Width * this.entity.Scale;
            var height = this.texture.Height * this.entity.Scale;

            var rect = new RectangleShape(new Vector2f(width, height));
            rect.Origin = new Vector2f(width / 2, height / 2);
            rect.Position = pos;

            rect.Rotation = this.entity.Rotation;

            /*
            var topLeft = new Vector2f((int)pos.X - width / 2, (int)pos.Y - height / 2);
            var topRight = new Vector2f((int)pos.X + width / 2, (int)pos.Y - height / 2);
            var bottomLeft = new Vector2f((int)pos.X - width / 2, (int)pos.Y + height / 2);
            var bottomRight = new Vector2f((int)pos.X + width / 2, (int)pos.Y + height / 2);
            */

            this.vao[0] = new Vertex(rect.GetPoint(0), this.Color, (Vector2)this.texture.TopLeft);
            this.vao[1] = new Vertex(rect.GetPoint(1), this.Color, (Vector2)this.texture.TopRight);
            this.vao[2] = new Vertex(rect.GetPoint(2), this.Color, (Vector2)this.texture.BottomRight);

            this.vao[3] = new Vertex(rect.GetPoint(0), this.Color, (Vector2)this.texture.TopLeft);
            this.vao[4] = new Vertex(rect.GetPoint(3), this.Color, (Vector2)this.texture.BottomLeft);
            this.vao[5] = new Vertex(rect.GetPoint(2), this.Color, (Vector2)this.texture.BottomRight);
        }

        public RenderStates? BatchRenderStates { get => null; }
        public Texture BatchTexture { get => this.texture.Texture; }
        public VertexArray BatchVertexes
        {
            get
            {
                this.updateVAO();
                return this.vao;
            }
        }
    }
}
