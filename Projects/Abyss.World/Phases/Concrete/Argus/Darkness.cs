using Abyss.World.Entities.Props.Concrete;
using Abyss.World.Universe;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Layers;

namespace Abyss.World.Phases.Concrete.Argus
{
    public class Darkness : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Darkness"/> class.
        /// </summary>
        public Darkness(int difficulty, int scoreRequirement)
            : base(
            Phase.Darkness,
            "A phase where the world is shrouded in darkness.",
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

            GameManager.LightsOut();

            if (layer.Parent.TagCache.ContainsKey(EntityTags.Light))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.Light])
                {
                    var light = (Light) e;
                    light.Extinguish();
                }
            }

            // Suspend all lightsources.
            // TODO: Can this be done in a better way?
            foreach (var entityList in layer.Parent.TagCache.Values)
            {
                foreach (var e in entityList)
                {
                    if (e.Components.ContainsKey(LightSource.Tag))
                    {
                        e.Components[LightSource.Tag].Suspend();
                    }
                }
            }

            // We really want to mess with the player, so remove all sources of light.
            GameManager.Player.Components[LightSource.Tag].Suspend();
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            base.Remove(layer);

            GameManager.LightsOn();

            if (layer.Parent.TagCache.ContainsKey(EntityTags.Light))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.Light])
                {
                    var light = (Light)e;
                    light.Ignite();
                }
            }

            // TODO: Can this be done in a better way?
            foreach (var entityList in layer.Parent.TagCache.Values)
            {
                foreach (var e in entityList)
                {
                    if (e.Components.ContainsKey(LightSource.Tag))
                    {
                        e.Components[LightSource.Tag].Resume();
                    }
                }
            }

            GameManager.Player.Components[LightSource.Tag].Resume();
        }
    }
}
