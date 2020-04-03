using System;
using Microsoft.Xna.Framework;
using Occasus.Core.Drawing;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Maths;
using Occasus.Core.Physics;

namespace Occasus.Core
{
    public abstract class GameManager<T> : IGameManager<T> where T : IEntity
    {
        private const int BaseFramerate = 60;

        private int seed;

        /// <summary>
        /// Occurs when the game has been begun.
        /// </summary>
        public event EventHandler GameBegun;

        public bool HasStarted { get; private set; }

        public T Player { get; set; }

        public Rectangle ViewPort { get; private set; }

        public Rectangle UniverseViewPort { get; private set; }

        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        public int Seed
        {
            get
            {
                return this.seed;
            }

            set
            {
                this.seed = value;
                MathsHelper.Seed(value);
            }
        }

        public abstract void Initialize();

        public abstract void Reset();

        /// <summary>
        /// Begins the game.
        /// </summary>
        public virtual bool Begin()
        {
            if (!this.HasStarted)
            {
                this.HasStarted = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        public virtual void End()
        {
            // Remove coroutines if they exist.
            this.HasStarted = false;
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="inputState">State of the input.</param>
        public virtual void Update(GameTime gameTime, IInputState inputState)
        {
            // Update the CurrentGridDepth with the Player's position.
            var currentGridDepth = this.Player.Transform.GridPosition.Y;

            // Determine the current drawable area of the game.
            var highVerticalBoundary = currentGridDepth - PhysicsManager.MapVerticalCenter;
            if (highVerticalBoundary < 0)
            {
                highVerticalBoundary = 0;
            }

            this.ViewPort = new Rectangle(0, highVerticalBoundary, PhysicsManager.MapWidth, PhysicsManager.MapHeight);
            this.UniverseViewPort = new Rectangle(0, this.ViewPort.Y * DrawingManager.TileHeight, PhysicsManager.UniverseMapWidth, PhysicsManager.UniverseMapHeight);
        }

        protected void OnGameBegun(EventArgs args)
        {
            var handler = this.GameBegun;
            if (handler != null)
            {
                handler(null, args);
            }
        }
    }
}
