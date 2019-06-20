using SFXT.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFXT
{
    public class Activity
    {
        public Game Game { get; private set; }

        private Dictionary<uint, Entity> entities;
        private List<Entity> entitiesToAdd;
        private List<Entity> entitiesToRemove;

        public bool AddEntity(Entity entity)
        {
            bool cancel = false;

            this.OnEntityQueuedAdd?.Invoke(entity, ref cancel);

            if(!cancel)
                entitiesToAdd.Add(entity);

            return !cancel;
        }

        public bool RemoveEntity(Entity entity)
        {
            bool cancel = false;

            this.OnEntityQueuedRemove?.Invoke(entity, ref cancel);

            if (!cancel)
                entitiesToRemove.Add(entity);

            return !cancel;
        }

        public bool RemoveEntity(uint entityId)
        {
            bool cancel = false;

            var success = this.entities.TryGetValue(entityId, out Entity entity);

            if (!success)
                return false;

            this.OnEntityQueuedRemove?.Invoke(entity, ref cancel);

            if (!cancel)
                entitiesToRemove.Add(entity);

            return !cancel;
        }

        private uint nextEntityId
        {
            get { return nextEntityIdInternal++; }
        }
        private uint nextEntityIdInternal = 0;

        private SpriteBatch spriteBatch;

        public Activity(Game game)
        {
            this.entities = new Dictionary<uint, Entity>();
            this.entitiesToAdd = new List<Entity>();
            this.entitiesToRemove = new List<Entity>();

            this.Game = game;
            this.spriteBatch = new SpriteBatch();
        }

        public virtual void Update()
        {
            foreach(var entity in this.entitiesToAdd)
            {
                this.entities.Add(this.nextEntityId, entity);
                this.OnEntityAdded(entity);
            }

            foreach (var item in this.entities.Where(kvp => this.entitiesToRemove.Contains(kvp.Value)).ToList())
            {
                this.OnEntityRemoved(item.Value);
                this.entities.Remove(item.Key);
            }

            this.entitiesToAdd.Clear();
            this.entitiesToRemove.Clear();

            foreach(var entity in this.entities.Values)
                entity.Update();

            
        }

        public virtual void Render()
        {
            this.entities.OrderBy(kvp => kvp.Value.Layer);

            foreach (var entity in this.entities.Values)
                foreach(var component in entity.GetComponents<Graphic>())
                    this.spriteBatch.Add(component);
        }

        public virtual bool ShouldBelowActivitiesUpdate() { return false; }
        public virtual bool ShouldBelowActivitiesRender() { return false; }

        public delegate void EntityEventHandler(Entity entity);
        public event EntityEventHandler OnEntityAdded;
        public event EntityEventHandler OnEntityRemoved;

        public delegate bool EntityEventCancellable(Entity entity, ref bool cancel);
        public event EntityEventCancellable OnEntityQueuedAdd;
        public event EntityEventCancellable OnEntityQueuedRemove;
    }
}
