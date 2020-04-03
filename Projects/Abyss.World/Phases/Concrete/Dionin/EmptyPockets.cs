using Abyss.World.Entities.Items;
using Abyss.World.Universe;
using Occasus.Core;
using Occasus.Core.Layers;

namespace Abyss.World.Phases.Concrete.Dionin
{
    public class EmptyPockets : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyPockets"/> class.
        /// </summary>
        public EmptyPockets(int difficulty, int scoreRequirement) 
            : base(
            Phase.EmptyPockets,
            "A phase where inter-dimensional items cannot be interacted with or collected.", 
            difficulty,
            UniverseConstants.DioninColor,
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

            if (layer.Parent.TagCache.ContainsKey(EntityTags.Item))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.Item])
                {
                    var i = (IItem) e;
                    i.FadeOut();
                    i.Flags[EngineFlag.Collidable] = false;
                }
            }
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            base.Remove(layer);

            if (layer.Parent.TagCache.ContainsKey(EntityTags.Item))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.Item])
                {
                    var i = (IItem)e;
                    i.FadeIn();
                    i.Flags[EngineFlag.Collidable] = true;
                }
            }
        }
    }
}
