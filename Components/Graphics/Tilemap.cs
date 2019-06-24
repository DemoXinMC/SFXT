using SFML.Graphics;
using SFXT.Graphics;
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

        private uint[][] tileData;

        public Tilemap(Entity entity, uint tileWidth, uint tileHeight, ITexels texture) : base(entity)
        {
            this.texture = texture;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.vbo = new VertexBuffer(6, PrimitiveType.Triangles, VertexBuffer.UsageSpecifier.Static);
        }

        public void SetTextureData(ITexels texture, uint tileWidth, uint tileHeight)
        {
            this.texture = texture;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.rebuildVBO();
        }

        public void SetTileData(uint[][] tileIds)
        {
            this.tileData = tileIds;
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
            var drawingRoot = (this.Entity.Position + this.OriginOffset) - new Vector2(totalWidth / 2, totalHeight / 2);

            for(uint i = 0; i < tileData.Length; i++)
            {
                for(uint j = 0; j < tileData[i].Length; j++)
                {
                    textureRow = tileData[i][j] / textureTilesX;
                    textureColumn = tileData[i][j] % textureTilesX;

                    var pos = new Vector2(i * tileWidth, j * tileHeight) + drawingRoot;

                    var topLeft = new Vector2(pos.X, pos.Y);
                    var topRight = topLeft + new Vector2(tileWidth, 0);
                    var bottomLeft = topLeft + new Vector2(0, tileHeight);
                    var bottomRight = topLeft + new Vector2(tileWidth, tileHeight);

                    var textureTopLeft = new Vector2(textureRow * tileHeight, textureColumn * tileWidth);
                    var textureTopRight = textureTopLeft + new Vector2(tileWidth, 0);
                    var textureBottomLeft = textureTopLeft + new Vector2(0, tileHeight);
                    var textureBottomRight = textureTopLeft + new Vector2(tileWidth, tileHeight);

                    vao.Append(new Vertex(topLeft, textureTopLeft));
                    vao.Append(new Vertex(topRight, textureTopRight));
                    vao.Append(new Vertex(bottomRight, textureBottomRight));

                    vao.Append(new Vertex(topLeft, textureTopLeft));
                    vao.Append(new Vertex(bottomLeft, textureBottomLeft));
                    vao.Append(new Vertex(bottomRight, textureBottomRight));
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
