using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Drawing.ParticleEffects.Concrete
{
    public class VoidSparkle : BoundParticleEffect
    {
        public const string ComponentName = "VoidSparkle";

        private readonly int horizontalBounds;
        private readonly int verticalBounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoidSparkle" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public VoidSparkle(IEntity parent) 
            : base(
            parent,
            ComponentName,
            "Void sparkling effect",
            2,
            ParticleEffectConstants.Large)
        {
            this.horizontalBounds = this.Parent.Collider.BoundingBox.Width / 2;
            this.verticalBounds = this.Parent.Collider.BoundingBox.Height / 2;
        }

        /// <summary>
        /// Gets the particle.
        /// </summary>
        /// <returns>
        /// A new particle.
        /// </returns>
        public override IParticle GetParticle()
        {
            var frameDelay = MathsHelper.Random(7, 15);
            var particle = Atlas.GetAnimatedParticle(
                "gameplay",
                "ReverseSmallCrossSparkle",
                this,
                this.GetBoundPosition(),
                Vector2.Zero,
                0f,
                0f,
                this.Scale,
                recycle: true,
                fadeIn: true,
                fadeOut: true,
                frameDelay: frameDelay,
                trackParent: false,
                shrink: false);

            return particle;
        }

        protected override Vector2 GetBoundPosition()
        {
            // Determine a random position inside the parent's collider. Because this is centred, we need to do this a bit different.
            var position = this.Position;

            var horizontalOffset = MathsHelper.Random(-this.horizontalBounds + 1, this.horizontalBounds - 1);
            var verticalOffset = MathsHelper.Random(-this.verticalBounds + 1, this.verticalBounds - 1);

            position.X += horizontalOffset;
            position.Y += verticalOffset;

            return position;
        }
    }
}
