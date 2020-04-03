using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Entities;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Triggers.Concrete
{
    public class BeginGame : Trigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeginGame"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        public BeginGame(Vector2 initialPosition, Rectangle boundingBox)
            : base("Begin Game", "Begins the game properly when the player passes through it.", initialPosition, boundingBox, Vector2.Zero)
        {
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            this.End();

            base.ColliderOnCollision(args);

            // Begin the game properly.
            Monde.GameManager.Begin();
        }
    }
}
