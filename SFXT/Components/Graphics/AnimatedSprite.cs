using SFML.Graphics;
using SFML.System;
using SFXT.Graphics;
using SFXT.Graphics.Texels;
using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SFXT.Components.Graphics
{
    public class AnimatedSprite : Graphic, IBatchable
    {
        protected ITexels texture;
        protected Vertex[] vao;

        protected FrameHelper frameHelper;

        private Dictionary<string, Animation> animations;
        public Animation CurrentAnimation { get; protected set; }
        private Clock animationClock;
        private float playSpeed;

        public SFML.Graphics.Color? Color { get; set; }

        public AnimatedSprite(Entity entity) : base(entity)
        {
            this.texture = null;
            this.vao = new Vertex[6];
            this.Color = null;
            this.frameHelper = null;
            this.animationClock = new SFML.System.Clock();
            this.animations = new Dictionary<string, Animation>();
        }

        public AnimatedSprite(Entity entity, ITexels texture, uint frameWidth, uint frameHeight) : base(entity)
        {
            this.texture = texture;
            this.vao = new Vertex[6];
            this.Color = null;
            this.frameHelper = new FrameHelper(frameWidth, frameHeight);
            this.animationClock = new Clock();
            this.animations = new Dictionary<string, Animation>();
        }

        public void SetTextureData(ITexels texture, uint frameWidth, uint frameHeight)
        {
            this.texture = texture;
            this.frameHelper = new FrameHelper(frameWidth, frameHeight);
        }

        public void SetTextureData(ITexels texture, FrameHelper frameHelper)
        {
            this.texture = texture;
            this.frameHelper = frameHelper;
        }

        public uint GetCurrentFrame()
        {
            if (this.CurrentAnimation != null)
                return this.CurrentAnimation.GetFrame(this.animationClock.ElapsedTime * this.playSpeed);
            return 0;
        }

        public void AddAnimation(string name, Animation animation)
        {
            if (!this.animations.ContainsKey(name) && animation != null)
                this.animations.Add(name, animation);
        }

        public Animation GetAnimation(string name)
        {
            var success = this.animations.TryGetValue(name, out Animation ret);

            if (success) return ret;
            return null;
        }

        public void Play(Animation animation, float playSpeed = 1f)
        {
            this.CurrentAnimation = animation;
            this.playSpeed = playSpeed;
            this.animationClock.Restart();
        }

        public void Play(string animationName, float playSpeed = 1f)
        {
            var success = this.animations.TryGetValue(animationName, out var animation);

            if (success)
            {
                this.CurrentAnimation = animation;
                this.playSpeed = 1f;
                this.animationClock.Restart();
            }
        }

        public void Stop()
        {
            this.CurrentAnimation = null;
        }

        public override void Draw(RenderTarget target, RenderStates renderStates)
        {
            this.updateVAO();
            var state = new RenderStates(renderStates);
            state.Texture = this.texture.Texture;
            target.Draw(this.vao, PrimitiveType.Triangles, state);
        }

        protected void updateVAO()
        {
            var pos = this.Entity.Position + this.OriginOffset;
            var width = this.frameHelper.Width * this.Entity.Scale;
            var height = this.frameHelper.Height * this.Entity.Scale;

            var topLeft = new Vector2((int)pos.X - width / 2, (int)pos.Y - height / 2);
            var bottomRight = new Vector2((int)pos.X + width / 2, (int)pos.Y + height / 2);

            var topRight = new Vector2(bottomRight.X, topLeft.Y);
            var bottomLeft = new Vector2(topLeft.X, bottomRight.Y);

            if (this.Entity.Rotation != 0)
            {
                topLeft = topLeft.RotateAround(pos, this.Entity.Rotation);
                topRight = topRight.RotateAround(pos, this.Entity.Rotation);
                bottomLeft = bottomLeft.RotateAround(pos, this.Entity.Rotation);
                bottomRight = bottomRight.RotateAround(pos, this.Entity.Rotation);
            }

            if (this.FlipHorizontal)
            {
                var temp = topLeft;
                topLeft = topRight;
                topRight = temp;
                temp = bottomLeft;
                bottomLeft = bottomRight;
                bottomRight = temp;
            }

            if (this.FlipVertical)
            {
                var temp = topLeft;
                topLeft = bottomLeft;
                bottomLeft = temp;
                temp = topRight;
                topRight = bottomRight;
                bottomRight = temp;
            }

            var currentFrame = this.GetCurrentFrame();
            var frameTexel = this.frameHelper.GetFrameTexel(this.texture, currentFrame);

            if (this.Color == null)
            {
                this.vao[0] = new Vertex(topLeft, frameTexel.TopLeft);
                this.vao[1] = new Vertex(topRight, frameTexel.TopRight);
                this.vao[2] = new Vertex(bottomRight, frameTexel.BottomRight);

                this.vao[3] = new Vertex(topLeft, frameTexel.TopLeft);
                this.vao[4] = new Vertex(bottomLeft, frameTexel.BottomLeft);
                this.vao[5] = new Vertex(bottomRight, frameTexel.BottomRight);
            }
            else
            {
                this.vao[0] = new Vertex(topLeft, this.Color.Value, frameTexel.TopLeft);
                this.vao[1] = new Vertex(topRight, this.Color.Value, frameTexel.TopRight);
                this.vao[2] = new Vertex(bottomRight, this.Color.Value, frameTexel.BottomRight);

                this.vao[3] = new Vertex(topLeft, this.Color.Value, frameTexel.TopLeft);
                this.vao[4] = new Vertex(bottomLeft, this.Color.Value, frameTexel.BottomLeft);
                this.vao[5] = new Vertex(bottomRight, this.Color.Value, frameTexel.BottomRight);
            }
        }

        public RenderStates? BatchRenderStates { get => null; }
        public Texture BatchTexture { get => this.texture.Texture; }
        public Vertex[] BatchVertexes
        {
            get
            {
                this.updateVAO();
                return this.vao;
            }
        }

        public SFML.Graphics.PrimitiveType BatchPrimitiveType => PrimitiveType.Triangles;

        public class Animation
        {
            protected Time animationTime;
            protected List<KeyValuePair<uint, SFML.System.Time>> frames;

            public Animation()
            {
                animationTime = Time.Zero;
                frames = new List<KeyValuePair<uint, SFML.System.Time>>();
            }

            public Animation(Time frameTime, params uint[] frames) : this()
            {
                foreach (var frame in frames)
                    this.AddFrame(frame, frameTime);
            }

            public Animation(Animation other) : this()
            {
                foreach (var frame in other.frames)
                    this.AddFrame(frame.Key, frame.Value);
            }

            public virtual void AddFrame(uint frameId, SFML.System.Time duration)
            {
                this.frames.Add(new KeyValuePair<uint, SFML.System.Time>(frameId, duration));
                animationTime += duration;
            }

            public virtual uint GetFrame(Time time)
            {
                var passedTime = time;

                foreach(var frame in this.frames)
                {
                    passedTime -= frame.Value;
                    if (passedTime < SFML.System.Time.Zero)
                        return frame.Key;
                }

                return this.frames.Last().Key;
            }

            public virtual bool IsFinished(Time time)
            {
                return time >= this.animationTime;
            }
        }

        public class LoopingAnimation : Animation
        {
            public LoopingAnimation() : base() { }

            public LoopingAnimation(Time frameTime, params uint[] frames) : this()
            {
                foreach (var frame in frames)
                    this.AddFrame(frame, frameTime);
            }

            public LoopingAnimation(Time frameTime, uint startFrame, uint frameCount) : this()
            {
                for (uint i = 0; i < frameCount; i++)
                    this.AddFrame(startFrame + i, frameTime);
            }

            public LoopingAnimation(Animation other) : base(other) { }

            public override uint GetFrame(Time time)
            {
                var passedTime = time % this.animationTime;

                foreach (var frame in this.frames)
                {
                    passedTime -= frame.Value;
                    if (passedTime < Time.Zero)
                        return frame.Key;
                }

                return 0;
            }

            public override bool IsFinished(Time time) => false;
        }
    }
}
