using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Assets;
using Occasus.Core.Components;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using Occasus.Core.States;
using System.Collections;
using Occasus.Core.Drawing.Sprites;

namespace Abyss.World.Entities.Props
{
    public class ExclamationBubble : Entity, IExclamationBubble
    {
        private const string InactivateState = "Inactive";
        private const string AppearingState = "Appearing";
        private const string DisappearingState = "Disappearing";
        private const string PulsingState = "Pulsing";

        private readonly Vector2 initialPosition;
        private readonly Vector2 floatPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExclamationBubble"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public ExclamationBubble(IEntity parent) 
            : base("ExclamationBubble", "A bubble that contains some sort of exclamation.")
        {
            this.Parent = parent;
            var position = this.Parent.Transform.Position;
            position.X += 10;
            this.Transform.Position = position;

            this.initialPosition = position;
            position.Y -= 3;
            this.floatPosition = position;
        }

        protected override void InitializeTags()
        {
            this.Tags.Add("ExclamationBubble");
        }

        protected override void InitializeSprite()
        {
            var sprite = Atlas.GetSprite(AtlasTags.Gameplay, this.Name, this);
            sprite.Opacity = 0f;
            this.AddComponent(Sprite.Tag, sprite);
        }

        protected override void InitializeCollider()
        {
        }

        protected override void InitializeLighting()
        {
            this.Tags.Add(Lighting.DeferredRender);
            this.Flags[EngineFlag.DeferredRender] = true;
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        public IEntity Parent
        {
            get; private set;
        }

        /// <summary>
        /// Makes the bubble appear.
        /// </summary>
        public void Appear()
        {
            this.Resume();
            this.CurrentState = AppearingState;
        }

        /// <summary>
        /// Makes the bubble disappear.
        /// </summary>
        public void Disappear()
        {
            this.CurrentState = DisappearingState;
        }

        /// <summary>
        /// Resets the exclamation bubble.
        /// </summary>
        public void Reset()
        {
            this.CurrentState = InactivateState;
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.Reset();
        }

        protected override void SetupStates()
        {
            var sprite = this.GetSprite();
            this.States.Add(InactivateState, State.GenericState(InactivateState, sprite));
            this.States.Add(AppearingState, new State(AppearingState, this.ShowBubble(), false));
            this.States.Add(DisappearingState, new State(DisappearingState, this.HideBubble(), false));
            this.States.Add(PulsingState, new State(PulsingState, this.Pulse(), false));
            base.SetupStates();
        }

        private IEnumerable ShowBubble()
        {
            yield return this.GetSprite().FadeTo(1f, 30, Ease.Linear);

            // Start to pulse.
            this.CurrentState = PulsingState;
        }

        private IEnumerable HideBubble()
        {
            yield return this.GetSprite().FadeTo(0f, 30, Ease.Linear);
            yield return Coroutines.Pause(5);
            this.CurrentState = InactivateState;
            this.Suspend();
        }

        private IEnumerable Pulse()
        {
            while (true)
            {
                yield return this.Transform.MoveTo(floatPosition, 30);
                yield return this.Transform.MoveTo(initialPosition, 30);
            }
        }
    }
}
