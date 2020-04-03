using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class DimensionMerge : ParticleEffect
    {
        private const int ParticleLifespan = 14;

        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionSplit"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        public DimensionMerge(IEntity parent, Vector2 offset)
            : base(
            parent,
            "DimensionMerge",
            "Dimension Merge Particle Effect",
            0,
            offset,
            Vector2.One)
        {
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get; set;
        }

        /// <summary>
        /// Pulses this particle effect.
        /// </summary>
        public override void Emit()
        {
            var sourceRect = new Rectangle(0, 0, ParticleEffectConstants.Large, ParticleEffectConstants.Large);
            var leftDownParticle = new Particle(this, this.Position, new Vector2(-1f, 1f), 0f, 0f, this.Scale, ParticleLifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(this.Color)
            };
            var rightDownParticle = new Particle(this, this.Position, new Vector2(1f, 1f), 0f, 0f, this.Scale, ParticleLifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(this.Color)
            };

            var leftUpParticle = new Particle(this, this.Position, new Vector2(-1f, -1f), 0f, 0f, this.Scale, ParticleLifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(this.Color)
            };
            var rightUpParticle = new Particle(this, this.Position, new Vector2(1f, -1f), 0f, 0f, this.Scale, ParticleLifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(this.Color)
            };

            this.particleCloud.Add(leftDownParticle);
            this.particleCloud.Add(rightDownParticle);
            this.particleCloud.Add(leftUpParticle);
            this.particleCloud.Add(rightUpParticle);
        }
    }
}
