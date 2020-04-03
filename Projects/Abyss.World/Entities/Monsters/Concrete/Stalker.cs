
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Components;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Physics;
using System.Collections;

using Occasus.Core.States;

namespace Abyss.World.Entities.Monsters.Concrete
{
    public class Stalker : Monster
    {
        private static readonly Rectangle boundingBox = new Rectangle(0, 0, 16, 16);

        /// <summary>
        /// Initializes a new instance of the <see cref="Stalker"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Stalker(Vector2 initialPosition)
            : base(
            "Stalker",
            "Stalkers track their prey by any means.",
            initialPosition,
            null,
            boundingBox,
            ZoneType.Normal)
        {
            this.Tags.Add(EntityTags.Stalker);
            this.Tags.Add(EntityTags.FlyingMonster);

            this.Collider.Flags[PhysicsFlag.CollidesWithEnvironment] = false;
        }

        protected override void SetupStates()
        {
            this.States.Add(MonsterStates.Stalk, new State(MonsterStates.Stalk, this.Stalk(), true));
            base.SetupStates();
        }

        private IEnumerable Stalk()
        {
            while (this.Flags[EngineFlag.Active])
            {
                // Calculate the direction between two positions to determine the vector direction.
                var direction = Vector2.Normalize(GameManager.Player.Transform.Position - this.Transform.Position) * 3;
                this.Transform.Position += direction;

                // Process sprite effects based on movement.
                this.ProcessMovement(this.Transform.Position.X < GameManager.Player.Transform.Position.X ? Direction.Right : Direction.Left);

                yield return null;
            }
        }

        private void ProcessMovement(Direction direction)
        {
            var sprite = (ISprite)this.Components[Sprite.Tag];
            switch (direction)
            {
                case Direction.Left:
                    sprite.GoToAnimation(MonsterStates.Stalk);
                    sprite.SpriteEffects = SpriteEffects.None;
                    break;
                case Direction.Right:
                    sprite.GoToAnimation(MonsterStates.Stalk);
                    sprite.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
            }
        }
    }
}
