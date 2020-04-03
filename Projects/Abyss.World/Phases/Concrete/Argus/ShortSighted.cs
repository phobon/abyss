using Abyss.World.Entities.Props.Concrete;
using Abyss.World.Universe;
using Occasus.Core.Layers;

namespace Abyss.World.Phases.Concrete.Argus
{
    public class ShortSighted : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShortSighted"/> class.
        /// </summary>
        public ShortSighted(int difficulty, int scoreRequirement) 
            : base(
            Phase.ShortSighted,
            "A phase where the player is only able to see things very close to them.", 
            difficulty,
            UniverseConstants.ArgusColor,
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

            Monde.GameManager.LightsOut();

            if (layer.Parent.TagCache.ContainsKey(EntityTags.Light))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.Light])
                {
                    var light = (Light)e;
                    light.Extinguish();
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

            Monde.GameManager.LightsOn();

            if (layer.Parent.TagCache.ContainsKey(EntityTags.Light))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.Light])
                {
                    var light = (Light)e;
                    light.Ignite();
                }
            }
        }
    }
}
