using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Abyss.World.Entities.Platforms
{
    public class PlatformDefinition : IPlatformDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformDefinition"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="initialPosition">The position.</param>
        /// <param name="size">The size.</param>
        public PlatformDefinition(string name, Vector2 initialPosition, Rectangle size)
        {
            this.Name = name;
            this.Position = initialPosition;
            this.Size = size;

            this.Path = new List<Vector2>();
            this.Parameters = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the name of the platform.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the position of the platform.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets the size of the platform.
        /// </summary>
        public Rectangle Size { get; private set; }

        /// <summary>
        /// Gets or sets the sprite location.
        /// </summary>
        /// <value>
        /// The sprite location.
        /// </value>
        public Point SpriteLocation { get; set; }

        /// <summary>
        /// Gets the path of this platform.
        /// </summary>
        /// <remarks>
        /// This will be null if this is a regular platform. If it's a dynamic platform, this will be populated with stuff.
        /// </remarks>
        public IList<Vector2> Path { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IDictionary<string, int> Parameters { get; private set; }
    }
}
