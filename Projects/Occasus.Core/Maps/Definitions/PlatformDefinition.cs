using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Occasus.Core.Maps.Definitions
{
    public class PlatformDefinition : SpawnDefinition, IPlatformDefinition
    {
        public PlatformDefinition(string name, Rectangle size, Point? spriteLocation) 
            : base(name, new Vector2(size.X, size.Y))
        {
            this.Size = size;

            if (spriteLocation.HasValue)
            {
                this.SpriteLocation = spriteLocation.Value;
            }
            
            this.Path = new List<Vector2>();
            this.Parameters = new Dictionary<string, int>();
        }

        public Rectangle Size { get; }

        public Point SpriteLocation { get; set; }

        public IList<Vector2> Path { get; }

        public IDictionary<string, int> Parameters { get; }
    }
}
