using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Microsoft.Xna.Framework;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Props.Concrete
{
    public class Waterfall : Prop
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Waterfall"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        public Waterfall(Vector2 initialPosition, Rectangle boundingBox)
            : base("Waterfall", "A flowing gush of water", initialPosition, boundingBox, Vector2.Zero)
        {
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            ((IParticleEffect)this.Components[Splash.ComponentName]).Emit();
        }

        protected override void InitializeSprite()
        {
            base.InitializeSprite();
            var sprite = this.GetSprite();
            sprite.Flags[SpriteFlag.CropToConstraints] = true;
            sprite.Flags[SpriteFlag.ScrollDown] = true;
        }

        protected override void InitializeComponents()
        {
            var origin = new Vector2(this.Collider.BoundingBox.Center.X, this.Collider.BoundingBox.Bottom);
            this.AddComponent(Splash.ComponentName, ParticleEffectFactory.GetParticleEffect(this, Splash.ComponentName, origin));
        }
    }
}
