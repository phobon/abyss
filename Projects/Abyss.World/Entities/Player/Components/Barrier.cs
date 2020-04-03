using System.Collections;
using Occasus.Core;
using Occasus.Core.Assets;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;
using Occasus.Core.States;

namespace Abyss.World.Entities.Player.Components
{
    public class Barrier : Entity, IBarrier
    {
        private readonly IPlayer player;

        /// <summary>
        /// Initializes a new instance of the <see cref="Barrier"/> class.
        /// </summary>
        public Barrier(IPlayer player) 
            : base("barrier", "The barrier for a player.")
        {
            this.player = player;

            this.Components.Add(Sprite.Tag, Atlas.GetSprite(AtlasTags.Gameplay, this.Name, this));

            this.Tags.Add(Lighting.DeferredRenderEntity);
            this.Flags[EngineFlag.DeferredRender] = true;

            this.Charges = 0;
        }

        /// <summary>
        /// Gets the barrier charges.
        /// </summary>
        public int Charges
        {
            get; private set;
        }

        /// <summary>
        /// Generates the barrier.
        /// </summary>
        public void Generate(int charges)
        {
            if (this.CurrentState.Equals(BarrierStates.Inactive))
            {
                this.CurrentState = BarrierStates.Generate;
            }

            // Add shield charges.
            if (this.Charges < charges)
            {
                this.Charges = charges;
            }

            this.player.Flags[PlayerFlags.Shielded] = true;
        }

        /// <summary>
        /// Breaks the barrier.
        /// </summary>
        public void Break()
        {
            this.Charges--;

            if (this.Charges == 0)
            {
                if (this.CurrentState.Equals(BarrierStates.Pulsing))
                {
                    this.CurrentState = BarrierStates.Break;
                }

                this.player.Flags[PlayerFlags.Shielded] = false;
            }
        }

        /// <summary>
        /// Resets the barrier.
        /// </summary>
        public void Reset()
        {
            this.CurrentState = BarrierStates.Inactive;
            this.Charges = 0;
        }

        protected override void SetupStates()
        {
            var sprite = this.GetSprite();
            this.States.Add(BarrierStates.Inactive, State.GenericState(BarrierStates.Inactive, sprite));
            this.States.Add(BarrierStates.Generate, new State(BarrierStates.Generate, this.GenerateBarrier(), false));
            this.States.Add(BarrierStates.Pulsing, State.GenericState(BarrierStates.Pulsing, sprite));
            this.States.Add(BarrierStates.Break, new State(BarrierStates.Break, this.BreakBarrier(), false));
            base.SetupStates();
        }

        private IEnumerable GenerateBarrier()
        {
            var sprite = this.GetSprite();
            sprite.GoToAnimation(BarrierStates.Generate);
            yield return Coroutines.Pause(sprite.GetAnimationFrameLength(BarrierStates.Generate));

            this.CurrentState = BarrierStates.Pulsing;
        }

        private IEnumerable BreakBarrier()
        {
            this.GetSprite().GoToAnimation(BarrierStates.Break);
            yield return Coroutines.Pause(7);

            this.CurrentState = BarrierStates.Inactive;
        }
    }
}
