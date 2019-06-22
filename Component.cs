using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT
{
    abstract public class Component
    {
        public Entity Entity { get; protected set; }

        public Vector2 OriginOffset { get => new Vector2(this.OriginOffsetUnscaled.X * this.Entity.Scale, this.OriginOffsetUnscaled.Y * this.Entity.Scale); }
        public Vector2 OriginOffsetUnscaled { get; set; }

        public Component(Entity entity)
        {
            this.Entity = entity;
            this.OriginOffsetUnscaled = new SFML.System.Vector2f(0, 0);
        }
    }
}
