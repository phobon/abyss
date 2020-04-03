using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Entities;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Relics.Concrete.Active
{
    public class Warp : ActiveRelic
    {
        private const int BaseWarpDistance = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="Warp"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Warp(Vector2 initialPosition)
            : base(
            RelicKeys.Warp,
            "Warps the player to the opposite side of the map.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            base.Activate(entityCache);

            var newPosition = Monde.GameManager.Player.Transform.Position;
            if (Monde.GameManager.Player.GetSprite().SpriteEffects == SpriteEffects.FlipHorizontally)
            {
                newPosition.X += BaseWarpDistance;
            }
            else
            {
                newPosition.X -= BaseWarpDistance;
            }

            Monde.GameManager.Player.Transform.MoveTo(newPosition, 0);

            // TODO: Effect is required.
        }
    }
}
