using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Occasus.Core.Assets.AtlasDefinitions
{
    public class TileAtlasDefinition
    {
        public TileAtlasDefinition(Point size)
        {
            this.Size = size;
            this.GroupedPoints = new Dictionary<int, IList<Point>>();
        }

        public Point Size
        {
            get; private set;
        }

        public int Width
        {
            get
            {
                return Size.X;
            }
        }

        public int Height
        {
            get
            {
                return Size.Y;
            }
        }

        public IDictionary<int, IList<Point>> GroupedPoints { get; private set; }
    }
}
