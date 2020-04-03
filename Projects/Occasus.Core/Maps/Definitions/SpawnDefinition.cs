using Microsoft.Xna.Framework;

namespace Occasus.Core.Maps.Definitions
{
    public class SpawnDefinition : ISpawnDefinition
    {
        public SpawnDefinition(string name, Vector2 position)
        {
            this.Name = name;
            this.Position = position;
        }

        public string Name { get; }

        public Vector2 Position { get; set; }
    }
}
