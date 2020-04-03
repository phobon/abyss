﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Components;
using Occasus.Core.Input;
using Occasus.Core.Physics;
using System.Collections.Generic;

using Occasus.Core.States;

namespace Occasus.Core.Entities
{
    public abstract class Entity : StateMachine, IEntity
    {
        private ICollider collider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        protected Entity(string name, string description)
            : base(name, description)
        {
            this.Transform = new Transform();
            this.Components = new Dictionary<string, IEntityComponent>();
            this.Tags = new List<string>();
        }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        public ITransform Transform
        {
            get; private set;
        }

        /// <summary>
        /// Gets a list of entity components associated with this entity.
        /// </summary>
        /// <remarks>
        /// Entity components are reusable objects that implement some sort of base-level functionality; such as drawing a sprite, etc.
        /// </remarks>
        public IDictionary<string, IEntityComponent> Components
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets an object that is used in physics collision methods.
        /// </summary>
        /// <remarks>
        /// If the Collider object is null, then this object should not be used in collision detection.
        /// </remarks>
        public ICollider Collider
        {
            get
            {
                return this.collider;
            }

            set
            {
                if (this.collider == value)
                {
                    return;
                }

                if (this.collider != null)
                {
                    this.collider.Collision -= this.ColliderOnCollision;
                }

                this.collider = value;

                if (this.collider == null)
                {
                    this.Flags[EngineFlag.Collidable] = false;
                }
                else
                {
                    this.Flags[EngineFlag.Collidable] = true;
                    this.collider.Collision += this.ColliderOnCollision;
                }
            }
        }

        /// <summary>
        /// Gets a collection of tags associated with this entity.
        /// </summary>
        /// <remarks>
        /// Used primarily to accelerate lookups in a parent Scene. The Scene caches these tags as entities are created.
        /// </remarks>
        public IList<string> Tags
        {
            get; private set;
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            base.Update(gameTime, inputState);

            foreach (var c in this.Components.Values)
            {
                if (c.Flags[EngineFlag.Active])
                {
                    c.Update(gameTime, inputState);
                }
            }
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (var c in this.Components.Values)
            {
                if (c.Flags[EngineFlag.Visible])
                {
                    c.Draw(gameTime, spriteBatch);
                }
            }
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            foreach (var c in this.Components.Values)
            {
                c.Initialize();
            }
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            foreach (var c in this.Components.Values)
            {
                if (!c.Flags[EngineFlag.DeferredBegin])
                {
                    c.Begin();
                }
            }
        }

        /// <summary>
        /// Resumes updating and drawing of the Engine Component.
        /// </summary>
        public override void Resume()
        {
            base.Resume();

            foreach (var c in this.Components.Values)
            {
                if (!c.Flags[EngineFlag.DeferredBegin])
                {
                    c.Resume();
                }
            }
        }

        /// <summary>
        /// Suspends the Engine Component from updating or drawing.
        /// </summary>
        public override void Suspend()
        {
            base.Suspend();

            foreach (var c in this.Components.Values)
            {
                c.Suspend();
            }
        }

        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public override void End()
        {
            base.End();

            foreach (var e in this.Components.Values)
            {
                e.End();
            }
        }

        protected virtual void ColliderOnCollision(CollisionEventArgs args)
        {
        }
    }
}
