using SFXT.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT
{
    public class Entity
    {
        protected Activity activity;

        public Entity(Activity activity)
        {
            this.activity = activity;
            this.components = new List<Component>();
            this.Transform = new SFML.Graphics.Transform();
            this.Position = new Vector2(0, 0);
        }

        public double X
        {
            get => this.Position.X;
            set
            {
                this.Position.X = value;
            }
        }

        public double Y
        {
            get => this.Position.Y;
            set
            {
                this.Position.Y = value;
            }
        }

        public float SizeX { get => this.Size.X; }
        public float SizeY { get => this.Size.Y; }

        public Vector2 Position;
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
                // Calculate this
                return new SFML.Graphics.IntRect(0, 0, 0, 0);
            }
        }

        public SFML.Graphics.Transform Transform { get; protected set; }

        private float _rotation = 0;
        public float Rotation
        {
            get => this._rotation;
            set
            {
                this._rotation = value;
                this.Transform.Rotate(value);
            }
        }

        private float _scale = 1f;
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
