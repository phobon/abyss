using System;

using Abyss.World.Entities.Monsters;
using Abyss.World.Entities.Platforms;
using Microsoft.Xna.Framework;
using Occasus.Core.Physics;
using Occasus.Core.Tiles;
using System.Collections.Generic;

namespace Abyss.World.Maps
{
    public class MapChunk : IMapChunk
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunk" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public MapChunk(string type)
        {
            this.Type = type;

            this.Layout = new List<Rectangle>();

            this.TileMap = new ITile[PhysicsManager.MapWidth, PhysicsManager.MapHeight];
            this.DoodadMap = new ITile[PhysicsManager.MapWidth, PhysicsManager.MapHeight];

            this.SpecialPlatforms = new List<IPlatformDefinition>();
            this.PropSpawnPoints = new List<Tuple<string, Rectangle>>();
            this.Triggers = new List<Tuple<string, Rectangle>>();
            this.ItemSpawnPoints = new List<Vector2>();
            this.Hazards = new List<Vector2>();
            this.MonsterSpawnPoints = new List<IMonsterSpawnDefinition>();
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public string Type
        {
            get; private set;
        }

        /// <summary>
        /// Gets the layout.
        /// </summary>
        public IList<Rectangle> Layout
        {
            get; private set;
        }

        /// <summary>
        /// Gets the tile map.
        /// </summary>
        public ITile[,] TileMap
        {
            get; private set;
        }

        /// <summary>
        /// Gets the doodad map.
        /// </summary>
        public ITile[,] DoodadMap
        {
            get; private set;
        }

        /// <summary>
        /// Gets the special platforms.
        /// </summary>
        public IList<IPlatformDefinition> SpecialPlatforms
        {
            get; private set;
        }

        /// <summary>
        /// Gets the property spawn points.
        /// </summary>
        public IList<Tuple<string, Rectangle>> PropSpawnPoints
        {
            get; private set;
        }

        /// <summary>
        /// Gets the triggers.
        /// </summary>
        public IList<Tuple<string, Rectangle>> Triggers
        {
            get; private set;
        }

        /// <summary>
        /// Gets the item spawn points.
        /// </summary>
        public IList<Vector2> ItemSpawnPoints
        {
            get; private set;
        }

        /// <summary>
        /// Gets the hazards.
        /// </summary>
        public IList<Vector2> Hazards
        {
            get; private set;
        }

        /// <summary>
        /// Gets the monster spawn points.
        /// </summary>
        public IList<IMonsterSpawnDefinition> MonsterSpawnPoints
        {
            get; private set;
        }
    }
}
