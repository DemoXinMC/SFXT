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
        protected Vertex[] vao;

        public Color? Color { get; set; }

        public BasicSprite(Entity entity, ITexels texture) : base(entity)
        {
            this.texture = texture;
            this.vao = new Vertex[6];
            this.Color = null;
        }

        public override void Draw(RenderTarget target, RenderStates renderStates)
        {
            this.updateVAO();
            var state = new RenderStates(renderStates);
            state.Texture = this.texture.Texture;
            target.Draw(this.vao, SFML.Graphics.PrimitiveType.Triangles, state);
        }

        protected void updateVAO()
        {
            var pos = this.Entity.Position + this.OriginOffset;
            var width = this.texture.Width * this.Entity.Scale;
            var height = this.texture.Height * this.Entity.Scale;

            var topLeft = new Vector2((int)pos.X - width / 2, (int)pos.Y - height / 2);
            var bottomRight = new Vector2((int)pos.X + width / 2, (int)pos.Y + height / 2);

            var topRight = new Vector2(bottomRight.X, topLeft.Y);
            var bottomLeft = new Vector2(topLeft.X, bottomRight.Y);

            if (this.Entity.Rotation != 0)
            {
                topLeft = topLeft.RotateAround(pos, this.Entity.Rotation);
                topRight = topRight.RotateAround(pos, this.Entity.Rotation);
                bottomLeft = bottomLeft.RotateAround(pos, this.Entity.Rotation);
                bottomRight = bottomRight.RotateAround(pos, this.Entity.Rotation);
            }

            if (this.FlipHorizontal)
            {
                var temp = topLeft;
                topLeft = topRight;
                topRight = temp;
                temp = bottomLeft;
                bottomLeft = bottomRight;
                bottomRight = temp;
            }

            if(this.FlipVertical)
            {
                var temp = topLeft;
                topLeft = bottomLeft;
                bottomLeft = temp;
                temp = topRight;
                topRight = bottomRight;
                bottomRight = temp;
            }

            if (this.Color == null)
            {
                this.vao[0] = new Vertex(topLeft, this.texture.TopLeft);
                this.vao[2] = new Vertex(topRight, this.texture.TopRight);
                this.vao[1] = new Vertex(bottomRight, this.texture.BottomRight);

                this.vao[3] = new Vertex(topLeft, this.texture.TopLeft);
                this.vao[4] = new Vertex(bottomLeft, this.texture.BottomLeft);
                this.vao[5] = new Vertex(bottomRight, this.texture.BottomRight);
            }
            else
            {
                this.vao[0] = new Vertex(topLeft, this.Color.Value, this.texture.TopLeft);
                this.vao[2] = new Vertex(topRight, this.Color.Value, this.texture.TopRight);
                this.vao[1] = new Vertex(bottomRight, this.Color.Value, this.texture.BottomRight);

                this.vao[3] = new Vertex(topLeft, this.Color.Value, this.texture.TopLeft);
                this.vao[4] = new Vertex(bottomLeft, this.Color.Value, this.texture.BottomLeft);
                this.vao[5] = new Vertex(bottomRight, this.Color.Value, this.texture.BottomRight);
            }
        }

        public RenderStates? BatchRenderStates { get => null; }
        public Texture BatchTexture { get => this.texture.Texture; }
        public Vertex[] BatchVertexes
        {
            get
            {
                this.updateVAO();
                return this.vao;
            }
        }

        public SFML.Graphics.PrimitiveType BatchPrimitiveType => PrimitiveType.Triangles;
    }
}
