using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Components;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Occasus.Core.States;

namespace Abyss.World.Entities.Monsters.Concrete
{
    public class Flyer : Monster
    {
        private static readonly Rectangle boundingBox = new Rectangle(2, 4, 12, 7);

        /// <summary>
        /// Initializes a new instance of the <see cref="Flyer" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="path">The path.</param>
        public Flyer(Vector2 initialPosition, IEnumerable<Vector2> path)
            : base(
            "Flyer",
            "Flyers fly in a straight line.",
            initialPosition,
            path,
            boundingBox,
            ZoneType.Normal)
        {
            this.Tags.Add(EntityTags.Flyer);
            this.Tags.Add(EntityTags.FlyingMonster);
        }

        protected override void SetupStates()
        {
            this.States.Add(MonsterStates.Fly, new State(MonsterStates.Fly, this.Fly(), false));
            base.SetupStates();
        }

        private IEnumerable Fly()
        {
            this.GetSprite().GoToAnimation(MonsterStates.Fly);

            var pathIndex = 0;

            while (this.Flags[EngineFlag.Active])
            {
                var nextPathTarget = this.Path.ElementAt(pathIndex);

                // Calculate distance between the two points to determine speed.
                var distance = Vector2.Distance(this.Transform.Position, nextPathTarget);
                var t = distance / this.Collider.MovementSpeed.X;

                // Process sprite effects based on movement.
                this.ProcessMovement(this.Transform.Position.X < nextPathTarget.X ? Direction.Right : Direction.Left);

                // Move to the next position.
                yield return this.Transform.MoveTo(nextPathTarget, TimingHelper.GetFrameCount(t), EaseType.Linear);

                // Allow a short time until the next move is made.
                yield return Coroutines.Pause(TimingHelper.GetFrameCount(1));

                // Increment pathIndex, clamping if necessary.
                if (pathIndex + 1 >= this.Path.Count())
                {
                    pathIndex = 0;
                }
                else
                {
                    pathIndex++;
                }
            }
        }

        private void ProcessMovement(Direction direction)
        {
            var sprite = (ISprite)this.Components[Sprite.Tag];
            switch (direction)
            {
                case Direction.Left:
                    sprite.GoToAnimation(MonsterStates.Fly);
                    sprite.SpriteEffects = SpriteEffects.None;
                    break;
                case Direction.Right:
                    sprite.GoToAnimation(MonsterStates.Fly);
                    sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
        }
    }
}
