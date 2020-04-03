using System.Collections.Generic;
using Occasus.Core.Maps.Definitions;
using Occasus.Core.Maps.Tiles;

namespace Occasus.Core.Maps
{
    public sealed class MapChunk : IMapChunk
    {
        public MapChunk(string type, int width, int height, IEnumerable<string> layoutKeys, IEnumerable<string> spawnKeys)
        {
            this.Type = type;

            this.TileMap = new ITile[width, height];
            this.DoodadMap = new ITile[width, height];

            this.Layout = new Dictionary<string, IList<IPlatformDefinition>>();
            foreach (var k in layoutKeys)
            {
                this.Layout.Add(k, new List<IPlatformDefinition>());
            }

            this.Spawns = new Dictionary<string, IList<ISpawnDefinition>>();
            foreach (var k in spawnKeys)
            {
                this.Spawns.Add(k, new List<ISpawnDefinition>());
            }
        }

        public string Type { get; }

        public IDictionary<string, IList<IPlatformDefinition>> Layout { get; }

        public ITile[,] TileMap { get; }

        public ITile[,] DoodadMap { get; }

        public IDictionary<string, IList<ISpawnDefinition>> Spawns { get; }
    }
}
