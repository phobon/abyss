using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Occasus.Core.States;

namespace Abyss.World.Entities.Monsters.Concrete
{
    public class Faller : Monster
    {
        private static readonly Rectangle boundingBox = new Rectangle(0, 0, 16, 16);

        /// <summary>
        /// Initializes a new instance of the <see cref="Faller" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="path">The path.</param>
        public Faller(Vector2 initialPosition, IEnumerable<Vector2> path)
            : base(
            "Faller",
            "Fallers lay in wait to pounce on unsuspecting prey.",
            initialPosition,
            path,
            boundingBox,
            ZoneType.Normal)
        {
        }

        protected override void SetupStates()
        {
            this.States.Add(MonsterStates.Fall, new State(MonsterStates.Fall, this.Fall(), true));
            base.SetupStates();
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add(EntityTags.GroundedMonster);
            this.Tags.Add(EntityTags.Faller);
        }

        private IEnumerable Fall()
        {
            var start = this.Transform.Position;
            var end = this.Path.First();

            var distance = Vector2.Distance(start, end);
            var t = TimingHelper.GetFrameCount(distance / this.Collider.MovementSpeed.X);

            while (this.Flags[EngineFlag.Active])
            {
                // Move to the next position, accelerating as we go.
                yield return this.Transform.MoveTo(end, t, EaseType.QuadIn);
                yield return Coroutines.Pause(TimingHelper.GetFrameCount(1));

                // Move back to the start.
                yield return this.Transform.MoveTo(start, t, EaseType.Linear);
                yield return Coroutines.Pause(TimingHelper.GetFrameCount(1));
            }
        }
    }
}
