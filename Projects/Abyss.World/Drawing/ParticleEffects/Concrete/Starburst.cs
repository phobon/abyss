using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Starburst : ParticleEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Starburst" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="color">The color.</param>
        public Starburst(IEntity parent, Vector2 offset, Color color)
            : base(
            parent,
            "Starburst",
            "Starburst Particle Effect",
            3,
            offset,
            Vector2.One)
        {
            this.Color = color;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get; private set;
        }

        /// <summary>
        /// Gets the particle.
        /// </summary>
        /// <returns>
        /// A new particle.
        /// </returns>
        public override IParticle GetParticle()
        {
            // Determine initial position by displacing the particles around the InitialPosition.
            var position = this.Position;
            position.X += MathsHelper.Random(-5, 5);
            position.Y += MathsHelper.Random(-5, 5);

            // Determine velocity.
            var velocity = new Vector2(MathsHelper.Random(-1, 2), MathsHelper.Random(-1, 2));

            // Determine lifespan.
            var lifespan = MathsHelper.Random(15, 60);

            var sourceRect = new Rectangle(0, 0, ParticleEffectConstants.Small, ParticleEffectConstants.Small);
            return new Particle(this, position, velocity, 0f, 0f, this.Scale, lifespan, sourceRect, recycle: true, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(Color.Purple)
            };
        }
    }
}
