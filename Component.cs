using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT
{
    abstract public class Component
    {
        protected Entity entity;

        public SFML.System.Vector2f OriginOffset { get => new SFML.System.Vector2f(this.OriginOffsetUnscaled.X * this.entity.Scale, this.OriginOffsetUnscaled.Y * this.entity.Scale); }
        public SFML.System.Vector2f OriginOffsetUnscaled { get; set; }

        public Component(Entity entity)
        {
            this.entity = entity;
            this.OriginOffsetUnscaled = new SFML.System.Vector2f(0, 0);
        }
    }
}
