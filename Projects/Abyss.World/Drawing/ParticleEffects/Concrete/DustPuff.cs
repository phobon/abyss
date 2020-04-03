using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class DustPuff : ParticleEffect
    {
        public const string ComponentName = "DustPuff";

        /// <summary>
        /// Initializes a new instance of the <see cref="Fire" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        public DustPuff(IEntity parent, Vector2 offset) 
            : base(
            parent,
            "Dust Puff",
            "A puff of dust particle effect",
            1,
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
            var position = this.Position;
            var velocity = new Vector2(0f, -0.125f);
            var lifespan = MathsHelper.Random(20, 40);

            var sourceRect = new Rectangle(0, 0, ParticleEffectConstants.Large, ParticleEffectConstants.Large);

            var rotationSpeed = MathsHelper.NextFloat() / 10;
            return new Particle(this, position, velocity, 0f, rotationSpeed, this.Scale, lifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: true)
                {
                    Texture = TextureManager.GetColorTexture(Color.BlanchedAlmond)
                };
        }
    }
}
