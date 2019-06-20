using SFML.Graphics;
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

        public ViewManager ViewManager { get; protected set; }

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
            this.ViewManager = new ViewManager(this);
        }

        public virtual void Update()
        {
            foreach(var entity in this.entitiesToAdd)
            {
                this.entities.Add(this.nextEntityId, entity);
                this.OnEntityAdded?.Invoke(entity);
            }

            foreach (var item in this.entities.Where(kvp => this.entitiesToRemove.Contains(kvp.Value)).ToList())
            {
                this.OnEntityRemoved?.Invoke(item.Value);
                this.entities.Remove(item.Key);
            }

            this.entitiesToAdd.Clear();
            this.entitiesToRemove.Clear();

            foreach(var entity in this.entities.Values)
                entity.Update();

        }

        public virtual void Render(RenderTarget target, RenderStates states)
        {
            List<Graphic> graphicComponents = new List<Graphic>(this.entities.Count);
            foreach (var entity in this.entities.Values)
                foreach (var component in entity.GetComponents<Graphic>())
                    graphicComponents.Add(component);

            // Maybe switch this to a predicate to allow more use of the "Generic" Activity
            graphicComponents.OrderBy(item => item.Layer);

            foreach (var graphic in graphicComponents)
            {
                if(graphic.RequirePerCameraBatching)
                {
                    // per camera stuff here eventually
                }
                this.spriteBatch.Add(graphic);
            }

            var previousView = this.Game.Window.GetView();
            foreach (var view in this.ViewManager.Views)
            {
                this.Game.Window.SetView(view.View);
                spriteBatch.Draw(this.Game.Window, view.RenderStates);
            }
            this.Game.Window.SetView(previousView);

            spriteBatch.Clear();
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
