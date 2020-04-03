using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Passive
{
    public class HeartSeeker : StompRelic
    {
        public HeartSeeker(Vector2 initialPosition)
            : base(
            RelicKeys.HeartSeeker,
            "Chance to spawn health on the platform the player lands on.",
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
