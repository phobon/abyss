using Microsoft.Xna.Framework;
using Occasus.Core.Drawing;

namespace Occasus.Core.Physics
{
    public class Transform : ITransform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transform"/> class.
        /// </summary>
        public Transform()
        {
            Scale = Vector2.One;
            Size = Vector2.One;
            Origin = Vector2.Zero;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets the grid position.
        /// </summary>
        public Point GridPosition
        {
            get
            {
                var x = this.Position.X / DrawingManager.TileWidth;
                var y = this.Position.Y / DrawingManager.TileHeight;
                return new Point((int)x, (int)y);
            }
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        public float Rotation { get; set; }
        
        public Vector2 Origin { get; set; }

        public Vector2 Size { get; set; }
    }
}
