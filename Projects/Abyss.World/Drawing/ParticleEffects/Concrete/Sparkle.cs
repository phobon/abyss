using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Sparkle : ParticleEffect
    {
        public static string ComponentName = "Sparkle";

        /// <summary>
        /// Initializes a new instance of the <see cref="Sparkle" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        public Sparkle(IEntity parent, Vector2 offset)
            : base(
            parent,
            "Sparkle",
            "Sparkle Particle Effect",
            6,
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
            position.X += MathsHelper.Random(-4, 4);
            position.Y += MathsHelper.Random(-4, 4);
            
            // Determine lifespan.
            var lifespan = MathsHelper.Random(15, 60);

            // Determine size.
            var size = MathsHelper.Random() > 50 ? ParticleEffectConstants.Small : ParticleEffectConstants.Medium;
            var sourceRect = new Rectangle(0, 0, size, size);

            return new Particle(this, position, Vector2.Zero, 0f, 0f, this.Scale, lifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = MathsHelper.Random() > 50 ? TextureManager.GetColorTexture(Color.Gold) : TextureManager.GetColorTexture(Color.White)
            };
        }
    }
}
