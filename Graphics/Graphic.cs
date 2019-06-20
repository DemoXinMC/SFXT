namespace SFXT.Graphics
{
    abstract public class Graphic : Component
    {
        public bool Relative { get; set; }

        public bool FlipHorizontal { get; set; }

        public bool FlipVertical { get; set; }
        public int Layer
        {
            get
            {
                if(this.Relative && this.entity != null)
                    return this.entity.Layer + this.layer;
                return this.layer;
            }
            set
            {
                this.layer = value;
            }
        }
        private int layer = 0;
        public abstract void Draw(SFML.Graphics.RenderTarget target, SFML.Graphics.RenderStates renderStates);

        public Graphic(Entity entity) : base(entity)
        {
            this.Relative = true;
            this.FlipHorizontal = false;
            this.FlipVertical = false;
            this.RequirePerCameraBatching = false;
        }

        public bool RequirePerCameraBatching { get; protected set; }
    }
}
