﻿using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFXT.Graphics
{
    /// <summary>
    /// SpriteBatch allows for efficient drawing of many Graphics, batching together any of them possible to reduce draw calls to the Graphics API.
    /// </summary>
    public class SpriteBatch
    {
        private List<SFXT.Graphics.Graphic> graphicList;

        /// <summary>
        /// Creates a new SpriteBatch
        /// </summary>
        /// <param name="buffer">The expected number of Graphics to be drawn by this batch.</param>
        public SpriteBatch(uint buffer = 5000)
        {
            this.graphicList = new List<SFXT.Graphics.Graphic>((int)buffer);
        }

        /// <summary>
        /// Adds a Graphic object to be drawn by this SpriteBatch
        /// </summary>
        /// <param name="graphic">The Graphic to enqueue.</param>
        public void Add(Graphic graphic) => this.graphicList.Add(graphic);

        /// <summary>
        /// SFML's Drawable.Draw implementation.  Processes the currently loaded Graphics into the RenderTarget, then clears itself out.
        /// </summary>
        /// <param name="target">A RenderTarget to draw to</param>
        /// <param name="states">The RenderStates that the SpriteBatch will attempt to return to as it draws</param>
        public void Draw(RenderTarget target, RenderStates states)
        {
            RenderStates currentState = states;

            VertexArray drawing = new VertexArray(PrimitiveType.Triangles, (uint)this.graphicList.Count * 6);
            uint drawingIndex = 0;

            foreach(var graphic in graphicList)
            {
                IBatchable batchable = graphic as IBatchable;

                if(batchable == null)
                {
                    target.Draw(drawing, currentState);
                    drawing.Clear();
                    graphic.Draw(target);
                    continue;
                }

                var drawBatch = false;

                RenderStates? batchableState = batchable.BatchRenderStates;

                if(batchableState == null)
                    batchableState = states;

                if(batchableState.Value.Texture != batchable.BatchTexture)
                    drawBatch = true;

                if(batchableState.Value.BlendMode != currentState.BlendMode)
                    drawBatch = true;

                if(batchableState.Value.Shader != currentState.Shader)
                    drawBatch = true;

                if(drawBatch)
                {
                    target.Draw(drawing, currentState);
                    drawing.Clear();
                    drawingIndex = 0;
                    RenderStates? newState = batchable.BatchRenderStates;
                    currentState = newState ?? currentState;
                    currentState.Texture = batchable.BatchTexture;
                }

                var batchVertexes = batchable.BatchVertexes;

                if (batchVertexes.PrimitiveType != drawing.PrimitiveType)
                {
                    target.Draw(drawing, currentState);
                    drawing.Clear();
                    drawingIndex = 0;
                    drawing.PrimitiveType = batchVertexes.PrimitiveType;
                }

                for (uint i = 0; i < batchVertexes.VertexCount; i++)
                    drawing[drawingIndex++] = batchVertexes[i];

                target.Draw(batchVertexes);
            }

            target.Draw(drawing, currentState);
            this.graphicList.Clear();
        }
    }
}