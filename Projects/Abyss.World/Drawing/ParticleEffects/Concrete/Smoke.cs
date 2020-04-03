using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Smoke : ParticleEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Smoke" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        public Smoke(IEntity parent, Vector2 offset) 
            : base(
            parent,
            "Smoke",
            "Smoke Particle Effect",
            8,
            offset,
            Vector2.One)
        {
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
            position.X += MathsHelper.Random(-10, 10);
            position.Y += MathsHelper.Random(-10, 0);

            // Determine velocity.
            var velocity = new Vector2(0f, -0.5f);

            // Determine lifespan.
            var lifespan = MathsHelper.Random(15, 60);

            // Determine size.
            var sourceRect = new Rectangle(0, 0, ParticleEffectConstants.Small, ParticleEffectConstants.Small);

            return new Particle(this, position, velocity, 0f, 0f, this.Scale, lifespan, sourceRect, recycle: true, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
                {
                    Texture = TextureManager.GetColorTexture(Color.WhiteSmoke)
                };
        }
    }
}
