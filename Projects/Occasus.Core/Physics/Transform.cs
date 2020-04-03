using Microsoft.Xna.Framework;
using Occasus.Core.Drawing;

namespace Occasus.Core.Physics
{
    public class Transform : ITransform
    {
        private Vector2 position;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transform"/> class.
        /// </summary>
        public Transform()
        {
            Scale = Vector2.One;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position
        {
            get { return this.position; }

            set
            {
                if (this.position == value)
                {
                    return;
                }

                this.position = value;
            }
        }

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
        /// <value>
        /// The scale.
        /// </value>
        public Vector2 Scale
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public float Rotation
        {
            get; set;
        }
    }
}
