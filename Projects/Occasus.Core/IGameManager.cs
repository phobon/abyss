using System;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using Occasus.Core.Input;

namespace Occasus.Core
{
    public interface IGameManager<T> where T : IEntity
    {
        /// <summary>
        /// Occurs when the game has been begun.
        /// </summary>
        event EventHandler GameBegun;

        bool HasStarted { get; }

        T Player { get; set; }

        Rectangle ViewPort { get; }

        Rectangle UniverseViewPort { get; }

        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        int Seed { get; set; }

        void Initialize();
        void Reset();

        /// <summary>
        /// Begins the game.
        /// </summary>
        bool Begin();

        /// <summary>
        /// Ends the game.
        /// </summary>
        void End();

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="inputState">State of the input.</param>
        void Update(GameTime gameTime, IInputState inputState);
    }
}
