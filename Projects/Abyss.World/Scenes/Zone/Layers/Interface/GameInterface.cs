using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Interface;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;

namespace Abyss.World.Scenes.Zone.Layers.Interface
{
    public class GameInterface : InterfaceElement
    {
        private static readonly Vector2 initialPosition = new Vector2(0, -120f);
        private static readonly Vector2 endPosition = Vector2.Zero;

        private readonly string beginKey;
        private readonly string endKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInterface"/> class.
        /// </summary>
        public GameInterface() 
            : base(
            "GameInterface", 
            "Main interface element for the game.",
            initialPosition)
        {
            this.Flags[EngineFlag.DeferredBegin] = true;
            this.beginKey = this.Id + "_Begin";
            this.endKey = this.Id + "_End";
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            CoroutineManager.Add(this.beginKey, this.BeginEffect());
        }

        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public override void End()
        {
            CoroutineManager.Add(this.endKey, this.EndEffect());
        }

        private IEnumerator BeginEffect()
        {
            // Slide the UI into view.
            yield return this.Transform.MoveTo(endPosition, TimingHelper.GetFrameCount(1f), EaseType.QuadOut);
        }

        private IEnumerator EndEffect()
        {
            // Slide the UI into view.
            yield return this.Transform.MoveTo(initialPosition, TimingHelper.GetFrameCount(0.5f), EaseType.QuadOut);
            base.End();
        }
    }
}
