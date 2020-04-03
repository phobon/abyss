using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Abyss.World.Entities.Monsters
{
    internal class MonsterSpawnDefinition : IMonsterSpawnDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonsterSpawnDefinition"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="position">The position.</param>
        /// <param name="path">The path.</param>
        public MonsterSpawnDefinition(string name, Vector2 position, IEnumerable<Vector2> path)
        {
            this.Name = name;
            this.Position = position;

            // Add path items to our path.
            this.Path = new List<Vector2>();
            foreach (var p in path)
            {
                this.Path.Add(p);
            }

            // Add the initial position to the path.
            this.Path.Add(this.Position);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector2 Position
        {
            get; set;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public IList<Vector2> Path
        {
            get; private set;
        }
    }
}
