using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Explosion : ParticleEffect
    {
        public const string ComponentName = "Explosion";

        /// <summary>
        /// Initializes a new instance of the <see cref="Explosion"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        public Explosion(IEntity parent, Vector2 offset)
            : base(parent, ComponentName, "An explosive particle effect", 1, offset, Vector2.One * 2)
        {
            this.Color = Color.OrangeRed;
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
                fadeOut: true,
                frameDelay: 2, 
                trackParent: true, 
                shrink: false);
            particle.Color = this.Color;
            particle.Offset = new Vector2(-8f, -8f);

            return particle;
        }
    }
}
