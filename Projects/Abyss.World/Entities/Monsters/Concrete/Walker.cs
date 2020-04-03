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
    public class Walker : Monster
    {
        private static readonly Rectangle boundingBox = new Rectangle(4, 10, 8, 6);

        /// <summary>
        /// Initializes a new instance of the <see cref="Walker" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="path">The path.</param>
        public Walker(Vector2 initialPosition, IEnumerable<Vector2> path)
            : base(
            "Walker",
            "Walkers roam around platforms looking for their prey.",
            initialPosition,
            path,
            boundingBox,
            ZoneType.Normal)
        {
        }

        protected override void SetupStates()
        {
            this.States.Add(MonsterStates.Walk, new State(MonsterStates.Walk, this.Walk(), true));
            base.SetupStates();
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add(EntityTags.GroundedMonster);
            this.Tags.Add(EntityTags.Walker);
        }

        protected override void InitializeCollider()
        {
            base.InitializeCollider();
            this.Collider.Flags[PhysicsFlag.CollidesWithEnvironment] = false;
        }

        private IEnumerable Walk()
        {
            var pathIndex = 0;

            var sprite = this.GetSprite();
            while (this.Flags[EngineFlag.Active])
            {
                var nextPathTarget = this.Path.ElementAt(pathIndex);

                // Calculate distance between the two points to determine speed.
                var distance = Vector2.Distance(this.Transform.Position, nextPathTarget);
                var t = distance / this.Collider.MovementSpeed.X;

                // Process sprite effects based on movement.
                this.ProcessMovement(this.Transform.Position.X < nextPathTarget.X ? Direction.Right : Direction.Left);

                // Move to the next position.
                yield return this.Transform.MoveTo(nextPathTarget, TimingHelper.GetFrameCount(t), Ease.Linear);

                // Allow a short time until the next move is made.
                sprite.GoToAnimation(MonsterStates.Idle);
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
            var sprite = this.GetSprite();
            switch (direction)
            {
                case Direction.Left:
                    sprite.GoToAnimation(MonsterStates.Walk);
                    sprite.SpriteEffects = SpriteEffects.None;
                    break;
                case Direction.Right:
                    sprite.GoToAnimation(MonsterStates.Walk);
                    sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
        }
    }
}
