using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Drawing;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Physics;
using Occasus.Core.States;
using System;

namespace Abyss.World.Entities.Props
{
    public abstract class ActiveProp : Prop, IActiveProp
    {
        private bool isActivated;
        private bool canActivate;
        private IExclamationBubble exclamationBubble;
        private bool playerColliding;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveProp"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="origin">The origin.</param>
        protected ActiveProp(
            string name, 
            string description, 
            Vector2 initialPosition, 
            Rectangle boundingBox,
            Vector2 origin)
            : base(name, description, initialPosition, boundingBox, origin)
        {
        }

        /// <summary>
        /// Occurs when prop is activated.
        /// </summary>
        public event EventHandler Activated;

        /// <summary>
        /// Gets a value indicating whether this prop can be activated.
        /// </summary>
        /// <value>
        ///   <c>True</c> if this prop can be activated; otherwise, <c>false</c>.
        /// </value>
        public bool CanActivate
        {
            get
            {
                return this.canActivate;
            }

            private set
            {
                if (this.canActivate == value)
                {
                    return;
                }

                this.canActivate = value;

                // Show or hide the bubble.
                if (this.canActivate)
                {
                    this.exclamationBubble.Appear();
                }
                else
                {
                    this.exclamationBubble.Disappear();
                }
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
            if (this.playerColliding)
            {
                var collision = GameManager.Player.Collider.QualifiedBoundingBox.GetIntersectionDepth(this.Collider.QualifiedBoundingBox);
                if (collision == Vector2.Zero)
                {
                    this.CanActivate = false;
                    this.playerColliding = false;
                    GameManager.Player.CurrentProp = null;
                }
            }
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (this.exclamationBubble != null)
            {
                if (this.exclamationBubble.Flags[EngineFlag.Visible])
                {
                    this.exclamationBubble.Draw(gameTime, spriteBatch);
                }
            }
        }

        /// <summary>
        /// Activates the prop.
        /// </summary>
        /// <param name="player">The player.</param>
        public virtual void Activate(IPlayer player)
        {
            this.isActivated = true;
            this.OnActivated();
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.exclamationBubble = new ExclamationBubble(this);
            this.exclamationBubble.Initialize();
        }

        protected override void SetupStates()
        {
            base.SetupStates();

            var sprite = this.GetSprite();
            this.States.Add(PropStates.Activated, State.GenericState(PropStates.Activated, sprite));
            this.States.Add(PropStates.Activating, State.GenericState(PropStates.Activating, sprite));
        }

        protected virtual bool CheckCanActivate(IPlayer player)
        {
            return !this.isActivated;
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            base.ColliderOnCollision(args);

            var player = args.CollisionEntity as IPlayer;
            if (player != null)
            {
                player.CurrentProp = this;
                this.playerColliding = true;
                this.CanActivate = this.CheckCanActivate(player);
            }
        }

        private void OnActivated()
        {
            var handler = this.Activated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
