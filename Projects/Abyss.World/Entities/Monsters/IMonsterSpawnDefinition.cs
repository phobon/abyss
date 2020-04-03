using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Abyss.World.Entities.Monsters
{
    public interface IMonsterSpawnDefinition
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        IList<Vector2> Path { get; }
    }
}
