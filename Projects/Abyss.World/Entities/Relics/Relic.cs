using System;
using System.Collections;
using Abyss.World.Entities.Items;
using Abyss.World.Entities.Player;
using Abyss.World.Phases;

using Microsoft.Xna.Framework;
using Occasus.Core.Components.Logic;
using Occasus.Core.Entities;
using System.Collections.Generic;
using Occasus.Core;

namespace Abyss.World.Entities.Relics
{
    public abstract class Relic : Item, IRelic
    {
#if DEBUG
        private const string RelicDebugKey = "Relic(s)";
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="Relic" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="relicType">Type of the relic.</param>
        /// <param name="relicActivationType">Type of the relic activation.</param>
        /// <param name="relevantEntityTags">The relevant entity tags.</param>
        /// <param name="activationChance">The activation chance.</param>
        /// <param name="lifeSpan">The life span.</param>
        protected Relic(
            string name,
            string description,
            Vector2 initialPosition,
            Rectangle boundingBox,
            RelicType relicType,
            RelicActivationType relicActivationType,
            IEnumerable<string> relevantEntityTags,
            int activationChance = 100,
            int lifeSpan = 0)
            : base(name, description, initialPosition, boundingBox)
        {
            this.Transform.Position = initialPosition;

            this.RelicType = relicType;
            this.RelicActivationType = relicActivationType;
            this.RelevantEntityTags = relevantEntityTags ?? new List<string>();
            foreach (var s in this.RelevantEntityTags)
            {
                if (s.Equals(EntityTags.All))
                {
                    this.UsesWholeEntityCache = true;
                    break;
                }
            }

            this.ActivationChance = activationChance;
            this.LifeSpan = lifeSpan;
        }

        /// <summary>
        /// Occurs when the relic is activated.
        /// </summary>
        public event EventHandler Activated;

        /// <summary>
        /// Occurs when the relic is deactivated.
        /// </summary>
        public event EventHandler Deactivated;

        /// <summary>
        /// Gets a value indicating whether this relic relates to the whole entity cache.
        /// </summary>
        public bool UsesWholeEntityCache
        {
            get; private set;
        }

        /// <summary>
        /// Gets the relevant entity tags.
        /// </summary>
        public IEnumerable<string> RelevantEntityTags { get; private set; }

        /// <summary>
        /// Gets the type of the relic.
        /// </summary>
        public RelicType RelicType
        {
            get; private set;
        }

        /// <summary>
        /// Gets the type of activation that this relic requires.
        /// </summary>
        public RelicActivationType RelicActivationType
        {
            get; private set;
        }

        /// <summary>
        /// Gets the life span of the relic in seconds.
        /// </summary>
        public int LifeSpan
        {
            get; protected set;
        }

        /// <summary>
        /// Gets the percentage chance that this relic will activate.
        /// </summary>
        public int ActivationChance
        {
            get; private set;
        }

        /// <summary>
        /// Collects this item.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void Collect(IPlayer player)
        {
            // If the CursedRelics phase is active, then the player is hurt.
            if (PhaseManager.IsPhaseActive(Phase.CursedRelics))
            {
                Monde.GameManager.Player.TakeDamage(Phase.CursedRelics);
                return;
            }

            // Set the relic collection in motion!
            // TODO: Is this really bad? I don't know.
            player.CollectRelic(this);

            base.Collect(player);
        }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public virtual void Activate(IEnumerable<IEntity> entityCache)
        {
#if DEBUG
            Monde.Debugger.Add(RelicDebugKey, this.Name);
#endif

            this.OnActivated();

            // Depending on the type of relic this is, we may need to schedule a deactivation. Do this using a coroutine.
            if (this.LifeSpan > 0)
            {
                CoroutineManager.Add(this.ActivationTimer(entityCache).GetEnumerator());
            }
        }

        /// <summary>
        /// Deactivates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public virtual void Deactivate(IEnumerable<IEntity> entityCache)
        {
#if DEBUG
            Monde.Debugger.Remove(RelicDebugKey);
#endif

            this.OnDeactivated();
        }

        protected override void InitializeTags()
        {
            this.Tags.Add(EntityTags.Relic);
        }

        private void OnActivated()
        {
            var handler = this.Activated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnDeactivated()
        {
            var handler = this.Deactivated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private IEnumerable ActivationTimer(IEnumerable<IEntity> entityCache)
        {
            yield return Coroutines.Pause(this.LifeSpan);
            this.Deactivate(entityCache);
        }
    }
}
