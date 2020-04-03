using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Maths;
using System.Collections.Generic;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Aura : InterfaceParticleEffect
    {
        private readonly IDictionary<Direction, IList<Vector2>> velocities;

        private bool flipOriginEdge;

        /// <summary>
        /// Initializes a new instance of the <see cref="Aura"/> class.
        /// </summary>
        /// <param name="particleDensity">The particle density.</param>
        /// <param name="color">The color.</param>
        public Aura(ParticleDensity particleDensity, Color color)
            : base(
            "Aura",
            "Aura particle effect",
            particleDensity)
        {
            this.Color = color;
            this.velocities = new Dictionary<Direction, IList<Vector2>>
                                  {
                                      { Direction.Left, new List<Vector2>() },
                                      { Direction.Right, new List<Vector2>() }
                                  };
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            // Aura has 2 horizontal directions.
            for (var i = 0; i < 5; i++)
            {
                var horizontalVelocity = MathsHelper.NextFloat();
                this.velocities[Direction.Right].Add(new Vector2(horizontalVelocity, 0f));
                this.velocities[Direction.Left].Add(new Vector2(-horizontalVelocity, 0f));
            }
        }

        /// <summary>
        /// Gets the particle.
        /// </summary>
        /// <returns>
        /// A new particle.
        /// </returns>
        protected override IParticle GetParticle()
        {
            // Determine which edge this particle should be set to.
            var verticalPosition = MathsHelper.Random((int)DrawingManager.BaseResolutionHeight);
            var position = Vector2.Zero;
            var velocity = Vector2.Zero;
            var size = MathsHelper.Random() > 50 ? ParticleEffectConstants.Medium : ParticleEffectConstants.Small;
            if (!this.flipOriginEdge)
            {
                // Start from the left edge.
                position = new Vector2(0f, verticalPosition);
                velocity = this.velocities[Direction.Right][MathsHelper.Random(this.velocities[Direction.Right].Count)];
                this.flipOriginEdge = true;
            }
            else
            {
                // Start from the right edge.
                position = new Vector2(DrawingManager.BaseResolutionWidth - size, verticalPosition);
                velocity = this.velocities[Direction.Left][MathsHelper.Random(this.velocities[Direction.Left].Count)];
                this.flipOriginEdge = false;
            }

            return new Particle(null, position, velocity, 0f, 0f, Vector2.One, MathsHelper.Random(15, 60), new Rectangle(0, 0, size, size), recycle: this.Flags[ParticleEffectFlag.Recycle], fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(this.Color)
            };
        }
    }
}