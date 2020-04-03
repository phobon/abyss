using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using System.Collections.Generic;

namespace Abyss.World.Entities.Monsters
{
    /// <summary>
    /// Interface for a Monster in Abyss. Monsters are dynamic hazards that attempt to destroy the Player.
    /// </summary>
    public interface IMonster : IEntity
    {
        /// <summary>
        /// Gets the type of zone this Monster lives in.
        /// </summary>
        ZoneType ZoneType { get; }

        /// <summary>
        /// Gets the path that this monster travels on.
        /// </summary>
        IEnumerable<Vector2> Path { get; }

        /// <summary>
        /// Triggers a movement decision.
        /// </summary>
        void TriggerMovementDecision();

        void Die();
    }
}
