using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Curse : ParticleEffect
    {
        public const string ComponentName = "Curse";

        private readonly Color curseColor = new Color(54, 0, 66);

        /// <summary>
        /// Initializes a new instance of the <see cref="Curse" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        public Curse(IEntity parent, Vector2 offset) 
            : base(
            parent,
            "Curse",
            "Curse Particle Effect",
            2,
            offset,
            Vector2.One)
        {
        }

        private const int PositionCoefficient = 5;

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
            var scaledPositionCoefficient = PositionCoefficient * this.Scale;
            position.X += MathsHelper.Random((int)-scaledPositionCoefficient.X, (int)scaledPositionCoefficient.X);

            // Determine velocity.
            var velocity = new Vector2(0f, -0.125f);

            var lifespan = MathsHelper.Random(5, 15);
            var particle = Atlas.GetAnimatedParticle(
                "gameplay",
                "Skull",
                this,
                position,
                velocity,
                0f,
                0f,
                this.Scale,
                recycle: true,
                fadeIn: true,
                fadeOut: true,
                frameDelay: lifespan,
                trackParent: false,
                shrink: false);
            particle.Color = curseColor;
            return particle;
        }
    }
}
