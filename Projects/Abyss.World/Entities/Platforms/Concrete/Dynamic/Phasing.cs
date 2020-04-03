using Abyss.World.Entities.Player;
using Abyss.World.Universe;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Entities;
using Occasus.Core.States;
using System.Collections;

namespace Abyss.World.Entities.Platforms.Concrete.Dynamic
{
    public class Phasing : DynamicPlatform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phasing" /> class.
        /// </summary>
        /// <param name="platformDefinition">The platform definition.</param>
        public Phasing(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Phasing,
                "A mysterious energy that manifests only in the Balance dimension.",
                platformDefinition.Position,
                platformDefinition.Size)
        {
            this.Tags.Add(EntityTags.InterdimensionalPlatform);
            GameManager.DimensionShifted += this.ZoneSceneOnDimensionShifted;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            GameManager.DimensionShifted -= this.ZoneSceneOnDimensionShifted;
        }

        protected override void SetupStates()
        {
            var sprite = this.GetSprite();
            this.States.Add(PlatformStates.Activated, State.GenericState(PlatformStates.Activated, sprite));
            this.States.Add(PlatformStates.Activating, new State(PlatformStates.Activating, this.Activating(), false));
            this.States.Add(PlatformStates.Deactivating, new State(PlatformStates.Deactivating, this.Deactivating(), false));
            this.States.Add(PlatformStates.Deactivated, State.GenericState(PlatformStates.Deactivated, sprite));
            this.CurrentState = GameManager.CurrentDimension == Dimension.Normal ? PlatformStates.Activated : PlatformStates.Deactivated;
        }

        private void ZoneSceneOnDimensionShifted(DimensionShiftedEventArgs dimensionShiftedEventArgs)
        {
            if (!this.Flags[EngineFlag.Initialized])
            {
                return;
            }

            this.CurrentState = dimensionShiftedEventArgs.NewDimension == Dimension.Normal ? PlatformStates.Activating : PlatformStates.Deactivating;
        }

        private IEnumerable Activating()
        {
            this.Flags[EngineFlag.Collidable] = true;
            if (this.Components.ContainsKey("PowerEffect"))
            {
                this.Components["PowerEffect"].Resume();
            }

            var sprite = this.GetSprite();
            sprite.GoToAnimation(PlatformStates.Activating);
            yield return Coroutines.Pause(sprite.GetAnimationFrameLength(PlatformStates.Activating));

            this.CurrentState = PlatformStates.Activated;
        }

        private IEnumerable Deactivating()
        {
            this.Flags[EngineFlag.Collidable] = false;
            if (this.Components.ContainsKey("PowerEffect"))
            {
                this.Components["PowerEffect"].Suspend();
            }

            var sprite = this.GetSprite();
            sprite.GoToAnimation(PlatformStates.Deactivating);
            yield return Coroutines.Pause(sprite.GetAnimationFrameLength(PlatformStates.Deactivating));

            this.CurrentState = PlatformStates.Deactivated;
        }
    }
}
