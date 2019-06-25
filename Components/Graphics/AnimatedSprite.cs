using SFML.Graphics;
using SFXT.Graphics;
using SFXT.Graphics.Texels;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SFXT.Components.Graphics
{
    public class AnimatedSprite : Graphic
    {
        protected ITexels texture;
        protected VertexArray vao;

        protected FrameHelper frameHelper;

        public SFML.Graphics.Color? Color { get; set; }

        public AnimatedSprite(Entity entity) : base(entity)
        {
            this.texture = null;
            this.vao = null;
            this.Color = null;
        }

        public AnimatedSprite(Entity entity, ITexels texture, uint frameWidth, uint frameHeight) : base(entity)
        {
            this.texture = texture;
            this.vao = new VertexArray(PrimitiveType.Triangles, 6);
            this.Color = null;
            this.frameHelper = new FrameHelper(frameWidth, frameHeight);
        }

        public void SetTextureData(ITexels texture, uint frameWidth, uint frameHeight, FrameHelper frameHelper = null)
        {
            this.texture = texture;

            if (frameHelper != null)
                this.frameHelper = frameHelper;
            else
                this.frameHelper = new FrameHelper(frameWidth, frameHeight);
        }

        public uint GetCurrentFrame()
        {
            // TODO: implement
            return 0;
        }

        public override void Draw(RenderTarget target, RenderStates renderStates)
        {
            this.updateVAO();
            var state = new RenderStates(renderStates);
            state.Texture = this.texture.Texture;
            target.Draw(this.vao, state);
        }

        protected void updateVAO()
        {
            var pos = this.Entity.Position + this.OriginOffset;
            var width = this.texture.Width * this.Entity.Scale;
            var height = this.texture.Height * this.Entity.Scale;

            var topLeft = new Vector2((int)pos.X - width / 2, (int)pos.Y - height / 2).RotateAround(pos, this.Entity.Rotation);
            var topRight = new Vector2((int)pos.X + width / 2, (int)pos.Y - height / 2).RotateAround(pos, this.Entity.Rotation);
            var bottomLeft = new Vector2((int)pos.X - width / 2, (int)pos.Y + height / 2).RotateAround(pos, this.Entity.Rotation);
            var bottomRight = new Vector2((int)pos.X + width / 2, (int)pos.Y + height / 2).RotateAround(pos, this.Entity.Rotation);

            if (this.FlipHorizontal)
            {
                var temp = topLeft;
                topLeft = topRight;
                topRight = temp;
                temp = bottomLeft;
                bottomLeft = bottomRight;
                bottomRight = temp;
            }

            if (this.FlipVertical)
            {
                var temp = topLeft;
                topLeft = bottomLeft;
                bottomLeft = temp;
                temp = topRight;
                topRight = bottomRight;
                bottomRight = temp;
            }

            var frameTexel = this.frameHelper.GetFrameTexel(this.texture, this.GetCurrentFrame());

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
