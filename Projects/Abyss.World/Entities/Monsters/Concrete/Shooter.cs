using Abyss.World.Universe;
using Microsoft.Xna.Framework;

using Occasus.Core.States;

namespace Abyss.World.Entities.Monsters.Concrete
{
    public class Shooter : Monster
    {
        private static readonly Rectangle boundingBox = new Rectangle(0, 0, 16, 16);

        /// <summary>
        /// Initializes a new instance of the <see cref="Shooter"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Shooter(Vector2 initialPosition)
            : base(
            "Shooter",
            "Shooters fire projectiles at their foes to defeat them from afar.",
            initialPosition,
            null,
            boundingBox,
            ZoneType.Normal)
        {
            this.Tags.Add(EntityTags.GroundedMonster);
            this.Tags.Add(EntityTags.Shooter);
        }

        protected override void SetupStates()
        {
            this.States.Add(MonsterStates.Shoot, new State(MonsterStates.Shoot, null, false));
            base.SetupStates();
        }
    }
}
