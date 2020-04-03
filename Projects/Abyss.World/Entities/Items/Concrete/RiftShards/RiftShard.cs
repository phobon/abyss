using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Abyss.World.Entities.Player;
using Abyss.World.Phases;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Audio;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.States;

namespace Abyss.World.Entities.Items.Concrete.RiftShards
{
    public abstract class RiftShard : Item
    {
        public const string Small = "SmallRiftShard";
        public const string Medium = "MediumRiftShard";
        public const string Large = "LargeRiftShard";

        /// <summary>
        /// Initializes a new instance of the <see cref="RiftShard"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="riftAmount">The rift amount.</param>
        protected RiftShard(
            string name, 
            string description, 
            Vector2 initialPosition, 
            Rectangle boundingBox, 
            int riftAmount) 
            : base(name, description, initialPosition, boundingBox)
        {
            this.RiftAmount = riftAmount;
            Monde.GameManager.DimensionShifted += ZoneSceneOnDimensionShifted;
        }

        /// <summary>
        /// Gets the rift amount.
        /// </summary>
        public int RiftAmount { get; private set; }

        /// <summary>
        /// Collects this item.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void Collect(IPlayer player)
        {
            // If the EmptyPockets phase is active, remove rift from the player, otherwise add it.
            if (PhaseManager.IsPhaseActive(Phase.EmptyPockets))
            {
                // Remove the RiftAmount to the Player's Rift if possible.
                player.Rift -= RiftAmount;
                Monde.GameManager.StatisticManager.RiftCollected -= RiftAmount;
            }
            else
            {
                // Add the RiftAmount to the Player's Rift if possible.
                player.Rift += RiftAmount;
                Monde.GameManager.StatisticManager.RiftCollected += RiftAmount;
                AudioManager.Play("itempickup");
            }

            Monde.GameManager.StatisticManager.TotalScore += RiftAmount;
            base.Collect(player);
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            if (Monde.GameManager.CurrentDimension == Dimension.Limbo)
            {
                this.FadeOut();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Monde.GameManager.DimensionShifted -= this.ZoneSceneOnDimensionShifted;
        }

        protected override void SetupStates()
        {
            var sprite = this.GetSprite();
            this.States.Add(RiftShardStates.Neutral, State.GenericState(RiftShardStates.Neutral, sprite));
            base.SetupStates();
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add(EntityTags.RiftCrystal);
        }

        protected override void InitializeSprite()
        {
            base.InitializeSprite();
            var sprite = this.GetSprite();
            sprite.Layers["outline"].Opacity = 0f;
        }

        protected override void InitializeLighting()
        {
            base.InitializeLighting();
            this.AddComponent(LightSource.Tag, new PointLight(this, 0.5f, 3f, UniverseConstants.NormalDimensionColor));
        }

        protected override void InitializeComponents()
        {
            var origin = new Vector2(this.Collider.BoundingBox.Center.X, this.Collider.BoundingBox.Top);
            this.AddComponent(Power.ComponentName, ParticleEffectFactory.GetParticleEffect(this, Power.ComponentName, origin));
        }

        private void ZoneSceneOnDimensionShifted(DimensionShiftedEventArgs dimensionShiftedEventArgs)
        {
            if (PhaseManager.IsPhaseActive(Phase.EmptyPockets))
            {
                return;
            }

            if (this.Collider != null && dimensionShiftedEventArgs.NewDimension == Dimension.Normal)
            {
                this.Flags[EngineFlag.Collidable] = true;
                this.FadeIn();
            }
            else
            {
                this.Flags[EngineFlag.Collidable] = false;
                this.FadeOut();
            }
        }
    }
}
