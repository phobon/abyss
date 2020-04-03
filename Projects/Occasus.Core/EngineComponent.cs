using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Input;
using System;
using System.Collections.Generic;

namespace Occasus.Core
{
    public abstract class EngineComponent : IEngineComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineComponent"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        protected EngineComponent(string name, string description)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.Description = description;

            this.Flags = new Dictionary<string, bool>
                             {
                                 { EngineFlag.Active, false },
                                 { EngineFlag.Visible, false },
                                 { EngineFlag.Relevant, true },
                                 { EngineFlag.Collidable, false },
                                 { EngineFlag.Initialized, false },
                                 { EngineFlag.DeferredRender, false },
                                 { EngineFlag.DeferredBegin, false },
                                 { EngineFlag.HasBegun, false },
                                 { EngineFlag.New, false },
                                 { EngineFlag.ForceIncludeInCache, false }
                             };
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Id
        {
            get; private set;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description
        {
            get; private set;
        }

        public IDictionary<string, bool> Flags
        {
            get; private set;
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public virtual void Update(GameTime gameTime, IInputState inputState)
        {
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// Performs any initialization logic required by this engine component.
        /// </summary>
        public virtual void Initialize()
        {
            this.LoadContent();
            this.Flags[EngineFlag.Initialized] = true;
            this.Flags[EngineFlag.Relevant] = true;
        }

        /// <summary>
        /// Loads the content for the Engine Component.
        /// </summary>
        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public virtual void Begin()
        {
            this.Flags[EngineFlag.Active] = true;
            this.Flags[EngineFlag.Visible] = true;
            this.Flags[EngineFlag.HasBegun] = true;
        }

        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public virtual void End()
        {
            this.Flags[EngineFlag.Active] = false;
            this.Flags[EngineFlag.Visible] = false;
            this.Flags[EngineFlag.HasBegun] = false;
            this.Flags[EngineFlag.Initialized] = false;
            this.Flags[EngineFlag.Relevant] = false;
        }

        /// <summary>
        /// Suspends the Engine Component from updating or drawing.
        /// </summary>
        public virtual void Suspend()
        {
            this.Flags[EngineFlag.Active] = false;
            this.Flags[EngineFlag.Visible] = false;
        }

        /// <summary>
        /// Resumes updating and drawing of the Engine Component.
        /// </summary>
        public virtual void Resume()
        {
            this.Flags[EngineFlag.Active] = true;
            this.Flags[EngineFlag.Visible] = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
