using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Entities.Player;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Assets;
using Occasus.Core.Components;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;
using System.Collections.Generic;
using Abyss.World.Phases;
using Occasus.Core.Drawing.Sprites;


namespace Abyss.World.Entities.Monsters
{
    public abstract class Monster : Entity, IMonster
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Monster" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="path">The path.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="zoneType">Type of the zone.</param>
        protected Monster(
            string name,
            string description,
            Vector2 initialPosition,
            IEnumerable<Vector2> path,
            Rectangle boundingBox,
            ZoneType zoneType)
            : base(name, description)
        {
            this.Transform.Position = initialPosition;
            this.Path = path;

            // Add tags.
            this.Tags.Add(EntityTags.Monster);

            // Add a sprite to this component.
            var sprite = Atlas.GetSprite(AtlasTags.Gameplay, this.Name, this);
            sprite.Layers["outline"].Opacity = 0f;
            this.Components.Add(Sprite.Tag, sprite);

            // Has collision.
            this.Collider = new Collider(this, boundingBox, Vector2.Zero)
                                {
                                    MovementSpeed = new Vector2(PhysicsManager.ActorMovementSpeeds[this.Name][ActorSpeed.Normal], PhysicsManager.ActorFallSpeeds[this.Name][ActorSpeed.Normal])
                                };
            this.Collider.Flags[PhysicsFlag.ReactsToPhysics] = true;
            this.Collider.Flags[PhysicsFlag.ReactsToGravity] = false;

            this.ZoneType = zoneType;

            // Setup lighting.
            this.Tags.Add(Lighting.DeferredRenderEntity);
            this.Flags[EngineFlag.DeferredRender] = true;
            this.Components.Add(LightSource.Tag, new PointLight(this, 0.5f, 2f, Color.White));
        }

        /// <summary>
        /// Gets the type of zone this Monster lives in.
        /// </summary>
        public ZoneType ZoneType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the path that this monster travels on.
        /// </summary>
        public IEnumerable<Vector2> Path
        {
            get;
            private set;
        }

        /// <summary>
        /// Triggers a movement decision.
        /// </summary>
        public virtual void TriggerMovementDecision()
        {
        }

        public void Die()
        {
            // Destroy this monster, it shouldn't hurt the player anymore!
            CoroutineManager.Add(this.Id + "_Die", this.DieEffect());
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Implosion effect.
            if (!this.Components.ContainsKey("ImplosionEffect"))
            {
                var effect = ParticleEffectFactory.GetParticleEffect(
                    this,
                    "Implosion",
                    new Vector2(
                        this.Collider.BoundingBox.Center.X,
                        this.Collider.BoundingBox.Bottom));
                this.Components.Add("ImplosionEffect", effect);
            }
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            base.ColliderOnCollision(args);

            if (args.CollisionType.Equals(CollisionTypes.Environment))
            {
                return;
            }

            var player = args.CollisionEntity as IPlayer;
            if (player != null)
            {
                // Handle Limbowalking phase.
                if (PhaseManager.IsPhaseActive(Phase.LimboWalking))
                {
                    player.TakeDamage(Phase.LimboWalking);
                    return;
                }

                // If the player lands on top of the monster; this should ideally just stun the monster rather than kill it, but kill it for now.
                if (args.CollisionPosition == CollisionPosition.Top)
                {
                    this.HandleStomp(player, args);
                    return;
                }

                // There are some things that will save a player for dying, but that happens in the Die method.
                if (player.TakeDamage(this.Name))
                {
                    this.Die();
                }
            }
        }

        protected virtual void HandleStomp(IPlayer player, CollisionEventArgs args)
        {
            // This method can be overridden based on the enemy type. Some enemies can't be stomped and will just kill the player; some will stun the enemy and so on.
            CoroutineManager.Add(this.Id + "_Stun", this.StunEffect());

            // Make the player stomp.
            var newArgs = new CollisionEventArgs(args.CollisionEntity, args.Rectangle, CollisionTypes.Monster);
            player.Stomp(newArgs);
            GameManager.StatisticManager.TotalScore += 50;
        }

        protected virtual IEnumerator DieEffect()
        {
            // Make it so this guy isn't able to be collided with anymore.
            this.Flags[EngineFlag.Collidable] = false;

            // Emit the impact effect.
            ((IParticleEffect)this.Components["ImplosionEffect"]).Emit();
            yield return Coroutines.Pause(20);
            yield return this.Transform.ScaleTo(Vector2.Zero, TimingHelper.GetFrameCount(0.8f), EaseType.CubeOut);
            this.End();
        }

        private IEnumerator StunEffect()
        {
            // Make it so this guy isn't able to be collided with anymore.
            this.Flags[EngineFlag.Collidable] = false;

            // Emit the impact effect.
            ((IParticleEffect)this.Components["ImplosionEffect"]).Emit();
            yield return this.GetSprite().FadeTo(0f, 5, Ease.Linear);
            yield return Coroutines.Pause(20);
            this.End();
        }
    }
}
