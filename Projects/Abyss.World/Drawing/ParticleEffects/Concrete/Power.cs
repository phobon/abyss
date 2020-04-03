using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Power : ParticleEffect
    {
        public const string ComponentName = "Power";

        /// <summary>
        /// Initializes a new instance of the <see cref="Power" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="color">The color.</param>
        public Power(IEntity parent, Vector2 offset, Color color) 
            : base(
            parent,
            "Power",
            "Power Particle Effect",
            8,
            offset,
            Vector2.One)
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
        /// Gets the particle.
        /// </summary>
        /// <returns>
        /// A new particle.
        /// </returns>
        public override IParticle GetParticle()
        {
            // Determine initial position by displacing the particles around the InitialPosition.
            var position = this.Position;
            position.X += MathsHelper.Random(-5, 5);
            position.Y += MathsHelper.Random(-5, 0);

            var lifespan = MathsHelper.Random(15, 90);

            var size = MathsHelper.Random() > 50 ? ParticleEffectConstants.Medium : ParticleEffectConstants.Small;

            return new Particle(this, position, new Vector2(0f, -0.1f), 0f, 0f, this.Scale, lifespan, new Rectangle(0, 0, size, size), recycle: this.Flags[ParticleEffectFlag.Recycle], fadeIn: true, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(this.Color)
            };
        }
    }
}
