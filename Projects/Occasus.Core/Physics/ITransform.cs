using Microsoft.Xna.Framework;

namespace Occasus.Core.Physics
{
    public interface ITransform
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets the grid position.
        /// </summary>
        Point GridPosition { get; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        Vector2 Scale { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the origin of the transform.
        /// </summary>
        Vector2 Origin { get; set; }

        Vector2 Size { get; set; }
    }
}
