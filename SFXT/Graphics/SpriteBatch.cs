using SFML.Graphics;
using SFXT.Components.Graphics;
using SFXT.Util;
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
        private List<Graphic> graphicList;

        /// <summary>
        /// Creates a new SpriteBatch
        /// </summary>
        /// <param name="buffer">The expected number of Graphics to be drawn by this batch.</param>
        public SpriteBatch(uint buffer = 1000)
        {
            this.graphicList = new List<Graphic>((int)buffer);
        }

        /// <summary>
        /// Adds a Graphic object to be drawn by this SpriteBatch
        /// </summary>
        /// <param name="graphic">The Graphic to enqueue.</param>
        public void Add(Graphic graphic) => this.graphicList.Add(graphic);

        public void Clear() => this.graphicList.Clear();

        /// <summary>
        /// SFML's Drawable.Draw implementation.  Processes the currently loaded Graphics into the RenderTarget, then clears itself out.
        /// </summary>
        /// <param name="target">A RenderTarget to draw to</param>
        /// <param name="states">The RenderStates that the SpriteBatch will attempt to return to as it draws</param>
        public void Draw(RenderTarget target, RenderStates states, bool clearGraphicList = true)
        {
            RenderStates currentState = new RenderStates(states);

            List<Vertex> drawing = new List<Vertex>(this.graphicList.Count * 6);
            PrimitiveType drawingType = PrimitiveType.Triangles;

            foreach(var graphic in graphicList)
            {
                IBatchable batchable = graphic as IBatchable;

                if(batchable == null)
                {
                    Debug.DrawCalls++;
                    target.Draw(drawing.ToArray(), drawingType, currentState);
                    drawing.Clear();
                    Debug.DrawCalls++;
                    graphic.Draw(target, states);
                    continue;
                }

                var drawBatch = false;

                RenderStates? batchableState = batchable.BatchRenderStates;

                if(batchableState == null)
                    batchableState = currentState;

                if(batchable.BatchTexture != currentState.Texture)
                    drawBatch = true;

                if(batchableState.Value.BlendMode != currentState.BlendMode)
                    drawBatch = true;

                if(batchableState.Value.Shader != currentState.Shader)
                    drawBatch = true;

                if (batchable.BatchPrimitiveType != drawingType)
                    drawBatch = true;

                if(drawBatch)
                {
                    Debug.DrawCalls++;
                    target.Draw(drawing.ToArray(), drawingType, currentState);
                    drawing.Clear();

                    if (batchable.BatchRenderStates != null)
                    {
                        currentState.BlendMode = batchableState.Value.BlendMode;
                        currentState.Shader = batchableState.Value.Shader;
                    }
                    currentState.Texture = batchable.BatchTexture;
                    drawingType = batchable.BatchPrimitiveType;
                }

                drawing.AddRange(batchable.BatchVertexes);
            }

            Debug.DrawCalls++;
            target.Draw(drawing.ToArray(), drawingType, currentState);
            if(clearGraphicList)
                this.graphicList.Clear();
        }
    }
}
