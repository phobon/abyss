using System;

using Abyss.World.Entities.Monsters;
using Abyss.World.Entities.Platforms;
using Microsoft.Xna.Framework;
using Occasus.Core.Tiles;
using System.Collections.Generic;

namespace Abyss.World.Maps
{
    public interface IMapChunk
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Gets the layout.
        /// </summary>
        IList<Rectangle> Layout { get; }
            
        /// <summary>
        /// Gets the tile map.
        /// </summary>
        ITile[,] TileMap { get; }

        /// <summary>
        /// Gets the doodad map.
        /// </summary>
        ITile[,] DoodadMap { get; }

        /// <summary>
        /// Gets the special platforms.
        /// </summary>
        IList<IPlatformDefinition> SpecialPlatforms { get; } 
        
        /// <summary>
        /// Gets the property spawn points.
        /// </summary>
        IList<Tuple<string, Rectangle>> PropSpawnPoints { get; }

        /// <summary>
        /// Gets the triggers.
        /// </summary>
        IList<Tuple<string, Rectangle>> Triggers { get; }

        /// <summary>
        /// Gets the item spawn points.
        /// </summary>
        IList<Vector2> ItemSpawnPoints { get; }

        /// <summary>
        /// Gets the hazards.
        /// </summary>
        IList<Vector2> Hazards { get; }

        /// <summary>
        /// Gets the monster spawn points.
        /// </summary>
        IList<IMonsterSpawnDefinition> MonsterSpawnPoints { get; }
    }
}
