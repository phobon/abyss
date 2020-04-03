using Microsoft.Xna.Framework;

using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Interface;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;

namespace Abyss.World.Scenes.Menu.Layers.Interface
{
    public class LogoElement : InterfaceElement
    {
        private static readonly Vector2 position = new Vector2(320f, 350f);

        private readonly string beginKey;
        private readonly string endKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoElement"/> class.
        /// </summary>
        public LogoElement() 
            : base(
            "LogoElement", 
            "Main interface element for the game.",
            position)
        {
            this.beginKey = this.Id + "_Begin";
            this.endKey = this.Id + "_End";
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.GetSprite().Opacity = 0f;
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
            yield return Coroutines.Pause(30);

            // Fade this UI element in.
            yield return this.SetOpacity(1f, 120, Ease.Linear);

            // Do a small hover just for a bit of dynamism.
            var top = this.Transform.Position;
            top.Y -= 5;
            var bottom = this.Transform.Position;
            while (true)
            {
                yield return this.Transform.MoveTo(top, 120, EaseType.QuadOut);
                yield return this.Transform.MoveTo(bottom, 120, EaseType.QuadOut);
            }
        }

        private IEnumerator EndEffect()
        {
            base.End();
            yield return null;
        }
    }
}
