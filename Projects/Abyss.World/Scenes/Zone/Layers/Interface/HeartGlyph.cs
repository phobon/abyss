using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Interface;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;

using Occasus.Core.States;

namespace Abyss.World.Scenes.Zone.Layers.Interface
{
    public class HeartGlyph : InterfaceElement
    {
        private readonly string mendKey;
        private readonly string breakKey;
        private readonly string shakeKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartGlyph"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public HeartGlyph(Vector2 initialPosition) 
            : base(
            "HeartGlyph", 
            "Glyph for a Heart interface element.",
            initialPosition)
        {
            this.Flags[EngineFlag.DeferredBegin] = true;
            this.Transform.Scale = Vector2.Zero;

            this.mendKey = this.Id + "_Mend";
            this.breakKey = this.Id + "_Break";
            this.shakeKey = this.Id + "_End";
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            this.Mend();
        }

        /// <summary>
        /// Suspends the Engine Component from updating or drawing.
        /// </summary>
        public override void Suspend()
        {
            base.Suspend();
            this.Break();
        }

        /// <summary>
        /// Mends this haert.
        /// </summary>
        public void Mend()
        {
            CoroutineManager.Add(this.mendKey, this.MendEffect());
        }

        /// <summary>
        /// Breaks this heart.
        /// </summary>
        public void Break() 
        {
            CoroutineManager.Add(this.breakKey, this.BreakEffect());
            CoroutineManager.Add(this.shakeKey, this.ShakeEffect());
        }

        protected override void SetupStates()
        {
            var sprite = this.GetSprite();
            this.States.Add("Active", State.GenericState("Active", sprite));
            this.States.Add("Inactive", State.GenericState("Inactive", sprite));
            base.SetupStates();
        }

        private IEnumerator MendEffect()
        {
            // Scale this up into view.
            yield return this.Transform.ScaleTo(Vector2.One * DrawingManager.WindowScale, TimingHelper.GetFrameCount(0.8f), Ease.ElasticOut);
        }

        private IEnumerator ShakeEffect()
        {
            yield return this.Transform.Shake(2f, TimingHelper.GetFrameCount(1f));
        }

        private IEnumerator BreakEffect()
        {
            // Scale this down to nothing.
            yield return this.Transform.ScaleTo(Vector2.Zero, TimingHelper.GetFrameCount(0.4f), Ease.ElasticIn);
        }
    }
}
