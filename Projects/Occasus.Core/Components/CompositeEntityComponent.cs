using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Occasus.Core.Components
{
    public abstract class CompositeEntityComponent : EntityComponent, ICompositeEntityComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeEntityComponent"/> class.
        /// </summary>
        /// <param name="parentComponent">The parent component.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        protected CompositeEntityComponent(
            IEntityComponent parentComponent, 
            string name, 
            string description)
            : base(parentComponent.Parent, name, description)
        {
            this.ParentComponent = parentComponent;
            this.Components = new List<IEntityComponent>();
        }

        /// <summary>
        /// Gets the parent component.
        /// </summary>
        /// <value>
        /// The parent component.
        /// </value>
        public IEntityComponent ParentComponent
        {
            get; private set;
        }

        /// <summary>
        /// Gets the components of this component.
        /// </summary>
        public IList<IEntityComponent> Components
        {
            get; private set;
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, Input.IInputState inputState)
        {
            foreach (var c in this.Components)
            {
                if (c.Flags[EngineFlag.Active])
                {
                    c.Update(gameTime, inputState);
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
            foreach (var c in this.Components)
            {
                if (c.Flags[EngineFlag.Visible])
                {
                    c.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}
