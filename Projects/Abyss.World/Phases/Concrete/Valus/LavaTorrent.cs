using Abyss.World.Entities.Props;
using Abyss.World.Universe;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Layers;
using Occasus.Core.Maths;
using System.Collections;

namespace Abyss.World.Phases.Concrete.Valus
{
    public class LavaTorrent : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LavaTorrent"/> class.
        /// </summary>
        public LavaTorrent(int difficulty, int scoreRequirement) 
            : base(
            Phase.LavaTorrent,
            "A phase where grand columns of lava spurt across the world from out of the void.", 
            difficulty,
            UniverseConstants.ValusColor,
            scoreRequirement)
        {
        }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Apply(ILayer layer)
        {
            base.Apply(layer);
            CoroutineManager.Add(this.Name, this.CycleLavaColumns(layer));
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            base.Remove(layer);
            CoroutineManager.Remove(this.Name);
        }

        private IEnumerator CycleLavaColumns(ILayer layer)
        {
            while (true)
            {
                var eruptionDirection = MathsHelper.Random() > 50 ? Direction.Left : Direction.Right;
                var column = PropFactory.GetHorizontalLavaColumn(eruptionDirection);
                column.Begin();
                layer.AddEntity(column);
                layer.UpdateEntityCache(GameManager.GameViewPort);
                yield return Coroutines.Pause(TimingHelper.GetFrameCount(5f));
            }
        }
    }
}
