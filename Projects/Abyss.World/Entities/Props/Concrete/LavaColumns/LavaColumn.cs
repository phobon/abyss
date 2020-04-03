﻿using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Physics;
using System.Collections;

namespace Abyss.World.Entities.Props.Concrete.LavaColumns
{
    public abstract class LavaColumn : Prop
    {
        protected Vector2 initialPosition;
        protected Vector2 startPosition;
        protected Vector2 endPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="LavaColumn" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="eruptionDirection">The eruption direction.</param>
        protected LavaColumn(Vector2 initialPosition, Rectangle boundingBox, Direction eruptionDirection)
            : base(
            "LavaColumn",
            "A dangerous column of lava that explodes from the edge of the world.", 
            initialPosition,
            boundingBox,
            Vector2.Zero)
        {
            this.Tags.Add("LavaColumn");
            this.EruptionDirection = eruptionDirection;

            // Setup lighting.
            this.Tags.Add(Lighting.DeferredRenderEntity);
            this.Flags[EngineFlag.DeferredRender] = true;
        }

        /// <summary>
        /// Gets the eruption direction.
        /// </summary>
        /// <value>
        /// The eruption direction.
        /// </value>
        public Direction EruptionDirection
        {
            get; private set;
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            if (!this.Components.ContainsKey(Lava.ComponentName))
            {
                var effect = ParticleEffectFactory.GetParticleEffect(
                    this,
                    Lava.ComponentName);
                this.Components.Add(Lava.ComponentName, effect);
            }

            base.Initialize();
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            this.Resume();
            CoroutineManager.Add(this.Id + "_Begin", this.BeginEffect());
            ((IParticleEffect)this.Components[Lava.ComponentName]).Emit();
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            base.ColliderOnCollision(args);

            var player = args.CollisionEntity as IPlayer;
            if (player != null)
            {
                // Player is killed by damage.
                player.TakeDamage(this.Name);
            }
        }

        protected abstract IEnumerator BeginEffect();
    }
}
