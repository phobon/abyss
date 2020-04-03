
using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Abyss.World.Entities.Props;
using Abyss.World.Entities.Relics;
using Abyss.World.Phases;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Occasus.Core;
using Occasus.Core.Assets;
using Occasus.Core.Audio;
using Occasus.Core.Components;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Images;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using Occasus.Core.States;
using System;
using System.Collections;
using Abyss.World.Entities.Player.Components;
using Occasus.Core.Drawing.Sprites;

namespace Abyss.World.Entities.Player
{
    public class Player : Entity, IPlayer
    {
        private const float DeathTime = 1f;
        //private const string MoveKey = "Player_Move";

        private static readonly Rectangle boundingBox = new Rectangle(3, 0, 9, 16);

        private int playerPosition = 3;

        private bool isMoving;

        private int lives;
        private int rift;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
            : base(
            "Player",
            "The main player of the game.")
        {
            // Add tags.
            this.Tags.Add(EntityTags.Player);

            // Initialize player flags.
            this.Flags.Add(PlayerFlags.Invulnerable, false);
            this.Flags.Add(PlayerFlags.Shielded, false);

            // Add sprites.
            this.Components.Add(Sprite.Tag, Atlas.GetSprite(AtlasTags.Gameplay, "Player", this));

            // Create a new phase gem.
            this.PhaseGem = new PhaseGem(this);

            // Create a new shield.
            this.Barrier = new Barrier(this);

            // Has collision.
            this.MovementSpeed = (int)PhysicsManager.ActorMovementSpeeds[this.Name][ActorSpeed.Normal];
            this.FallSpeed = (int)PhysicsManager.ActorFallSpeeds[this.Name][ActorSpeed.Normal];
            this.Collider = new Collider(this, boundingBox, Vector2.Zero)
                                {
                                    MovementSpeed = new Vector2(this.MovementSpeed, this.FallSpeed)
                                };
            this.Collider.Flags[PhysicsFlag.ReactsToPhysics] = true;
            this.Collider.Flags[PhysicsFlag.ReactsToGravity] = true;

            // Setup lighting.
            this.Tags.Add(Lighting.DeferredRenderEntity);
            this.Flags[EngineFlag.DeferredRender] = true;
            this.Components.Add(LightSource.Tag, new PointLight(this, 0.8f, 2f, Color.White));
        }

        /// <summary>
        /// Occurs when the player loses a life.
        /// </summary>
        public event StatChangedEventHandler LifeLost;

        /// <summary>
        /// Occurs when the player collects rift.
        /// </summary>
        public event StatChangedEventHandler RiftCollected;

        /// <summary>
        /// Occurs when the player stomps a monster.
        /// </summary>
        public event EventHandler MonsterStomped;

        /// <summary>
        /// Occurs when the player stomps a platform.
        /// </summary>
        public event PlatformStompedEventHandler PlatformStomped;

        /// <summary>
        /// Occurs when the player gains a life.
        /// </summary>
        public event StatChangedEventHandler LifeGained;

        /// <summary>
        /// Gets the Phase Gem that the player uses to switch dimensions.
        /// </summary>
        public IPhaseGem PhaseGem
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the shield that protects the player.
        /// </summary>
        public IBarrier Barrier
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Player's current Rift.
        /// </summary>
        /// <remarks>
        /// Rift is the Player's main resource. It is used to buy items and other things.
        /// </remarks>
        public int Rift
        {
            get
            {
                return this.rift;
            }

            set
            {
                if (this.rift == value)
                {
                    return;
                }

                // If the player tries to collect more than the maximum rift they can carry, then just set it to the maximum rift.
                if (value >= this.MaximumRift)
                {
                    value = this.MaximumRift;
                }

                this.rift = value;
                var riftDifference = value - rift;
                this.OnRiftCollected(riftDifference);
            }
        }

