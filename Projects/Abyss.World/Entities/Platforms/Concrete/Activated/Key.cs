using Abyss.World.Entities.Player;
using Occasus.Core;
using Occasus.Core.Entities;
using Occasus.Core.Physics;
using System.Collections;
using Occasus.Core.Maps;
using Occasus.Core.Maps.Definitions;
using Occasus.Core.States;

namespace Abyss.World.Entities.Platforms.Concrete.Activated
{
    public class Key : ActivatedPlatform, IConnectedPlatform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Key" /> class.
        /// </summary>
        /// <param name="platformDefinition">The platform definition.</param>
        public Key(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Key,
                "A platform that is impenetrable until it is fed a set amount of rift energy.",
                platformDefinition.Position,
                platformDefinition.Size)
        {
            this.Tags.Add(EntityTags.Key);
        }

        /// <summary>
        /// Gets or sets the connected identifier.
        /// </summary>
        /// <value>
        /// The connected identifier.
        /// </value>
        public string ConnectedId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the connected gate.
        /// </summary>
        /// <value>
        /// The connected gate.
        /// </value>
        public Gate ConnectedGate
        {
            get; set;
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Initialize the parent rift gate if required.
            if (!this.ConnectedGate.Flags[EngineFlag.Initialized])
            {
                this.ConnectedGate.Initialize();
            }
        }

        protected override void SetupStates()
        {
            var sprite = this.GetSprite();
            this.States.Add(PlatformStates.Deactivated, State.GenericState(PlatformStates.Deactivated, sprite));
            this.States.Add(PlatformStates.Activated, new State(PlatformStates.Activated, this.ActivatedEffect(), true));
            this.States.Add(PlatformStates.Activating, State.GenericState(PlatformStates.Activating, sprite));
            this.States.Add(PlatformStates.Deactivating, State.GenericState(PlatformStates.Deactivating, sprite));
            base.SetupStates();
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            if (!this.IsValidCollision(args))
            {
                return;
            }

            var player = args.CollisionEntity as IPlayer;
            if (player != null)
            {
                this.CurrentState = PlatformStates.Activated;
            }

            this.isActivated = true;
        }

        private IEnumerable ActivatedEffect()
        {
            // Activate this platform.
            this.GetSprite().GoToAnimation(PlatformStates.Activated);
            this.ConnectedGate.UnlockedKeys++;
            yield return null;
        }
    }
}
