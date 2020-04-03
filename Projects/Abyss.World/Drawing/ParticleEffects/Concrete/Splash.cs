using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Splash : ParticleEffect
    {
        public const string ComponentName = "Splash";

        private readonly List<Vector2> velocities;

        public Splash(IEntity parent, Vector2 offset) 
            : base(
            parent,
            "Splash",
            "A splash of water.",
            15,
            offset,
            Vector2.One)
        {
            // Initialize velocities.
            this.velocities = new List<Vector2>
            {
                new Vector2(-0.125f, 0),
                new Vector2(-0.0625f, 0),
                new Vector2(0f, 0),
                new Vector2(0.0625f, 0),
                new Vector2(0.125f, 0)
            };
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
            position.X += MathsHelper.Random(-5, 4);
            position.Y += MathsHelper.Random(-4, -2);
            var velocity = this.velocities[MathsHelper.Random(this.velocities.Count)];
            velocity.Y = -(MathsHelper.NextFloat() / 6);
            var lifespan = MathsHelper.Random(20, 40);

            var sourceRect = new Rectangle(0, 0, ParticleEffectConstants.Massive, ParticleEffectConstants.Massive);

            return new Particle(this, position, velocity, 0f, 0f, this.Scale, lifespan, sourceRect, recycle: true, fadeIn: true, fadeOut: true, trackParent: false, shrink: true)
                {
                    Texture = TextureManager.GetColorTexture(Color.White)
                };
        }
    }
}
