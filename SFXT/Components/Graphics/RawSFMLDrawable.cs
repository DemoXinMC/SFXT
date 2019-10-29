using SFML.Graphics;
using SFXT.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT.Components.Graphics
{
    public class RawSFMLDrawable : Graphic
    {
        public SFML.Graphics.Drawable drawable { get; private set; }
        public RenderStates? renderStates { get; private set; }
        public RawSFMLDrawable(Entity entity, SFML.Graphics.Drawable drawable) : base(entity)
        {
            this.drawable = drawable;
            this.renderStates = null;
        }

        public RawSFMLDrawable(Entity entity, SFML.Graphics.Drawable drawable, RenderStates renderStates) : base(entity)
        {
            this.drawable = drawable;
            this.renderStates = renderStates;
        }

        public override void Draw(RenderTarget target, RenderStates renderStates)
        {
            if (this.renderStates != null)
                target.Draw(this.drawable, this.renderStates.Value);
            else
                target.Draw(this.drawable, renderStates);
        }
    }
}
