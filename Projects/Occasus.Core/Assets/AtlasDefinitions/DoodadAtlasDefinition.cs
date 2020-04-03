using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Occasus.Core.Assets.AtlasDefinitions
{
    public class DoodadAtlasDefinition
    {
        public DoodadAtlasDefinition(string id)
        {
            this.Id = id;

            this.Placements = new List<DoodadPlacement>();
            this.Tiles = new List<int>();
            this.Doodads = new List<Point>();
        }

        public string Id
        {
            get; private set;
        }

        public IList<DoodadPlacement> Placements
        {
            get; private set;
        }

        public IList<int> Tiles
        {
            get; private set;
        }

        public IList<Point> Doodads
        {
            get; private set;
        }
    }
}
