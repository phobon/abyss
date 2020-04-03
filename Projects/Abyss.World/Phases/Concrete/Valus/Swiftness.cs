using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core.Layers;
using Occasus.Core.Physics;

namespace Abyss.World.Phases.Concrete.Valus
{
    public class Swiftness : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Swiftness"/> class.
        /// </summary>
        public Swiftness(int difficulty, int scoreRequirement) 
            : base(
            Phase.Swiftness,
            "A phase where grounded entities move with great pace.", 
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
            if (layer.Parent.TagCache.ContainsKey(EntityTags.Monster))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.Monster])
                {
                    e.Collider.MovementSpeed = new Vector2(PhysicsManager.ActorMovementSpeeds[e.Name][ActorSpeed.Fast], e.Collider.MovementSpeed.Y);
                }
            }

            base.Apply(layer);
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            if (layer.Parent.TagCache.ContainsKey(EntityTags.Monster))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.Monster])
                {
                    e.Collider.MovementSpeed = new Vector2(PhysicsManager.ActorMovementSpeeds[e.Name][ActorSpeed.Normal], e.Collider.MovementSpeed.Y);
                }
            }

            base.Remove(layer);
        }
    }
}
