using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Components;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Occasus.Core.States;

namespace Abyss.World.Entities.Monsters.Concrete
{
    public class Crawler : Monster
    {
        private static readonly Rectangle boundingBox = new Rectangle(0, 0, 16, 16);

        /// <summary>
        /// Initializes a new instance of the <see cref="Crawler" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="path">The path.</param>
        public Crawler(Vector2 initialPosition, IEnumerable<Vector2> path)
            : base(
                "Crawler",
                "Crawlers are able to cling to walls.",
                initialPosition,
                path,
                boundingBox,
                ZoneType.Normal)
        {
        }

        protected override void SetupStates()
        {
            this.States.Add(MonsterStates.Crawl, new State(MonsterStates.Crawl, this.Crawl(), true));
            base.SetupStates();
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add(EntityTags.GroundedMonster);
            this.Tags.Add(EntityTags.Crawler);
        }

        private IEnumerable Crawl()
        {
            var pathIndex = 0;

            while (this.Flags[EngineFlag.Active])
            {
                var nextPathTarget = this.Path.ElementAt(pathIndex);

                // Calculate distance between the two points to determine speed.
                var distance = Vector2.Distance(this.Transform.Position, nextPathTarget);
                var t = distance / this.Collider.MovementSpeed.X;

                // Process sprite effects based on movement.
                if (!this.Transform.Position.X.Equals(nextPathTarget.X))
                {
                    this.ProcessMovement(this.Transform.Position.X < nextPathTarget.X ? Direction.Right : Direction.Left);
                }
                else
                {
                    this.ProcessMovement(this.Transform.Position.Y < nextPathTarget.Y ? Direction.Up : Direction.Down);
                }

                // Move to the next position.
                yield return this.Transform.MoveTo(nextPathTarget, TimingHelper.GetFrameCount(t), EaseType.Linear);
                
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
                    sprite.GoToAnimation(MonsterStates.Crawl);
                    sprite.SpriteEffects = SpriteEffects.None;
                    break;
                case Direction.Right:
                    sprite.GoToAnimation(MonsterStates.Crawl);
                    sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
        }
    }
}
