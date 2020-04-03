using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Input;
using System;
using System.Collections.Generic;

namespace Occasus.Core
{
    public interface IEngineComponent : IDisposable
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a dictionary containing any flags the engine component requires.
        /// </summary>
        IDictionary<string, bool> Flags { get; }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        void Update(GameTime gameTime, IInputState inputState);

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Loads the content for this engine component.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Performs any initialization logic required by this engine component.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        void Begin();

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component should end.
        /// </summary>
        void End();

        /// <summary>
        /// Suspends the Engine Component from updating or drawing.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Resumes updating and drawing of the Engine Component.
        /// </summary>
        void Resume();
    }
}
