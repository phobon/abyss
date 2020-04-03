using Abyss.World.Entities.Props;
using Abyss.World.Universe;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Layers;
using System.Collections;

namespace Abyss.World.Phases.Concrete.Phobon
{
    public class VoidZone : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoidZone"/> class.
        /// </summary>
        public VoidZone(int difficulty, int scoreRequirement) 
            : base(
            Phase.VoidZone,
            "A phase where enormous zones of pure void appear, drawing everything inside them.", 
            difficulty,
            UniverseConstants.PhobonColor,
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
            CoroutineManager.Add(this.Name, this.CycleVoidPatches(layer));
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            base.Remove(layer);
            CoroutineManager.Remove(this.Name);

            if (layer.Parent.TagCache.ContainsKey("VoidPatch"))
            {
                foreach (var e in layer.Parent.TagCache["VoidPatch"])
                {
                    e.End();
                }
            }
        }

        private IEnumerator CycleVoidPatches(ILayer layer)
        {
            while (true)
            {
                var patch = PropFactory.GetVoidPatch();
                patch.Begin();
                layer.AddEntity(patch);
                layer.UpdateEntityCache(GameManager.GameViewPort);
                yield return Coroutines.Pause(TimingHelper.GetFrameCount(3f));
            }
        }
    }
}
