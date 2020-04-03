using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Implosion : ParticleEffect
    {
        public const string ComponentName = "Implosion";

        /// <summary>
        /// Initializes a new instance of the <see cref="Implosion"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        public Implosion(IEntity parent, Vector2 offset)
            : base(parent, ComponentName, "An implosive particle effect", 1, offset, Vector2.One)
        {
            this.Color = Color.White;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the particle.
        /// </summary>
        /// <returns>
        /// A new particle.
        /// </returns>
        public override IParticle GetParticle()
        {
            var particle = Atlas.GetAnimatedParticle(
                "gameplay",
                ComponentName,
                this,
                this.Parent.Transform.Position, 
                Vector2.Zero,
                0f,
                0f,
                this.Scale,
                recycle: false,
                fadeIn: false,
                fadeOut: false,
                frameDelay: 3,
                trackParent: true,
                shrink: false);
            particle.Color = this.Color;

            return particle;
        }
    }
}
