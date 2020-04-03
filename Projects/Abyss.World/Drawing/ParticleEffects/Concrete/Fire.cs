using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Fire : ParticleEffect
    {
        public const string ComponentName = "Fire";

        /// <summary>
        /// Initializes a new instance of the <see cref="Fire" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        public Fire(IEntity parent, Vector2 offset) 
            : base(
            parent,
            "Fire",
            "Fire Particle Effect",
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
            position.X += MathsHelper.Random(-2, 2);
            position.Y += MathsHelper.Random(-2, -1);

            // Determine velocity.
            var velocity = new Vector2(0f, -0.125f);

            //var lifespan = MathsHelper.Random(3, 10);
            //var particle = Atlas.GetAnimatedParticle(
            //    "gameplay",
            //    "SquareSparkle",
            //    this,
            //    position,
            //    velocity,
            //    0f,
            //    0f,
            //    1f,
            //    recycle: true,
            //    fadeIn: false,
            //    fadeOut: true,
            //    frameDelay: lifespan,
            //    trackParent: false,
            //    shrink: false);
            //particle.Color = MathsHelper.Random() > 50 ? Color.Red : Color.Yellow;
            //return particle;

            // Determine lifespan.
            var lifespan = MathsHelper.Random(15, 45);

            var sourceRect = new Rectangle(0, 0, ParticleEffectConstants.Medium, ParticleEffectConstants.Medium);

            return new Particle(this, position, velocity, 0f, 0f, this.Scale, lifespan, sourceRect, recycle: true, fadeIn: true, fadeOut: true, trackParent: false, shrink: false)
                {
                    Texture = MathsHelper.Random() > 50 ? TextureManager.GetColorTexture(Color.Red) : TextureManager.GetColorTexture(Color.Yellow)
                };
        }
    }
}
