using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Occasus.Core.Maps.Definitions
{
    public interface IPlatformDefinition : ISpawnDefinition
    {
        /// <summary>
        /// Gets the size of the platform.
        /// </summary>
        Rectangle Size { get; }

        /// <summary>
        /// Gets the sprite location.
        /// </summary>
        Point SpriteLocation { get; }

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
