using SFML.Graphics;
using SFXT.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Components.Graphics
{
    public class Tilemap : Graphic
    {
        private ITexels texture;
        private VertexBuffer vbo;
        private uint width;
        private uint height;
        private uint tileWidth;
        private uint tileHeight;
        public Tilemap(Entity entity, uint width, uint height, uint tileWidth, uint tileHeight, ITexels texture) : base(entity)
        {
            this.texture = texture;

            uint numTiles = width * height;

            this.width = width;
            this.height = height;

            this.vbo = new VertexBuffer(numTiles * 6, PrimitiveType.Triangles, VertexBuffer.UsageSpecifier.Static);
        }

        public void SetTileData(uint[][] tileIds)
        {
            uint textureTilesX = texture.Width / tileWidth;
            uint textureTilesY = texture.Height / tileHeight;

            uint textureRow;
            uint textureColumn;

            for(uint i = 0; i < tileIds.Length; i++)
            {
                for(uint j = 0; i < tileIds[i].Length; j++)
                {
                    textureRow = tileIds[i][j] / textureTilesX;
                    textureColumn = tileIds[i][j] % textureTilesX;


                }
            }
        }

        public override void Draw(RenderTarget target, RenderStates renderStates)
        {

        }
    }
}
