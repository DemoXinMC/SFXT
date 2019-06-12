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

        private uint nextEntityId
        {
            get { return nextEntityIdInternal++; }
        }
        private uint nextEntityIdInternal = 0;

        private SpriteBatch spriteBatch;

        public Activity(Game game)
        {
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

            foreach(var item in this.entities.Where(kvp => this.entitiesToRemove.Contains(kvp.Value)).ToList())
                this.entities.Remove(item.Key);

            this.entitiesToAdd.Clear();
            this.entitiesToRemove.Clear();

            foreach(var entity in this.entities.Values)
                entity.Update();

            this.entities.OrderBy(kvp => kvp.Value.Layer);
        }

        public virtual void Render()
        {

            foreach(var entity in this.entities.Values)
                foreach(var component in entity.GetComponents<Graphic>())
                    this.spriteBatch.Add(component);
        }

        public virtual bool ShouldBelowActivitiesUpdate() { return false; }
        public virtual bool ShouldBelowActivitiesRender() { return false; }

        #region Events
        public delegate void EntityEventHandler(Entity entity);
        public event EntityEventHandler OnEntityAdded;
        public event EntityEventHandler OnEntityRemoved;
        #endregion
    }
}
