using Abyss.World.Entities.Items;
using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Components;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Sprites;
using System.Globalization;

namespace Abyss.World.Entities.Props.Concrete
{
    public class PurchasableItem : ActiveProp
    {
        private Vector2 costPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchasableItem" /> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="sprite">The sprite.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="cost">The cost.</param>
        public PurchasableItem(
            IItem item,
            Vector2 initialPosition, 
            ISprite sprite,
            Rectangle boundingBox, 
            int cost)
            : base(
                "PurchasableItem", 
                "The player is able to purchase this item for a rift cost.", 
                initialPosition,
                boundingBox,
                Vector2.Zero)
        {
            this.Cost = cost;
            this.Item = item;

            this.Components.Remove(Sprite.Tag);
            this.Components.Add(Sprite.Tag, sprite);
        }

        /// <summary>
        /// Gets the item to be purchased.
        /// </summary>
        public IItem Item
        {
            get; private set;
        }

        /// <summary>
        /// Gets the cost of this item.
        /// </summary>
        public int Cost
        {
            get; private set;
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (this.costPosition == Vector2.Zero)
            {
                var costWidth = DrawingManager.Font.MeasureString(this.Cost.ToString());
                this.costPosition = new Vector2(this.Collider.QualifiedBoundingBox.Center.X - (costWidth.X / 2), this.Collider.QualifiedBoundingBox.Center.Y - 32);
            }

            var color = GameManager.Player.Rift >= this.Cost ? Color.White : Color.Red;
            spriteBatch.DrawString(DrawingManager.Font, this.Cost.ToString(CultureInfo.InvariantCulture), this.costPosition, color);
        }

        /// <summary>
        /// Activates the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void Activate(IPlayer player)
        {
            base.Activate(player);

            this.Item.Collect(player);
            player.Rift -= this.Cost;

            this.Suspend();
            this.Flags[EngineFlag.Relevant] = false;
        }

        protected override bool CheckCanActivate(IPlayer player)
        {
            if (GameManager.Player.Rift < this.Cost)
            {
                return false;
            }

            return base.CheckCanActivate(player);
        }

        protected override void SetupStates()
        {
        }
    }
}
