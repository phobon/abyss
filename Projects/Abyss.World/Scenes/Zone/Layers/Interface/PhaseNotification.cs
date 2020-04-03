using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Interface;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using System.Collections;

namespace Abyss.World.Scenes.Zone.Layers.Interface
{
    public class PhaseNotification : InterfaceElement
    {
        private static readonly Vector2 textPosition = new Vector2(320f, DrawingManager.ScaledWindowHeight - 183f);

        private static readonly Vector2 initialPosition = new Vector2(168f, DrawingManager.ScaledWindowHeight - 204f);

        private readonly string fadeInKey;
        private readonly string endKey;
        private readonly string fadeOutKey;

        private string phaseName;
        private Vector2 phaseNameCenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhaseNotification"/> class.
        /// </summary>
        public PhaseNotification()
            : base(
            "PhaseNotificationElement",
            "Phase notification element.",
            initialPosition)
        {
            this.Flags[EngineFlag.DeferredBegin] = true;
            var sprite = (ISprite)this.Components[Sprite.Tag];
            sprite.Opacity = 0f;

            this.fadeInKey = this.Id + "_FadeIn";
            this.fadeOutKey = this.Id + "_FadeOut";
            this.endKey = this.Id + "_End";
        }

        /// <summary>
        /// Gets or sets the name of the phase.
        /// </summary>
        /// <value>
        /// The name of the phase.
        /// </value>
        public string PhaseName
        {
            get
            {
                return this.phaseName;
            }

            set
            {
                if (!string.IsNullOrEmpty(this.phaseName) && this.phaseName.Equals(value))
                {
                    return;
                }

                this.phaseName = value;

                // Determine the size and the center of the text.
                var phaseNameSize = DrawingManager.Font.MeasureString(this.phaseName);
                this.phaseNameCenter = phaseNameSize * 0.5f;
            }
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            var sprite = (ISprite)this.Components[Sprite.Tag];
            spriteBatch.DrawString(DrawingManager.Font, this.PhaseName, new Vector2(textPosition.X - this.phaseNameCenter.X, textPosition.Y), Color.White * sprite.Opacity);
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            CoroutineManager.Add(this.fadeInKey, this.FadeInEffect());
        }

        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public override void End()
        {
            CoroutineManager.Add(this.endKey, this.EndEffect());
        }

        /// <summary>
        /// Fades the phase notification bar in.
        /// </summary>
        public void FadeIn()
        {
            this.Resume();
            CoroutineManager.Add(this.fadeInKey, this.FadeInEffect());
        }

        /// <summary>
        /// Fades the out.
        /// </summary>
        public void FadeOut()
        {
            CoroutineManager.Add(this.fadeOutKey, this.FadeOutEffect());
        }

        private IEnumerator FadeInEffect()
        {
            // Fade this UI element in.
            yield return this.SetOpacity(1f, 60, Ease.QuadIn);
        }

        private IEnumerator EndEffect()
        {
            yield return this.FadeOutEffect();
            base.End();
        }

        private IEnumerator FadeOutEffect()
        {
            // Fade this UI element out.
            yield return this.SetOpacity(0f, 30, Ease.QuadIn);
            this.Suspend();
        }
    }
}
