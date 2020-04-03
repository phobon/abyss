using System.Linq;
using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Abyss.World.Phases;
using Abyss.World.Phases.Concrete.Argus;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Drawing.ParticleEffects;

namespace Abyss.World.Entities.Props.Concrete
{
    public class Light : Prop
    {
        private static readonly Rectangle boundingBox = new Rectangle(5, 8, 6, 3);

        private readonly float intensity;
        private readonly float scale;

        /// <summary>
        /// Initializes a new instance of the <see cref="Light" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="color">The color.</param>
        /// <param name="intensity">The intensity.</param>
        /// <param name="scale">The scale.</param>
        public Light(Vector2 initialPosition, Color color, float intensity, float scale)
            : base(
            "Light", 
            "Provides a light source for the level.", 
            initialPosition,
            boundingBox,
            Vector2.Zero)
        {
            this.intensity = intensity;
            this.scale = scale;
            this.Color = color;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            if (PhaseManager.IsPhaseActive(Phase.Darkness))
            {
                this.Extinguish();
            }
            else
            {
                this.Ignite();
            }
        }

        public void Ignite()
        {
            // Set up all the different effects that the player requires.
            if (this.Components.ContainsKey(Fire.ComponentName))
            {
                ((IParticleEffect) this.Components[Fire.ComponentName]).Emit();
            }

            this.Components[LightSource.Tag].Resume();
        }

        public void Extinguish()
        {
            if (this.Components.ContainsKey(Fire.ComponentName))
            {
                this.Components[Fire.ComponentName].Suspend();
            }

            this.Components[LightSource.Tag].Suspend();
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add(EntityTags.Light);
        }

        protected override void InitializeLighting()
        {
            base.InitializeLighting();
            this.AddComponent(LightSource.Tag, new PointLight(this, intensity, scale, this.Color));
        }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            var origin = new Vector2(this.Collider.BoundingBox.Center.X, this.Collider.BoundingBox.Center.Y);
            this.AddComponent(Fire.ComponentName, ParticleEffectFactory.GetParticleEffect(this, Fire.ComponentName, origin));
        }
    }
}
