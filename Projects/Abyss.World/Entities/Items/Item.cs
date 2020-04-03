using System;
using System.Linq;
using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Entities.Player;
using Abyss.World.Phases;

using Microsoft.Xna.Framework;

using Occasus.Core;
using Occasus.Core.Assets;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Occasus.Core.Debugging.Components;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.States;

namespace Abyss.World.Entities.Items
{
    public abstract class Item : Entity, IItem
    {
        private const float HoverTime = 1f;
        private const string FadedFlag = "Faded";

        private readonly Rectangle boundingBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        protected Item(
            string name,
            string description,
            Vector2 initialPosition,
            Rectangle boundingBox)
            : base(name: name, description: description)
        {
            this.boundingBox = boundingBox;
            this.Transform.Position = initialPosition;
        }

        /// <summary>
        /// Occurs when the item is collected.
        /// </summary>
        public event EventHandler Collected;

        /// <summary>
        /// Collects this item.
        /// </summary>
        /// <param name="player">The player.</param>
        public virtual void Collect(IPlayer player)
        {
            this.OnCollected();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            // Remove any coroutines that are associated with this entity.
            CoroutineManager.Remove(this.Id + "_Hover");
            CoroutineManager.Remove(this.Id + "_Collect");

            base.Dispose();
        }

        /// <summary>
        /// Makes this item hover when required.
        /// </summary>
        public void Hover()
        {
            CoroutineManager.Add(this.Id + "_Hover", this.HoverEffect());
        }

        /// <summary>
        /// Fades the item out of the current dimension.
        /// </summary>
        public void FadeOut()
        {
            if (!this.Flags[FadedFlag])
            {
                //CoroutineManager.Add(this.FadeEffect(0.2f));
                var sprite = this.GetSprite();

                if (sprite != null)
                {
                    sprite.Layers["outline"].Opacity = 1f;
                    sprite.Layers.First().Value.Opacity = 0.2f;
                    this.Flags[FadedFlag] = true;
                }
            }
        }

        /// <summary>
        /// Fades the item into the current dimension.
        /// </summary>
        public virtual void FadeIn()
        {
            if (this.Flags[FadedFlag])
            {
                //CoroutineManager.Add(this.FadeEffect(1f));
                var sprite = this.GetSprite();

                if (sprite != null)
                {
                    sprite.Layers["outline"].Opacity = 0f;
                    sprite.Layers.First().Value.Opacity = 1f;
                    this.Flags[FadedFlag] = false;
                }
            }
        }

        protected override void SetupStates()
        {
            this.States.Add(ItemStates.Idle, State.GenericState(ItemStates.Idle, this.GetSprite()));
            base.SetupStates();
        }

        protected override void InitializeTags()
        {
            this.Tags.Add(EntityTags.Item);
        }

        protected override void InitializeFlags()
        {
            this.Flags.Add(FadedFlag, false);
        }

        protected override void InitializeSprite()
        {
            this.AddComponent(Sprite.Tag, Atlas.GetSprite(AtlasTags.Gameplay, this.Name, this));
        }

        protected override void InitializeCollider()
        {
            this.Collider = new Collider(this, this.boundingBox, null)
            {
                MovementSpeed = new Vector2(PhysicsManager.BaseActorSpeed, PhysicsManager.BaseActorSpeed)
            };
        }

        protected override void InitializeLighting()
        {
            this.Tags.Add(Lighting.DeferredRender);
            this.Flags[EngineFlag.DeferredRender] = true;
        }

        protected override void InitializeComponents()
        {
            // Add an effect to the screen so the item doesn't just disappear into oblivion.
            var effect = ParticleEffectFactory.GetParticleEffect(this, Sparkle.ComponentName, new Vector2(this.Collider.BoundingBox.Center.X, this.Collider.BoundingBox.Center.Y));
            this.AddComponent(Sparkle.ComponentName, effect);

#if DEBUG
            this.AddComponent("BoundingBox", new Border(this, Color.Yellow));
#endif
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            base.ColliderOnCollision(args);

            var player = args.CollisionEntity as IPlayer;
            if (player == null)
            {
                return;
            }

            if (PhaseManager.IsPhaseActive(Phase.EmptyPockets))
            {
                return;
            }

            this.Collect(player);
            CoroutineManager.Remove(this.Id + "_Hover");
            CoroutineManager.Add(this.Id + "_Collect", this.CollectEffect());
        }

        /// <summary>
        /// Coroutine that changes the hover direction of this 
        /// </summary>
        /// <returns>Hover coroutine enumerator.</returns>
        private IEnumerator HoverEffect()
        {
            var top = this.Transform.Position;
            top.Y -= 1;

            var bottom = this.Transform.Position;

            while (true)
            {
                yield return this.Transform.MoveTo(top, TimingHelper.GetFrameCount(HoverTime));
                yield return this.Transform.MoveTo(bottom, TimingHelper.GetFrameCount(HoverTime));
            }
        }

        /// <summary>
        /// Co-routine that applies a particle effect to this particular position.
        /// </summary>
        /// <returns>Collect effect co-routine.</returns>
        private IEnumerator CollectEffect()
        {
            foreach (var c in this.Components.Values)
            {
                c.End();
            }

            this.ClearComponents();
            var effect = (IParticleEffect) this.Components[Sparkle.ComponentName];
            effect.Emit();

            // Null out the collider so we don't have to worry about that anymore.
            this.Collider = null;

            yield return Coroutines.Pause(TimingHelper.GetFrameCount(0.5f));

            this.End();
        }

        private IEnumerator FadeEffect(float target)
        {
            yield return this.SetOpacity(target, 10, Ease.Linear);
        }

        private void OnCollected()
        {
            var handler = this.Collected;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
