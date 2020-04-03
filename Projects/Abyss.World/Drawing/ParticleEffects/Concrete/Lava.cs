using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Lava : BoundParticleEffect
    {
        public const string ComponentName = "Lava";

        private static readonly Color lavaColor = new Color(240, 186, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Lava" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Lava(IEntity parent) 
            : base(
            parent,
            ComponentName,
            "Lava particle effect",
            2,
            ParticleEffectConstants.Large)
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
            var velocity = new Vector2(0f, -(MathsHelper.NextFloat() / 10));
            var lifespan = MathsHelper.Random(60, 120);

            var sourceRect = new Rectangle(0, 0, this.ParticleSize, this.ParticleSize);

            return new Particle(this, this.GetBoundPosition(), velocity, 0f, 0f, this.Scale, lifespan, sourceRect, recycle: true, fadeIn: false, fadeOut: true, trackParent: false, shrink: true)
                {
                    Texture = TextureManager.GetColorTexture(lavaColor)
                };
        }
    }
}
