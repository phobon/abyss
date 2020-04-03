using Microsoft.Xna.Framework;

namespace Occasus.Core.Physics
{
    public interface ITransform
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets the grid position.
        /// </summary>
        Point GridPosition { get; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        Vector2 Scale { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        float Rotation { get; set; }
    }
}