        /// <summary>
        /// Gets or sets the maximum rift a player can carry.
        /// </summary>
        /// <value>
        /// The maximum rift.
        /// </value>
        public int MaximumRift
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum lives.
        /// </summary>
        /// <value>
        /// The maximum lives.
        /// </value>
        public int MaximumLives
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Player's remaining lives.
        /// </summary>
        /// <value>
        /// Lives are another of the Player's main resources. When the Player runs out of lives, the game is over.
        /// </value>
        public int Lives
        {
            get
            {
                return this.lives;
            }

            set
            {
                if (this.lives == value)
                {
                    return;
                }

                // Check whether the new value is greater than the maximum number of lives the player has.
                if (value > this.MaximumLives)
                {
                    return;
                }

                // Player has gained a life, so fire off that event.
                if (this.lives < value)
                {
                    this.OnLifeGained(value);
                }
                else
                {
                    // Player has lost a life, so fire that event.
                    this.OnLifeLost(value);
                }

                this.lives = value;

                // If the player has no lives left, it's game over.
                if (this.lives == 0)
                {
                    // TODO: Coroutine to transition out of this scene.
                    Engine.ActivateScene("GameOver");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Player's movement speed.
        /// </summary>
        /// <value>
        /// The movement speed.
        /// </value>
        public int MovementSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fall speed.
        /// </summary>
        /// <value>
        /// The fall speed.
        /// </value>
        public int FallSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time the player can spend in Limbo.
        /// </summary>
        /// <value>
        /// The time that the player can spend in Limbo.
        /// </value>
        public int LimboTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current prop.
        /// </summary>
        /// <value>
        /// The current prop.
        /// </value>
        public IActiveProp CurrentProp
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the player for a new game.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.Flags[EngineFlag.Collidable] = true;
            this.LimboTime = 60;
            this.Rift = 0;
            this.MaximumLives = 1;
            this.MaximumRift = 100;
            this.lives = 1;
            this.Transform.Position = GameData.PlayerStart;

            var sprite = this.GetSprite();
            var hairLayer = sprite.Layers["hair"];
            hairLayer.Color = UniverseConstants.NormalDimensionColor;

            var outlineLayer = sprite.Layers["outline"];
            outlineLayer.Color = Color.Transparent;

            // Set up all the different effects that the player requires.
            if (!this.Components.ContainsKey(Impact.ComponentName))
            {
                var effect = ParticleEffectFactory.GetParticleEffect(
                    this,
                    Impact.ComponentName,
                    new Vector2(
                        this.Collider.BoundingBox.Center.X,
                        this.Collider.BoundingBox.Bottom - 1));
                this.Components.Add(Impact.ComponentName, effect);
            }

            // Explosion effect.
            if (!this.Components.ContainsKey(Explosion.ComponentName))
            {
                var effect = ParticleEffectFactory.GetParticleEffect(
                    this,
                    Explosion.ComponentName,
                    new Vector2(
                        this.Collider.BoundingBox.Center.X,
                        this.Collider.BoundingBox.Bottom - 1));
                this.Components.Add(Explosion.ComponentName, effect);
            }

            // Implosion effect.
            if (!this.Components.ContainsKey(Implosion.ComponentName))
            {
                var effect = ParticleEffectFactory.GetParticleEffect(
                    this,
                    Implosion.ComponentName,
                    new Vector2(
                        this.Collider.BoundingBox.Center.X,
                        this.Collider.BoundingBox.Bottom - 1));
                this.Components.Add(Implosion.ComponentName, effect);
            }

            // Puff of dust effect.
            if (!this.Components.ContainsKey(DustPuff.ComponentName))
            {
                var effect = ParticleEffectFactory.GetParticleEffect(
                    this,
                    DustPuff.ComponentName,
                    new Vector2(
                        this.Collider.BoundingBox.Center.X,
                        this.Collider.BoundingBox.Bottom - 1));
                this.Components.Add(DustPuff.ComponentName, effect);
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

            // Make sure the shield matches the Player's position.
            this.Barrier.Transform.Position = this.Transform.Position;
        }

        /// <summary>
        /// Player takes damage.
        /// </summary>
        /// <param name="damageSource">The damage source.</param>
        /// <returns>
        /// True if the player takes damage; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Depending on the current PlayerFlags, this method can spawn off a variety of different outcomes.
        /// </remarks>
        public bool TakeDamage(string damageSource)
        {
            // Check if the player is invulnerable. If so, nothing should happen, except in a limbo walking phase.
            if (this.Flags[PlayerFlags.Invulnerable] && !damageSource.Equals(Phase.LimboWalking))
            {
                return false;
            }

            // If the player is shielded, there are a couple of things that have to happen; the shield bursts, the player becomes invulnerable for a set amount of time, etc.
            if (this.Flags[PlayerFlags.Shielded])
            {
                CoroutineManager.Add(this.ShieldLost());
                return true;
            }

            // Go to the death state.
            this.CurrentState = PlayerStates.Die;

            // Update the statistics.
            GameManager.StatisticManager.Deaths.Add(damageSource);

            return true;
        }

        /// <summary>
        /// Collects the relic.
        /// </summary>
        /// <param name="relic">The relic.</param>
        public void CollectRelic(IRelic relic)
        {
            // If the Malfunction phase is active, the player is unable to activate this relic.
            if (PhaseManager.IsPhaseActive(Phase.Malfunction))
            {
                return;
            }

            // Add the relic to the collection.
            GameManager.RelicCollection.AddRelic(relic);
        }

        /// <summary>
        /// Player stomps the ground or a monster.
        /// </summary>
        /// <param name="args"></param>
        public void Stomp(CollisionEventArgs args)
        {
            // If the collision came from a monster, fire off a monster split event.
            if (args.CollisionType.Equals(CollisionTypes.Monster))
            {
                // Handle Limbowalking phase.
                if (PhaseManager.IsPhaseActive(Phase.LimboWalking))
                {
                    this.TakeDamage(Phase.LimboWalking);
                    return;
                }

                this.OnMonsterStomped();
                this.CurrentState = PlayerStates.MonsterStomp;

                // Reward the player with a phase gem charge.
                this.PhaseGem.Generate();
            }
            else
            {
                // This is an environment, so fire off a platform stomped event.
                this.OnPlatformStomped(new PlatformStompedEventArgs(args.Rectangle));
                GameManager.RelicCollection.ActivateRelics(RelicActivationType.Stomp);

                this.CurrentState = PlayerStates.Impact;
            }

            if (this.PhaseGem.Charges == 0)
            {
                CoroutineManager.Add(this.DimensionShiftCooldown(0));
            }
        }

        public void GainInvulnerability(int totalFrames)
        {
            if (this.Flags[PlayerFlags.Invulnerable])
            {
                return;
            }

            CoroutineManager.Add(this.Id + "_Invulnerable", this.InvulnerabilityEffect(totalFrames));
        }

        /// <summary>
        /// Handles the input.
        /// </summary>
        /// <param name="inputState">State of the input.</param>
        public void HandleInput(IInputState inputState)
        {
            if (!GameManager.Player.Flags[EngineFlag.Active])
            {
                return;
            }

            PlayerIndex playerIndex;

#if DEBUG
            // Shield states.
            if (inputState.IsNewKeyPress(Keys.G, PlayerIndex.One, out playerIndex))
            {
                this.Barrier.Generate(1);
            }
            else if (inputState.IsNewKeyPress(Keys.H, PlayerIndex.One, out playerIndex))
            {
                this.Barrier.Break();
            }
#endif

            // Check analog movement first.
            var gamepadState = inputState.GetGamePadState(PlayerIndex.One);
            var playerSprite = this.GetSprite();
            if (!gamepadState.ThumbSticks.Left.X.Equals(0f))
            {
                this.Collider.MovementFactor = gamepadState.ThumbSticks.Left;
                playerSprite.SpriteEffects = gamepadState.ThumbSticks.Left.X > 0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                this.CurrentState = this.Collider.Flags[PhysicsFlag.Grounded] ? PlayerStates.Run : PlayerStates.Fall;
            }
            else if (inputState.IsKeyPressed(Keys.Left, PlayerIndex.One, out playerIndex))
            {
                //this.MoveLeft();
                playerSprite.SpriteEffects = SpriteEffects.None;
                this.Collider.MovementFactor = new Vector2(-1f, 0);
                this.CurrentState = this.Collider.Flags[PhysicsFlag.Grounded] ? PlayerStates.Run : PlayerStates.Fall;
            }
            else if (inputState.IsKeyPressed(Keys.Right, PlayerIndex.One, out playerIndex))
            {
                //this.MoveRight();
                playerSprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                this.Collider.MovementFactor = new Vector2(1f, 0);
                this.CurrentState = this.Collider.Flags[PhysicsFlag.Grounded] ? PlayerStates.Run : PlayerStates.Fall;
            }
            else
            {
                // This is a fallthrough for all movement.
                this.Collider.MovementFactor = Vector2.Zero;
                this.CurrentState = this.Collider.Flags[PhysicsFlag.Grounded] ? PlayerStates.Idle : PlayerStates.Fall;
            }

            // Check input flags. I really want this to be a "one-button" game, so everything is handled through Keys.Space (tap on screen/press A).
            if (inputState.IsNewKeyPress(Keys.Space, PlayerIndex.One, out playerIndex) || inputState.IsNewButtonPress(Buttons.A, PlayerIndex.One, out playerIndex))
            {
                var prop = this.CurrentProp;
                if (prop != null && prop.CanActivate)
                {
                    prop.Activate(GameManager.Player);
                    return;
                }

                this.PhaseGem.Use();
            }

            while (TouchPanel.IsGestureAvailable)
            {
                var gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    case GestureType.Flick:
                        break;
                    case GestureType.Tap:
                        break;
                    case GestureType.VerticalDrag:
                        break;
                    case GestureType.HorizontalDrag:
                        break;
                }
            }
        }

        public void ShowOutline(Color color, bool pulse)
        {
            var spriteLayer = this.GetSprite().Layers["outline"];
            spriteLayer.Color = color;

            if (pulse)
            {
                // Coroutine to pulse this.
                CoroutineManager.Coroutines.Remove(this.Id + "_OutlinePulse");
                CoroutineManager.Coroutines.Add(this.Id + "_OutlinePulse", spriteLayer.Pulse(60));
            }
        }

        public void HideOutline()
        {
            CoroutineManager.Coroutines.Remove(this.Id + "_OutlinePulse");

            var sprite = this.GetSprite();
            sprite.Layers["outline"].Color = Color.Transparent;
            sprite.Layers["outline"].Opacity = 0f;
        }

        //public bool MoveLeft()
        //{
        //    if (this.isMoving || this.playerPosition == UniverseConstants.PlayerPositionLowConstraint)
        //    {
        //        return false;
        //    }

        //    // Add the move left coroutine.
        //    CoroutineManager.Add(MoveKey, this.MovePlayerLeft().GetEnumerator());

        //    return true;
        //}

        //public bool MoveRight()
        //{
        //    if (this.isMoving || this.playerPosition == UniverseConstants.PlayerPositionHighConstraint)
        //    {
        //        return false;
        //    }

        //    // Add the move right coroutine.
        //    CoroutineManager.Add(MoveKey, this.MovePlayerRight().GetEnumerator());

        //    return true;
        //}

        protected override void SetupStates()
        {
            var sprites = this.GetSprites();

            // Generic states.
            this.States.Add(PlayerStates.Idle, State.GenericState(PlayerStates.Idle, sprites));
            this.States.Add(PlayerStates.Run, new State(PlayerStates.Run, this.RunEffect(), false));
            this.States.Add(PlayerStates.Fall, State.GenericState(PlayerStates.Fall, sprites));

            // Complex states.
            this.States.Add(PlayerStates.Impact, new State(PlayerStates.Impact, this.ImpactEffect("land"), true));
            this.States.Add(PlayerStates.MonsterStomp, new State(PlayerStates.MonsterStomp, this.ImpactEffect("monsterstomped"), true));
            this.States.Add(PlayerStates.Die, new State(PlayerStates.Die, this.Die(), true));
            this.States.Add(PlayerStates.ShiftDimension, new State(PlayerStates.ShiftDimension, this.ShiftDimension(), true));
            base.SetupStates();
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            base.ColliderOnCollision(args);

            if (args.CollisionType.Equals(CollisionTypes.Environment))
            {
                // TODO: Handle collisions with things to the left and right.
                if ((args.CollisionPosition == CollisionPosition.Right ||
                     args.CollisionPosition == CollisionPosition.Left))// && this.isMoving)
                {
                    //if (this.isMoving)
                    //{
                    //CoroutineManager.Add(this.Bonk().GetEnumerator());
                    //}
                }
                else
                {
                    this.Stomp(args);
                }
            }
            else
            {
                // Reset the player's ungrounded frames.
                this.Collider.UngroundedFrames = 0;
            }
        }

        //private float moveOrigin;

        //// TODO: It might be nicer to ease this at some point.
        //private IEnumerable MovePlayerLeft()
        //{
        //    this.isMoving = true;
        //    this.moveOrigin = this.Transform.Position.X;
        //    var current = moveOrigin;

        //    this.playerPosition--;
        //    var end = UniverseConstants.PlayerPositions[this.playerPosition];

        //    while (current > end)
        //    {
        //        this.Collider.MovementFactor = new Vector2(-1f, this.Collider.MovementFactor.Y);
        //        this.GetSprite().SpriteEffects = SpriteEffects.None;
        //        this.CurrentState = this.Collider.Flags[PhysicsFlag.Grounded] ? PlayerStates.Run : PlayerStates.Fall;

        //        current = this.Transform.Position.X;
        //        yield return null;
        //    }

        //    this.Collider.MovementFactor = new Vector2(0, this.Collider.MovementFactor.Y);
        //    this.isMoving = false;
        //}

        //// TODO: It might be nicer to ease this at some point.
        //private IEnumerable MovePlayerRight()
        //{
        //    this.isMoving = true;
        //    this.moveOrigin = this.Transform.Position.X;
        //    var current = this.moveOrigin;

        //    this.playerPosition++;
        //    var end = UniverseConstants.PlayerPositions[this.playerPosition];

        //    while (current < end)
        //    {
        //        this.Collider.MovementFactor = new Vector2(1f, this.Collider.MovementFactor.Y);
        //        this.GetSprite().SpriteEffects = SpriteEffects.FlipHorizontally;
        //        this.CurrentState = this.Collider.Flags[PhysicsFlag.Grounded] ? PlayerStates.Run : PlayerStates.Fall;

        //        current = this.Transform.Position.X;
        //        yield return null;
        //    }

        //    this.Collider.MovementFactor = new Vector2(0, this.Collider.MovementFactor.Y);
        //    this.isMoving = false;
        //}

        //private IEnumerable Bonk()
        //{
        //    this.isMoving = true;

        //    var current = this.Transform.Position.X;
        //    var bonkFrames = TimingHelper.GetFrameCount(0.5f);
        //    var frameCount = 0;
        //    if (this.Transform.Position.X > this.moveOrigin)
        //    {
        //        // We need to bonk to the left.
        //        while (frameCount < bonkFrames)
        //        {
        //            // TODO: Determine sprite effects.
        //            this.GetSprite().SpriteEffects = SpriteEffects.FlipHorizontally;

        //            // TODO: Add a "bonk" state.
        //            this.CurrentState = this.Collider.Flags[PhysicsFlag.Grounded] ? PlayerStates.Run : PlayerStates.Fall;

        //            current = this.Transform.Position.X;
        //            yield return null;
        //        }
        //    }
        //    else
        //    {

        //    }

        //    this.isMoving = false;
        //}

        private void OnLifeGained(int newLife)
        {
            var handler = this.LifeGained;
            if (handler != null)
            {
                handler(new StatChangedEventArgs(newLife));
            }
        }

        private void OnLifeLost(int newLife)
        {
            var handler = this.LifeLost;
            if (handler != null)
            {
                handler(new StatChangedEventArgs(newLife));
            }
        }

        private void OnRiftCollected(int newRift)
        {
            var handler = this.RiftCollected;
            if (handler != null)
            {
                handler(new StatChangedEventArgs(newRift));
            }
        }

        private void OnMonsterStomped()
        {
            var handler = this.MonsterStomped;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnPlatformStomped(PlatformStompedEventArgs e)
        {
            var handler = this.PlatformStomped;
            if (handler != null)
            {
                handler(e);
            }
        }

        private IEnumerable ImpactEffect(string audioClip)
        {
            var sprites = this.GetSprites();
            foreach (var s in sprites)
            {
                s.GoToAnimation(PlayerStates.Impact);
            }

            // Emit the impact effect.
            ((IParticleEffect)this.Components[Impact.ComponentName]).Emit();

            // Play a sound effect.
            AudioManager.Play(audioClip);

            // Stop all vertical velocity for a short period of time.
            for (var i = 0; i < 10; i++)
            {
                this.Collider.Velocity = new Vector2(this.Collider.Velocity.X, 0f);
                yield return null;
            }
        }

        private IEnumerable RunEffect()
        {
            var sprites = this.GetSprites();
            foreach (var s in sprites)
            {
                s.GoToAnimation(PlayerStates.Run);
            }

            while (true)
            {
                // Emit the impact effect.
                ((IParticleEffect)this.Components[DustPuff.ComponentName]).Emit();

                // Player a sound.
                AudioManager.Play("Step");
                yield return Coroutines.Pause(10);
            }
        }

        private IEnumerable Die()
        {
            // When the player dies, he or she respawns, destroying the hazard that ended up killing them but losing some time in the process.
            this.Flags[EngineFlag.Active] = false;
            this.Collider.Velocity = Vector2.Zero;

            // Make the player invulnerable for a short amount of time.
            this.GainInvulnerability(TimingHelper.GetFrameCount(3f));

            DrawingManager.Camera.Shake(10f, 1f, Ease.QuadIn);

            // Lose a life
            this.Lives--;

            var sprites = this.GetSprites();
            foreach (var s in sprites)
            {
                s.GoToAnimation(PlayerStates.Die);
            }

            // Explosion effect.
            ((IParticleEffect)this.Components[Explosion.ComponentName]).Emit();

            AudioManager.Play("hurt");

            // TODO: Move the player to a spot out of harm's reach
            var safePosition = this.Transform.Position;
            safePosition.Y -= 2 * DrawingManager.TileHeight;
            yield return this.Transform.MoveTo(safePosition, TimingHelper.GetFrameCount(DeathTime), EaseType.CubeOut);

            this.Flags[EngineFlag.Active] = true;

            // TODO: Pause updating everything in the game except for the player so things actually stop moving... Can probably do that through the GameManager.
        }

        private IEnumerable ShiftDimension()
        {
            // Determine whether alternate dimension textures should be drawn or not.
            var sprite = this.GetSprite();
            var colourLayer = sprite.Layers["colour"];
            var hairLayer = sprite.Layers["hair"];
            //hairLayer.Color = UniverseConstants.LimboDimensionColor;

            sprite.GoToAnimation(PlayerStates.ShiftDimension);

            colourLayer.IsVisible = false;
            hairLayer.IsVisible = false;

            // Emit the impact effect.
            var explosion = (Explosion)this.Components[Explosion.ComponentName];
            explosion.Color = UniverseConstants.NeutralColor;
            explosion.Emit();

            AudioManager.Play("dimensionshift");

            // TODO: Animate this dimension shift effect
            this.GainInvulnerability(TimingHelper.GetFrameCount(1.5f));

            sprite.GoToAnimation(PlayerStates.ShiftDimension);

            //CoroutineManager.Add(this.DimensionShiftCooldown(this.LimboTime));

            yield return Coroutines.Pause(20);
        }

        private IEnumerator DimensionShiftCooldown(int cooldownFrames)
        {
            yield return Coroutines.Pause(cooldownFrames);
            var sprite = this.GetSprite();
            sprite.Layers["colour"].IsVisible = true;
            sprite.Layers["hair"].IsVisible = true;

            GameManager.CurrentDimension = Dimension.Normal;
            this.PhaseGem.Generate();

            var implosion = (Implosion)this.Components[Implosion.ComponentName];
            implosion.Color = UniverseConstants.NormalDimensionColor;
            implosion.Emit();

            AudioManager.Play("dimensionshiftcooldown");
        }

        private IEnumerator ShieldLost()
        {
            this.Barrier.Break();

            // Make the player invulnerable for a short amount of time so they can get away from the danger.
            this.GainInvulnerability(TimingHelper.GetFrameCount(1.5f));
            yield return null;
        }

        private IEnumerator InvulnerabilityEffect(int totalFrames)
        {
            // TODO: Apply proper effects to the player sprite to show something is happening.
            this.Flags[PlayerFlags.Invulnerable] = true;
            var sprites = this.GetSprites();
            foreach (var s in sprites)
            {
                s.Blink(3, totalFrames);
            }

            yield return Coroutines.Pause(totalFrames);
            this.Flags[PlayerFlags.Invulnerable] = false;
        }

        private IEnumerator PhasedEffect(int totalFrames)
        {
            var sprite = this.GetSprite();

            this.Collider.Flags[PhysicsFlag.CollidesWithEnvironment] = false;
            yield return sprite.FadeTo(0.5f, 30, Ease.QuadIn);

            yield return Coroutines.Pause(totalFrames);

            this.Collider.Flags[PhysicsFlag.CollidesWithEnvironment] = true;
            yield return sprite.FadeTo(1f, 30, Ease.QuadIn);
        }
    }
}