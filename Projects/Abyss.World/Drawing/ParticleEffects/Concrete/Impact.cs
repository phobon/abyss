using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class Impact : ParticleEffect
    {
        public const string ComponentName = "Impact";

        private const int ParticleLifespan = 14;

        /// <summary>
        /// Initializes a new instance of the <see cref="Impact" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="offset">The offset.</param>
        public Impact(IEntity parent, Vector2 offset)
            : base(
            parent,
            ComponentName,
            "Impact Particle Effect",
            0,
            offset,
            Vector2.One)
        {
        }

        /// <summary>
        /// Pulses this particle effect.
        /// </summary>
        public override void Emit()
        {
            var sourceRect = new Rectangle(0, 0, ParticleEffectConstants.Large, ParticleEffectConstants.Large);
            var leftParticle = new Particle(this, this.Position, new Vector2(-1f, 0f), 0f, 0f, this.Scale, ParticleLifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(Color.WhiteSmoke)
            };
            var rightParticle = new Particle(this, this.Position, new Vector2(1f, 0f), 0f, 0f, this.Scale, ParticleLifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(Color.WhiteSmoke)
            };

            var diagonalLeftParticle = new Particle(this, this.Position, new Vector2(-1f, -1f), 0f, 0f, this.Scale, ParticleLifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(Color.WhiteSmoke)
            };
            var diagonalRightParticle = new Particle(this, this.Position, new Vector2(1f, -1f), 0f, 0f, this.Scale, ParticleLifespan, sourceRect, recycle: false, fadeIn: false, fadeOut: true, trackParent: false, shrink: false)
            {
                Texture = TextureManager.GetColorTexture(Color.WhiteSmoke)
            };

            this.particleCloud.Add(leftParticle);
            this.particleCloud.Add(rightParticle);
            this.particleCloud.Add(diagonalLeftParticle);
            this.particleCloud.Add(diagonalRightParticle);
        }
    }
}
