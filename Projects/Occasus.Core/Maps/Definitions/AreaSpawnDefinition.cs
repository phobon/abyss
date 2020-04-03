using Microsoft.Xna.Framework;

namespace Occasus.Core.Maps.Definitions
{
    public class AreaSpawnDefinition : SpawnDefinition, IAreaSpawnDefinition
    {
        public AreaSpawnDefinition(string name, Rectangle area) 
            : base(name, new Vector2(area.Left, area.Top))
        {
            this.Area = area;
        }

        public Rectangle Area { get; }
    }
}
