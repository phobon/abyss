using System.Collections;

using Abyss.World.Universe;
using Microsoft.Xna.Framework;

using Occasus.Core;
using Occasus.Core.Components;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Sprites;

using Occasus.Core.Maths;
using Occasus.Core.Physics;
using Occasus.Core.States;

namespace Abyss.World.Entities.Monsters.Concrete
{
    public class Jumper : Monster
    {
        private float JumpDistance = -3 * DrawingManager.TileWidth;

        private static readonly Rectangle boundingBox = new Rectangle(4, 9, 7, 7);

        /// <summary>
        /// Initializes a new instance of the <see cref="Jumper"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Jumper(Vector2 initialPosition)
            : base(
            "Jumper",
            "Jumpers leap into the air to try and catch their prey.",
            initialPosition,
            null,
            boundingBox,
            ZoneType.Normal)
        {
            this.Tags.Add(EntityTags.GroundedMonster);
            this.Tags.Add(EntityTags.Jumper);
        }

        protected override void SetupStates()
        {
            this.States.Add(MonsterStates.Jump, new State(MonsterStates.Jump, this.Jump(), true));
            base.SetupStates();
        }

        private IEnumerable Jump()
        {
            var sprite = (ISprite)this.Components[Sprite.Tag];
            var startPosition = this.Transform.Position;
            var endPosition = startPosition + new Vector2(0f, JumpDistance);

            while (this.Flags[EngineFlag.Active])
            {
                sprite.GoToAnimation(MonsterStates.Jump);
                yield return Coroutines.Pause(8);
                yield return this.Transform.MoveTo(endPosition, 45, EaseType.QuadInOut);

                sprite.GoToAnimation(MonsterStates.Idle);
                yield return this.Transform.MoveTo(startPosition, 60, EaseType.QuadIn);

                sprite.GoToAnimation(MonsterStates.Impact);
                sprite.GoToAnimation(MonsterStates.Idle);
                yield return Coroutines.Pause(TimingHelper.GetFrameCount(2));
            }
        }
    }
}
