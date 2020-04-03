using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Interface;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;

namespace Abyss.World.Scenes.Zone.Layers.Interface
{
    public class ScoreInterface : InterfaceElement
    {
        private static readonly Rectangle boundingBox = new Rectangle(0, 0, 20, 20);
        private static readonly Vector2 initialPosition = new Vector2(DrawingConstants.ScaledWindowWidth + 10, 54f);
        private static readonly Vector2 endPosition = new Vector2(DrawingConstants.ScaledWindowWidth - 82, 54f);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreInterface"/> class.
        /// </summary>
        public ScoreInterface() 
            : base(
            "ScoreInterface", 
            "Score interface element for the game.",
            initialPosition)
        {
            this.Flags[EngineFlag.DeferredBegin] = true;
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            CoroutineManager.Remove(this.Id + "_Begin");
            CoroutineManager.Add(this.Id + "_Begin", this.BeginEffect());
        }

        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public override void End()
        {
            CoroutineManager.Remove(this.Id + "_End");
            CoroutineManager.Add(this.Id + "_End", this.EndEffect());
        }

        private IEnumerator BeginEffect()
        {
            // Slide the UI into view.
            yield return this.Transform.MoveTo(endPosition, TimingHelper.GetFrameCount(0.5f), EaseType.QuadOut);
        }

        private IEnumerator EndEffect()
        {
            // Slide the UI into view.
            yield return this.Transform.MoveTo(initialPosition, TimingHelper.GetFrameCount(0.5f), EaseType.QuadOut);
            base.End();
        }
    }
}
