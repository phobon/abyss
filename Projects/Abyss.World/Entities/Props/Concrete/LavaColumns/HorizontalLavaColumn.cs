using System.Collections;
using Microsoft.Xna.Framework;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Maths;
using System;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Props.Concrete.LavaColumns
{
    public class HorizontalLavaColumn : LavaColumn
    {
        private static readonly Rectangle boundingBox = new Rectangle(0, 0, (int)DrawingManager.BaseResolutionWidth, DrawingManager.TileHeight * 2);

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalLavaColumn"/> class.
        /// </summary>
        /// <param name="eruptionDirection">The eruption direction.</param>
        /// <exception cref="System.ArgumentException">Eruption direction must be Direction.Left or Direction.Right;eruptionDirection</exception>
        public HorizontalLavaColumn(Direction eruptionDirection) 
            : base(Vector2.Zero, boundingBox, eruptionDirection)
        {
            if (eruptionDirection != Direction.Left && eruptionDirection != Direction.Right)
            {
                throw new ArgumentException("Eruption direction must be Direction.Left or Direction.Right", "eruptionDirection");
            }
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Determine a random vertical starting position for this lava column. 
            var verticalPosition = (GameManager.GameViewPort.Top * DrawingManager.TileHeight) + (MathsHelper.Random(5, 17) * DrawingManager.TileHeight);
            verticalPosition += MathsHelper.Random(DrawingManager.TileHeight);
            switch (this.EruptionDirection)
            {
                case Direction.Left:
                    this.initialPosition = new Vector2((int)DrawingManager.BaseResolutionWidth, verticalPosition);
                    this.startPosition = new Vector2((int)DrawingManager.BaseResolutionWidth - 10, verticalPosition);
                    break;
                case Direction.Right:
                    this.initialPosition = new Vector2(-(int)DrawingManager.BaseResolutionWidth, verticalPosition);
                    this.startPosition = new Vector2(-(int)DrawingManager.BaseResolutionWidth + 10, verticalPosition);
                    break;
            }

            this.Transform.Position = this.initialPosition;
            this.endPosition = new Vector2(0, verticalPosition);
        }

        protected override IEnumerator BeginEffect()
        {
            yield return this.Transform.MoveTo(this.startPosition, TimingHelper.GetFrameCount(0.2f));
            yield return Coroutines.Pause(TimingHelper.GetFrameCount(1f));
            yield return this.Transform.MoveTo(this.endPosition, TimingHelper.GetFrameCount(2f), Ease.ExpoIn);

            // Pause for a bit and then end this entity.
            yield return Coroutines.Pause(TimingHelper.GetFrameCount(2f));
            yield return this.Transform.MoveTo(this.initialPosition, TimingHelper.GetFrameCount(1f), Ease.ExpoIn);
            
            yield return Coroutines.Pause(60);
            this.End();
        }
    }
}
