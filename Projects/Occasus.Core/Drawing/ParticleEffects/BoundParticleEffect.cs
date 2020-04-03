using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public abstract class BoundParticleEffect : ParticleEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundParticleEffect"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="particlesPerCell">The particles per cell.</param>
        /// <param name="particleSize">Size of the particle.</param>
        protected BoundParticleEffect(
            IEntity parent,
            string name, 
            string description, 
            int particlesPerCell,
            int particleSize)
            : base(parent, name, description, 0, Vector2.Zero, Vector2.One)
        {
            this.ParticlesPerCell = particlesPerCell;
            this.ParticleSize = particleSize;

            // Determine maximum number of particles.
            var horizontalCells = parent.Collider.BoundingBox.Width / DrawingManager.TileWidth;
            var verticalCells = parent.Collider.BoundingBox.Height / DrawingManager.TileHeight;

            for (var y = 0; y < verticalCells; y++)
            {
                for (var x = 0; x < horizontalCells; x++)
                {
                    this.MaximumParticles += this.ParticlesPerCell;
                }
            }
        }

        /// <summary>
        /// Gets the particles per cell.
        /// </summary>
        public int ParticlesPerCell
        {
            get; private set;
        }

        /// <summary>
        /// Gets the size of the particle.
        /// </summary>
        public int ParticleSize
        {
            get; private set;
        }

        protected virtual Vector2 GetBoundPosition()
        {
            // Determine a random position inside the parent's collider.
            var position = this.Position;

            // Make sure we're not overlapping any edge with particles except the top.
            var horizontalOffset = MathsHelper.Random(this.ParticleSize, this.Parent.Collider.BoundingBox.Width);
            var verticalOffset = MathsHelper.Random(this.Parent.Collider.BoundingBox.Height - this.ParticleSize);

            position.X += horizontalOffset;
            position.Y += verticalOffset;

            return position;
        }
    }
}
