using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Entities;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Relics.Concrete.Transient
{
    public class Warp : Relic
    {
        private const int BaseWarpDistance = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="Warp"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Warp(Vector2 initialPosition)
            : base(
            RelicKeys.Warp,
            "Warps the player some distance in the direction they are facing.",
            initialPosition,
            Rectangle.Empty,
            RelicType.Transient,
            RelicActivationType.DimensionShift,
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

            var newPosition = GameManager.Player.Transform.Position;
            if (GameManager.Player.GetSprite().SpriteEffects == SpriteEffects.FlipHorizontally)
            {
                newPosition.X += BaseWarpDistance;
            }
            else
            {
                newPosition.X -= BaseWarpDistance;
            }

            GameManager.Player.Transform.MoveTo(newPosition, 0);

            // TODO: Effect is required.
        }
    }
}
