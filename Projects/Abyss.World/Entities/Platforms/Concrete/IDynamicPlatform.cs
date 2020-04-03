using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Abyss.World.Entities.Platforms
{
    public interface IDynamicPlatform : IPlatform
    {
        /// <summary>
        /// Gets the path of this platform.
        /// </summary>
        IEnumerable<Vector2> Path { get; }
    }
}
