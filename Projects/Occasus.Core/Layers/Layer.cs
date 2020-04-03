using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Components;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Scenes;
using System.Collections.Generic;
using System.Globalization;

namespace Occasus.Core.Layers
{
    public abstract class Layer : EngineComponent, ILayer
    {
        protected readonly IList<IEntity> entities;
        protected readonly IList<IEntity> allEntities;

        private readonly IList<IEntityComponent> lightingCache;

#if DEBUG
        private readonly string relevantEntityDebugKey;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer" /> class.
        /// </summary>
        /// <param name="parentScene">The parent scene.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="layerType">Type of the layer.</param>
        /// <param name="depth">The depth.</param>
        protected Layer(IScene parentScene, string name, string description, LayerType layerType, int depth)
            : base(name, description)
        {
            this.Parent = parentScene;

            this.LayerType = layerType;
            this.Depth = depth;

            this.entities = new List<IEntity>();
            this.allEntities = new List<IEntity>();
            this.lightingCache = new List<IEntityComponent>();

            this.SpriteBatch = new SpriteBatch(DrawingManager.GraphicsDevice);
            
#if DEBUG
            this.relevantEntityDebugKey = this.Name + " Relevant Entities";
#endif
        }

        /// <summary>
        /// Gets the Scene this Layer belongs to.
        /// </summary>
        public IScene Parent
        {
            get; private set;
        }

        /// <summary>
        /// Gets the depth of this layer. Layers are depth sorted and drawn by a Scene.
        /// </summary>
        public int Depth
        {
            get; private set;
        }

        /// <summary>
        /// Gets the type of Layer this is.
        /// </summary>
        public LayerType LayerType
        {
            get; private set;
        }

        /// <summary>
        /// Gets the active, relevant entities.
        /// </summary>
        public IEnumerable<IEntity> Entities
        {
            get
            {
                return this.entities;
            }
        }

        /// <summary>
        /// Gets the lighting cache collection. Light sources are cached as part of regular game execution.
        /// </summary>
        public IEnumerable<IEntityComponent> LightingCache
        {
            get
            {
                return this.lightingCache;
            }
        }

        /// <summary>
        /// Gets the spritebatch for this particular layer.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get; private set;
        }

        /// <summary>
        /// Adds an entity to the layer.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <remarks>
        /// Note that this does not add an entity to the Entity cache; just into an internal collection that is then iterated over when updating the cache. This is purely for performance reasons.
        /// </remarks>
        public void AddEntity(IEntity entity)
        {
            // Flag this entity as new, so it is prioritised in caching.
            entity.Flags[EngineFlag.New] = true;

            this.allEntities.Add(entity);

            // Add the entity to the Scene's TagCache.
            foreach (var t in entity.Tags)
            {
                // If the TagCache does not contain the key, create a new one and add it to the cache. Otherwise, just add it to that tag collection.
                // TODO: Maybe move this method into the UpdateEntityCache method.
                if (!this.Parent.TagCache.ContainsKey(t))
                {
                    this.Parent.TagCache.Add(t, new List<IEntity> { entity });
                }
                else
                {
                    this.Parent.TagCache[t].Add(entity);
                }
            }
        }

        /// <summary>
        /// Removes an entity from this layer.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void RemoveEntity(IEntity entity)
        {
            if (!this.allEntities.Contains(entity))
            {
                return;
            }

            // Remove the entity from all collections.
            this.allEntities.Remove(entity);
            if (this.entities.Contains(entity))
            {
                this.entities.Remove(entity);
            }

            // Remove the entity from the parent Scene's TagCache.
            // TODO: Maybe move this method into the UpdateEntityCache method.
            foreach (var t in entity.Tags)
            {
                // Remove the item from the tag collection.
                this.Parent.TagCache[t].Remove(entity);

                // If there are no entities left in a particular tag collection, remove it from the cache.
                if (this.Parent.TagCache[t].Count == 0)
                {
                    this.Parent.TagCache.Remove(t);
                }
            }
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            base.Update(gameTime, inputState);

            foreach (var e in this.Entities)
            {
                if (e.Flags[EngineFlag.Active])
                {
                    e.Update(gameTime, inputState);
                }
            }

#if DEBUG
            Engine.Debugger.EntityCount += this.entities.Count;
#endif
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (var e in this.Entities)
            {
                // Draw objects that are visible and not flagged as being deferred render.
                if (e.Flags[EngineFlag.Visible] && !e.Flags[EngineFlag.DeferredRender])
                {
                    e.Draw(gameTime, spriteBatch);
                }
            }
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Inititalize all entities in this layer.
            foreach (var e in this.allEntities)
            {
                e.Initialize();
            }
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            // Initialize the entity cache here so we only get what we want.
            this.InitializeEntityCache();

            foreach (var e in this.entities)
            {
                if (!e.Flags[EngineFlag.DeferredBegin])
                {
                    e.Begin();
                }
            }
        }

        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public override void End()
        {
            base.End();

            foreach (var e in this.allEntities)
            {
                e.End();
            }

            this.entities.Clear();
            this.allEntities.Clear();

            this.Dispose();
        }

        /// <summary>
        /// Updates the entity cache.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        public virtual void UpdateEntityCache(Rectangle viewPort)
        {
            foreach (var e in this.entities)
            {
                e.Suspend();
            }

            this.entities.Clear();
            foreach (var entity in this.allEntities)
            {
                this.entities.Add(entity);
                if (!entity.Flags[EngineFlag.Initialized])
                {
                    entity.Initialize();
                }
            }

            foreach (var e in this.entities)
            {
                // If the entity is deferred begin and hasn't begun, skip it; else resume it.
                if (e.Flags[EngineFlag.DeferredBegin] && !e.Flags[EngineFlag.HasBegun])
                {
                    continue;
                }

                e.Resume();
            }

            this.UpdateLightingCache();
        }

        protected virtual void InitializeEntityCache()
        {
            this.UpdateEntityCache(Rectangle.Empty);
        }

        protected void UpdateLightingCache()
        {
            this.lightingCache.Clear();
            foreach (var e in this.entities)
            {
                if (e.Tags.Contains(Lighting.DeferredLightSource))
                {
                    if (e.Components.ContainsKey(LightSource.Tag))
                    {
                        this.lightingCache.Add(e.Components[LightSource.Tag]);
                    }
                }
            }
        }
    }
}
