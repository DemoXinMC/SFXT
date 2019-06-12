using System;
using System.Collections.Generic;
using System.Text;

namespace SFXT
{
    public class Entity
    {
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

        public float OriginX
        {
            get => this.Origin.X;
            set => Math.Min(value, this.SizeX);
        }

        public float OriginY
        {
            get => this.Origin.Y;
            set => Math.Min(value, this.SizeY);
        }

        public float SizeX { get => this.Size.X; }
        public float SizeY { get => this.Size.Y; }

        public SFML.System.Vector2f Position;
        public SFML.System.Vector2f Origin;
        public SFML.System.Vector2f Size
        {
            get
            {
                return new SFML.System.Vector2f(0, 0);
            }
        }
        public float Rotation { get; protected set; }
        public float Scale { get; protected set; }

        public int Layer { get; private set; }

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
