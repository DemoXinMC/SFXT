using SFML.Graphics;
using SFXT.Graphics;
using SFXT.Graphics.Texels;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Components.Graphics
{
    public class Tilemap : Graphic
    {
        private ITexels texture;
        private VertexBuffer vbo;

        private uint tileWidth;
        private uint tileHeight;
        private FrameHelper frameHelper;

        private uint[][] tileData;

        public Tilemap(Entity entity, uint tileWidth, uint tileHeight, ITexels texture) : base(entity)
        {
            this.texture = texture;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.frameHelper = new FrameHelper(tileWidth, tileHeight);
            this.vbo = new VertexBuffer(6, PrimitiveType.Triangles, VertexBuffer.UsageSpecifier.Static);
        }

        public void SetTextureData(ITexels texture, uint tileWidth, uint tileHeight, FrameHelper frameHelper = null)
        {
            this.texture = texture;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;

            if (frameHelper != null)
                this.frameHelper = frameHelper;
            else
                this.frameHelper = new FrameHelper(tileWidth, tileHeight);

            this.rebuildVBO();
        }

        public void SetTileData(uint[][] tileIds)
        {
            this.tileData = tileIds;
            this.rebuildVBO();
        }

        public void SetTileData(uint x, uint y, uint tileId)
        {
            if (x >= this.tileData.Length || y >= this.tileData[0].Length)
                return;
            this.tileData[x][y] = tileId;
            this.rebuildVBO();
        }

        public void Redraw()
        {
            this.rebuildVBO();
        }

        private void rebuildVBO()
        {
            uint textureTilesX = texture.Width / tileWidth;
            uint textureTilesY = texture.Height / tileHeight;

            uint textureRow;
            uint textureColumn;

            VertexArray vao = new VertexArray();

            var totalWidth = tileData.Length * tileWidth;
            var totalHeight = tileData[0].Length * tileHeight;
            var center = this.Entity.Position + this.OriginOffset;
            var drawingRoot =  center - (new Vector2(totalWidth / 2, totalHeight / 2) * this.Entity.Scale);

            for(uint i = 0; i < tileData.Length; i++)
            {
                for(uint j = 0; j < tileData[i].Length; j++)
                {
                    var pos = new Vector2(i * tileWidth * this.Entity.Scale, j * tileHeight * this.Entity.Scale) + drawingRoot;

                    var topLeft = new Vector2(pos.X, pos.Y);
                    var topRight = topLeft + new Vector2(tileWidth * this.Entity.Scale, 0);
                    var bottomLeft = topLeft + new Vector2(0, tileHeight * this.Entity.Scale);
                    var bottomRight = topLeft + new Vector2(tileWidth * this.Entity.Scale, tileHeight * this.Entity.Scale);

                    topLeft = topLeft.RotateAround(center, this.Entity.Rotation);
                    topRight = topRight.RotateAround(center, this.Entity.Rotation);
                    bottomLeft = bottomLeft.RotateAround(center, this.Entity.Rotation);
                    bottomRight = bottomRight.RotateAround(center, this.Entity.Rotation);

                    /*
                    var textureTopLeft = new Vector2(textureRow * tileHeight, textureColumn * tileWidth);
                    var textureTopRight = textureTopLeft + new Vector2(tileWidth, 0);
                    var textureBottomLeft = textureTopLeft + new Vector2(0, tileHeight);
                    var textureBottomRight = textureTopLeft + new Vector2(tileWidth, tileHeight);
                    */
                    var tileTexel = this.frameHelper.GetFrameTexel(this.texture, tileData[i][j]);

                    vao.Append(new Vertex(topLeft, tileTexel.TopLeft));
                    vao.Append(new Vertex(topRight, tileTexel.TopRight));
                    vao.Append(new Vertex(bottomRight, tileTexel.BottomRight));

                    vao.Append(new Vertex(topLeft, tileTexel.TopLeft));
                    vao.Append(new Vertex(bottomLeft, tileTexel.BottomLeft));
                    vao.Append(new Vertex(bottomRight, tileTexel.BottomRight));
                }
            }

            this.vbo.Update(vao.ToArray());
        }

        public override void Draw(RenderTarget target, RenderStates renderStates)
        {
            RenderStates state = new RenderStates(renderStates);
            state.Texture = this.texture.Texture;
            target.Draw(this.vbo, state);
        }
    }
}
