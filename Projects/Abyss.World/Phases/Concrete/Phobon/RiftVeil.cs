using Abyss.World.Entities.Props;
using Abyss.World.Universe;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Layers;
using System.Collections;

namespace Abyss.World.Phases.Concrete.Phobon
{
    public class RiftVeil : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RiftVeil"/> class.
        /// </summary>
        public RiftVeil(int difficulty, int scoreRequirement) 
            : base(
            Phase.RiftVeil,
            "A phase where curtains of pure rift energy shrouds parts of the world, stealing rift energy from them.", 
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
            CoroutineManager.Add(this.Name, this.CreateAndProcessRiftSheets(layer));
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            base.Remove(layer);
            CoroutineManager.Remove(this.Name);

            if (layer.Parent.TagCache.ContainsKey("RiftSheet"))
            {
                foreach (var e in layer.Parent.TagCache["RiftSheet"])
                {
                    e.End();
                }
            }
        }

        private IEnumerator CreateAndProcessRiftSheets(ILayer layer)
        {
            var leftSheet = PropFactory.GetRiftSheet(Direction.Left);
            var rightSheet = PropFactory.GetRiftSheet(Direction.Right);

            leftSheet.Begin();
            rightSheet.Begin();
            layer.AddEntity(leftSheet);
            layer.AddEntity(rightSheet);
            layer.UpdateEntityCache(Monde.GameManager.ViewPort);

            yield return null;

            // Do something to move the sheets around. Just comment this out for the time being.
            //while (true)
            //{
            //    yield return Coroutines.Pause(TimingHelper.GetFrameCount(3f));
            //}
        }
    }
}
