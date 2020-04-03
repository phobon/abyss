using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Occasus.Core.Maps.Definitions
{
    public class ActorSpawnDefinition : SpawnDefinition, IActorSpawnDefinition
    {
        public ActorSpawnDefinition(string name, Vector2 position, IEnumerable<Vector2> path = null) 
            : base(name, position)
        {
            this.Path = new List<Vector2>();
            if (path != null)
            {
                foreach (var p in path)
                {
                    this.Path.Add(p);
                }
            }
        }

        public IList<Vector2> Path { get; }
    }
}
