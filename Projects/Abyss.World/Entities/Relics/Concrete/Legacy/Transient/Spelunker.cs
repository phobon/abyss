using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using System.Collections.Generic;

namespace Abyss.World.Entities.Relics.Concrete.Transient
{
    public class Spelunker : StompRelic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Spelunker"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Spelunker(Vector2 initialPosition)
            : base(
            RelicKeys.Spelunker,
            "Chance to spawn a rift crystal near where the player lands.",
            initialPosition,
            Rectangle.Empty,
            null,
            75)
        {
        }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            base.Activate(entityCache);

            // TODO: Spawn a rift crystal (of any type) on the platform near the player.
        }
    }
}
