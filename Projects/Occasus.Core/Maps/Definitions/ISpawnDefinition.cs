using Microsoft.Xna.Framework;

namespace Occasus.Core.Maps.Definitions
{
    public interface ISpawnDefinition
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        Vector2 Position { get; set; }
    }
}
