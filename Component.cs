using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT
{
    abstract public class Component
    {
        protected Entity entity;
        public SFML.System.Vector2f OriginOffset { get => new SFML.System.Vector2f(this.originOffsetUnscaled.X * this.entity.Scale, this.originOffsetUnscaled.Y * this.entity.Scale); }
        public SFML.System.Vector2f originOffsetUnscaled;

        public Component(Entity entity)
        {
            this.entity = entity;
        }
    }
}
