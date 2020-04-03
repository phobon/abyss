using Microsoft.Xna.Framework;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Input;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;

namespace Abyss.World.Entities.Props.Concrete.LavaColumns
{
    public class VerticalLavaColumn : LavaColumn
    {
        private static readonly Rectangle boundingBox = new Rectangle(0, 0, DrawingManager.TileWidth * 2, (int)DrawingManager.BaseResolutionHeight);

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalLavaColumn"/> class.
        /// </summary>
        public VerticalLavaColumn()
            : base(Vector2.Zero, boundingBox, Direction.Up)
        {
            this.maintainStartPosition = true;
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Determine a random vertical starting position for this lava column. 
            var horizontalPosition = MathsHelper.Random(10) * DrawingManager.TileWidth;
            horizontalPosition += MathsHelper.Random(DrawingManager.TileWidth);
            this.Transform.Position = new Vector2(horizontalPosition, Monde.GameManager.Player.Transform.Position.Y + 500);
            this.endPosition = this.Transform.Position - new Vector2(0, boundingBox.Height * 2);
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            base.Update(gameTime, inputState);

            if (this.maintainStartPosition)
            {
                this.Transform.Position = new Vector2(this.Transform.Position.X, Monde.GameManager.Player.Transform.Position.Y + 500);
            }
        }

        private bool maintainStartPosition;

        protected override IEnumerator BeginEffect()
        {
            yield return Coroutines.Pause(TimingHelper.GetFrameCount(1f));
            this.maintainStartPosition = false;
            yield return this.Transform.MoveTo(this.endPosition, TimingHelper.GetFrameCount(2f), Ease.ExpoIn);
            this.End();
        }
    }
}
