using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Sprites;
using System.Collections.Generic;

namespace Abyss.World.Entities.Props.Concrete
{
    public class Shrine : ActiveProp
    {
        private static readonly Point spriteLocation = new Point(0, 1);
        private static readonly Rectangle boundingBox = new Rectangle(7, 0, 50, 64);

        /// <summary>
        /// Initializes a new instance of the <see cref="Shrine"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Shrine(Vector2 initialPosition)
            : base(
            "Shrine", 
            "Bestows a buff or a debuff on the player when activated. Usually costs rift, health or both to activate", 
            new List<ISpriteDetails>
                {
                    new SpriteDetails(
                        spriteLocation, 
                        DrawingConstants.TileDimensions, 
                        null,
                        DrawingConstants.EntitySpriteDepths["Props"], 
                        "Props")
                },
            new List<IAnimationState>
                {
                    new AnimationState("Idle", spriteLocation, 10, TimingHelper.GetFrameCount(0.1f), playInFull: false, isLooping: true) { LoopLagFrames = TimingHelper.GetFrameCount(1f) }
                },
            initialPosition,
            boundingBox,
            Vector2.Zero)
        {
        }

        /// <summary>
        /// Activates the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void Activate(IPlayer player)
        {
            this.isActivated = true;
        }

        protected override bool CheckCanActivate(IPlayer player)
        {
            return !this.isActivated;
        }
    }
}
