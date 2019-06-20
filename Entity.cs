using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT
{
    public class Entity
    {
        public Entity()
        {
            this.components = new List<Component>();
            this.Transform = new SFML.Graphics.Transform();
            this.Position = new SFML.System.Vector2f();
        }

        public Entity(int x, int y) : this()
        {
            this.Position = new SFML.System.Vector2f(x, y);
        }

        public float X
        {
            get => this.Position.X;
            set => this.Position.X = value;
        }

        public float Y
        {
            get => this.Position.Y;
            set => this.Position.Y = value;
        }

        public float SizeX { get => this.Size.X; }
        public float SizeY { get => this.Size.Y; }

        public SFML.System.Vector2f Position;
        public SFML.System.Vector2f Size
        {
            get
            {
                var globalBounds = this.GlobalBounds;
                return new SFML.System.Vector2f(globalBounds.Width, globalBounds.Height);
            }
        }

        public SFML.Graphics.IntRect GlobalBounds
        {
            get
            {
                return new SFML.Graphics.IntRect(0, 0, 0, 0);
            }
        }

        public SFML.Graphics.Transform Transform { get; protected set; }

        private float _rotation;
        public float Rotation
        {
            get => this._rotation;
            set
            {
                this._rotation = value;
                this.Transform.Rotate(value);
            }
        }

        private float _scale;
        public float Scale
        {
            get => this._scale;
            set {
                this._scale = value;
                this.Transform.Scale(new SFML.System.Vector2f(value, value));
            }
        }

        public int Layer { get; set; }

        private List<Component> components;
        public void AddComponent(Component component)
        {
            this.components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            this.components.Remove(component);
        }

        public T GetComponent<T>() where T : Component
        {
            foreach(var component in this.components)
                if(component is T)
                    return (T)component;
            return null;
        }

        public List<T> GetComponents<T>() where T : Component
        {
            var retValue = new List<T>();

            foreach(var component in this.components)
                if(component is T)
                    retValue.Add((T)component);

            if(retValue.Count > 0)
                return retValue;
            return null;
        }

        public virtual void Update() { }
    }
}
