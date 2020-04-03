using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Maths;
using System.Collections.Generic;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Snow : FullScreenParticleEffect
    {
        private const int MaximumLifespan = 5;

        private List<Vector2> velocities;

        /// <summary>
        /// Initializes a new instance of the <see cref="Snow"/> class.
        /// </summary>
        /// <param name="particleDensity">The particle density.</param>
        /// <param name="color">The color.</param>
        public Snow(ParticleDensity particleDensity, Color color)
            : base(
            "Snow",
            "Snow particle effect",
            particleDensity)
        {
            this.Color = color;
            this.Flags[ParticleEffectFlag.RecycleToViewPort] = true;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        public Color Color
        {
            get; private set;
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            var verticalVelocity = MathsHelper.NextFloat() * 2;
            var horizontalVelocity = MathsHelper.NextFloat();
            horizontalVelocity -= 0.7f;
            if (horizontalVelocity <= 0f)
            {
                horizontalVelocity = 0.1f;
            }

            this.velocities = new List<Vector2>
                                {
                                    new Vector2(horizontalVelocity, verticalVelocity),
                                    new Vector2(0f, verticalVelocity),
                                    new Vector2(-horizontalVelocity, verticalVelocity)
                                };

            base.Initialize();
        }

        /// <summary>
        /// Gets the particle.
        /// </summary>
        /// <returns>
        /// A new particle.
        /// </returns>
        protected override IParticle GetParticle()
        {
            // Randomize this particle's lifespan.
            var lifespan = MathsHelper.Random(60, 300);
            return new Particle(null, Vector2.Zero, velocities[MathsHelper.Random(this.velocities.Count)], 0f, 0f, Vector2.One, lifespan, new Rectangle(0, 0, ParticleEffectConstants.Small, ParticleEffectConstants.Small), recycle: this.Flags[ParticleEffectFlag.Recycle], fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(this.Color)
            };
        }
    }
}

