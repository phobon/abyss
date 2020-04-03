using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Maths;
using System.Collections.Generic;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Speck : FullScreenParticleEffect
    {
        private List<Vector2> velocities;

        /// <summary>
        /// Initializes a new instance of the <see cref="Speck"/> class.
        /// </summary>
        /// <param name="maximumDepth">The maximum depth.</param>
        /// <param name="particleDensity">The particle density.</param>
        /// <param name="color">The color.</param>
        public Speck(ParticleDensity particleDensity, Color color)
            : base(
            "Speck",
            "Speck particle effect",
            particleDensity)
        {
            this.Color = color;
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
            var velocity = MathsHelper.NextFloat();
            velocity -= 0.7f;
            if (velocity <= 0f)
            {
                velocity = 0.1f;
            }

            this.velocities = new List<Vector2>
                                {
                                    new Vector2(velocity, velocity),
                                    new Vector2(velocity, -velocity),
                                    new Vector2(-velocity, -velocity),
                                    new Vector2(-velocity, velocity)
                                };

            base.Initialize();
        }

        protected override IParticle GetParticle()
        {
            // Randomize this particle's lifespan.
            var lifespan = MathsHelper.Random(45, 180);

            return new Particle(null, Vector2.Zero, this.velocities[MathsHelper.Random(3)], 0f, 0f, Vector2.One, lifespan, new Rectangle(0, 0, ParticleEffectConstants.Small, ParticleEffectConstants.Small), recycle: this.Flags[ParticleEffectFlag.Recycle], fadeIn: true, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(this.Color)
            };
        }
    }
}
