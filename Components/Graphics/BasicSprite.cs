﻿using System;
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

        public Color Color { get; set; }

        public BasicSprite(Entity entity, ITexels texture) : base(entity)
        {
            this.texture = texture;
            this.vao = new VertexArray(PrimitiveType.Triangles, 6);
            //this.Color = Color.Red;
        }

        public override void Draw(RenderTarget target)
        {
            this.updateVAO();
            target.Draw(this.vao, new RenderStates(this.texture.Texture));
        }

        protected void updateVAO()
        {
            var pos = this.entity.Position + this.OriginOffset;
            var width = this.texture.Width * this.entity.Scale;
            var height = this.texture.Height * this.entity.Scale;

            var topLeft = new Vector2((int)pos.X - width / 2, (int)pos.Y - height / 2).RotateAround(pos, this.entity.Rotation);
            var topRight = new Vector2((int)pos.X + width / 2, (int)pos.Y - height / 2).RotateAround(pos, this.entity.Rotation);
            var bottomLeft = new Vector2((int)pos.X - width / 2, (int)pos.Y + height / 2).RotateAround(pos, this.entity.Rotation);
            var bottomRight = new Vector2((int)pos.X + width / 2, (int)pos.Y + height / 2).RotateAround(pos, this.entity.Rotation);

            if(this.FlipHorizontal)
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

            this.vao[0] = new Vertex(topLeft, this.texture.TopLeft);
            this.vao[2] = new Vertex(topRight,this.texture.TopRight);
            this.vao[1] = new Vertex(bottomRight, this.texture.BottomRight);

            this.vao[3] = new Vertex(topLeft, this.texture.TopLeft);
            this.vao[4] = new Vertex(bottomLeft, this.texture.BottomLeft);
            this.vao[5] = new Vertex(bottomRight, this.texture.BottomRight);
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
