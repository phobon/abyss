using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Assets;
using Occasus.Core.Components;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;
using Occasus.Core.States;

namespace Abyss.World.Entities.Props.Concrete
{
    public class VoidPatch : Entity, IProp
    {
        private readonly Color voidColor = new Color(54, 0, 66);

        private readonly Vector2 origin;
        private readonly Rectangle boundingBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoidPatch" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        public VoidPatch(Vector2 initialPosition, Rectangle boundingBox)
            : base("VoidPatch", "Patches of void energy that suck all matter inside it.")
        {
            this.Transform.Position = initialPosition;
            this.boundingBox = boundingBox;
            this.origin = new Vector2(boundingBox.Width / 2, boundingBox.Height / 2);
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            this.Resume();

            CoroutineManager.Add(this.Id + "_Shake", this.ShakeEffect());
            this.CurrentState = PropStates.Activating;
        }

        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public override void End()
        {
            this.CurrentState = PropStates.Deactivating;
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.GetColorTexture(voidColor), this.Transform.Position, this.Collider.BoundingBox, Color.White, this.Transform.Rotation, origin, this.Transform.Scale, SpriteEffects.None, 0f);
            base.Draw(gameTime, spriteBatch);
        }

        protected override void SetupStates()
        {
            this.States.Add(PropStates.Idle, new State(PropStates.Idle, this.IdleEffect(), false));
            this.States.Add(PropStates.Activating, new State(PropStates.Activating, this.BeginEffect(), true));
            this.States.Add(PropStates.Deactivating, new State(PropStates.Deactivating, this.EndEffect(), true));
            base.SetupStates();
        }

        protected override void InitializeTags()
        {
            this.Tags.Add(EntityTags.Prop);
            this.Tags.Add("VoidPatch");
        }

        protected override void InitializeCollider()
        {
            this.Collider = new Collider(this, boundingBox, origin)
            {
                MovementSpeed = new Vector2(PhysicsManager.BaseActorSpeed, PhysicsManager.BaseActorSpeed)
            };
            this.Transform.Scale = Vector2.Zero;
        }

        protected override void InitializeLighting()
        {
            this.Tags.Add(Lighting.DeferredRender);
            this.Flags[EngineFlag.DeferredRender] = true;
            this.AddComponent(LightSource.Tag, new PointLight(this, 1f, 3f, Color.Black));
        }

        protected override void InitializeComponents()
        {
            this.AddComponent(VoidSparkle.ComponentName, ParticleEffectFactory.GetParticleEffect(this, VoidSparkle.ComponentName));
        }

        /// <summary>
        /// Colliders the on collision.
        /// </summary>
        /// <param name="args">The <see cref="CollisionEventArgs"/> instance containing the event data.</param>
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

        private IEnumerable IdleEffect()
        {
            yield return null;
        }

        private IEnumerator ShakeEffect()
        {
            ((IParticleEffect)this.Components[VoidSparkle.ComponentName]).Emit();
            yield return this.Transform.Shake(1f, TimingHelper.GetFrameCount(999f));
        }

        private IEnumerable BeginEffect()
        {
            yield return this.Transform.ScaleTo(Vector2.One, TimingHelper.GetFrameCount(1f), EaseType.ElasticOut);
        }

        private IEnumerable EndEffect()
        {
            yield return this.Transform.ScaleTo(Vector2.Zero, TimingHelper.GetFrameCount(1f), EaseType.ExpoIn);
            CoroutineManager.Remove(this.Id + "_Shake");
            base.End();
        }
    }
}
