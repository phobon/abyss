using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Abyss.World.Entities.Platforms
{
    public interface IPlatformDefinition
    {
        /// <summary>
        /// Gets the name of the platform.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the position of the platform.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets the size of the platform.
        /// </summary>
        Rectangle Size { get; }

        /// <summary>
        /// Gets or sets the sprite location.
        /// </summary>
        /// <value>
        /// The sprite location.
        /// </value>
        Point SpriteLocation { get; set; }

        /// <summary>
        /// Gets the path of this platform.
        /// </summary>
        /// <remarks>
        /// This will be null if this is a regular platform. If it's a dynamic platform, this will be populated with stuff.
        /// </remarks>
        IList<Vector2> Path { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        IDictionary<string, int> Parameters { get; } 
    }
}
