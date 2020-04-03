using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Abyss.World.Entities.Items;
using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;
using System.Linq;
using Occasus.Core.States;

namespace Abyss.World.Entities.Props.Concrete
{
    public class TreasureChest : ActiveProp
    {
        public const string Cursed = "Cursed";

        private static readonly Rectangle boundingBox = new Rectangle(4, 10, 9, 6);

        private string cachedState;
        private Vector2 cachedPosition;
        private IItem treasureItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreasureChest" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public TreasureChest(Vector2 initialPosition)
            : base(
                "TreasureChest",
                "A special cache filled with exotic items and riches.",
                initialPosition,
                boundingBox,
                Vector2.Zero)
        {
            this.Tags.Add(EntityTags.TreasureChest);

            // Set up basic lighting.
            this.Tags.Add(Lighting.DeferredRenderEntity);
            this.Flags[EngineFlag.DeferredRender] = true;
            this.Components.Add(LightSource.Tag, new PointLight(this, 0.5f, 2f, Color.White));
        }

        /// <summary>
        /// Applies a curse to this treasure chest.
        /// </summary>
        public void ApplyCurse()
        {
            this.cachedState = this.CurrentState;
            this.CurrentState = Cursed;
        }

        /// <summary>
        /// Removes the curse from this treasure chest.
        /// </summary>
        public void RemoveCurse()
        {
            if (!string.IsNullOrEmpty(cachedState))
            {
                this.CurrentState = cachedState;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.StateChanged += delegate(StateChangedEventEventArgs args)
            {
                if (args.OldState.Equals(Cursed))
                {
                    if (this.Components.ContainsKey(Curse.ComponentName))
                    {
                        this.Components[Curse.ComponentName].Suspend();
                    }

                    CoroutineManager.Remove(this.Id + "_Shake");

                    this.CurrentState = this.cachedState;
                    this.cachedState = string.Empty;

                    this.Transform.Position = this.cachedPosition;

                    var sprite = this.GetSprite();
                    sprite.Color = Color.White;
                }
            };

            if (!this.Components.ContainsKey(Power.ComponentName))
            {
                this.Components.Add(
                    Power.ComponentName,
                    ParticleEffectFactory.GetParticleEffect(
                        this,
                        Power.ComponentName,
                        new Vector2(this.Collider.BoundingBox.Center.X, this.Collider.BoundingBox.Top),
                        Color.Gold));
            }

            if (!this.Components.ContainsKey(Curse.ComponentName))
            {
                this.Components.Add(
                    Curse.ComponentName,
                    ParticleEffectFactory.GetParticleEffect(
                        this,
                        Curse.ComponentName,
                        new Vector2(this.Collider.BoundingBox.Center.X, this.Collider.BoundingBox.Top)));
            }
        }

        /// <summary>
        /// Activates the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void Activate(IPlayer player)
        {
            base.Activate(player);

            this.GetSprite().GoToAnimation(PropStates.Activated);

            // Create a new item and set it on the Player.
            var item = ItemFactory.GetRandomTreasureItem(Vector2.Zero);
            item.Suspend();
            item.Collect(player);

            // Create an effect for opening this chest, so the player knows what they received.
            CoroutineManager.Add(this.Id + "_Activate", this.TreasureCollectedEffect(item));
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // Draw the treasure item if available.
            if (this.treasureItem != null && this.treasureItem.Flags[EngineFlag.Visible])
            {
                this.treasureItem.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            base.Update(gameTime, inputState);

            // Update the treasure item if available.
            if (this.treasureItem != null && this.treasureItem.Flags[EngineFlag.Active])
            {
                this.treasureItem.Update(gameTime, inputState);
            }
        }

        protected override void SetupStates()
        {
            base.SetupStates();

            this.States.Add(Cursed, new State(Cursed, this.CursedEffect(), false));
        }

        private IEnumerable CursedEffect()
        {
            var sprite = this.GetSprite();
            sprite.Color = new Color(54, 0, 66);

            // Emit the curse effect and shake this thing about.
            ((IParticleEffect)this.Components[Curse.ComponentName]).Emit();

            this.cachedPosition = this.Transform.Position;
            CoroutineManager.Add(this.Id + "_Shake", this.Transform.Shake(1f, TimingHelper.GetFrameCount(999f)));
            yield return null;
        }

        private IEnumerator TreasureCollectedEffect(IItem item)
        {
            // Turn off the particle effect.
            this.Components.Remove(Power.ComponentName);

            // TODO: Add a bouncy effect to the chest so it looks awesome when you open it.

            // Draw the treasure item.
            this.treasureItem = item;

            // If the item hasn't been Begun yet, do it.
            this.treasureItem.Begin();
            this.treasureItem.Transform.Position = this.Transform.Position;
            this.treasureItem.Resume();
            this.treasureItem.Flags[EngineFlag.Collidable] = false;

            var position = this.treasureItem.Transform.Position;
            position.Y -= DrawingManager.TileHeight;
            yield return this.treasureItem.Transform.MoveTo(position, TimingHelper.GetFrameCount(0.8f), EaseType.ElasticOut);

            // Fade it out and make it disappear.
            var sprite = (ISprite)this.treasureItem.Components.Values.Single(o => o is ISprite);
            yield return sprite.FadeTo(0f, 30, Ease.QuadOut);
            this.treasureItem.Suspend();
        }
    }
}
