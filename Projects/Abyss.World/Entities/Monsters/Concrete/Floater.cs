using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Physics;
using Occasus.Core.States;
using System.Collections;
using System.Collections.Generic;

namespace Abyss.World.Entities.Monsters.Concrete
{
    public class Floater : Monster
    {
        private static readonly Rectangle boundingBox = new Rectangle(4, 4, 7, 8);

        /// <summary>
        /// Initializes a new instance of the <see cref="Floater" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="path">The path.</param>
        public Floater(Vector2 initialPosition)
            : base(
            "Floater",
            "Floaters hover in place, awaiting witless prey.",
            initialPosition,
            null,
            boundingBox,
            ZoneType.Normal)
        {
        }

        protected override void SetupStates()
        {
            this.States.Add(MonsterStates.Idle, new State(MonsterStates.Idle, this.Float(), true));
            base.SetupStates();
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add(EntityTags.FlyingMonster);
            this.Tags.Add(EntityTags.Floater);
        }

        private IEnumerable Float()
        {
            var top = this.Transform.Position;
            top.Y -= 5;

            var bottom = this.Transform.Position;

            while (true)
            {
                yield return this.Transform.MoveTo(top, TimingHelper.GetFrameCount(1));
                yield return this.Transform.MoveTo(bottom, TimingHelper.GetFrameCount(1));
            }
        }
    }
}
