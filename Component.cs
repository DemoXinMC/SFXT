using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT
{
    abstract public class Component
    {
        protected Entity entity;

        public Vector2 OriginOffset { get => new Vector2(this.OriginOffsetUnscaled.X * this.entity.Scale, this.OriginOffsetUnscaled.Y * this.entity.Scale); }
        public Vector2 OriginOffsetUnscaled { get; set; }

        public Component(Entity entity)
        {
            this.entity = entity;
            this.OriginOffsetUnscaled = new SFML.System.Vector2f(0, 0);
        }
    }
}
