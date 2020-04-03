using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Transient
{
    public class TreasureSeeker : StompRelic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreasureSeeker"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public TreasureSeeker(Vector2 initialPosition)
            : base(
            RelicKeys.TreasureSeeker,
            "Chance to spawn a treasure chest on the platform the player lands on.",
            initialPosition,
            Rectangle.Empty,
            null,
            25)
        {
        }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            base.Activate(entityCache);

            // TODO: Spawn a treasure chest on the platform near the player.
        }
    }
}
