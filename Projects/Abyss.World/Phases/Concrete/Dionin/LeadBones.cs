using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Layers;

namespace Abyss.World.Phases.Concrete.Dionin
{
    public class LeadBones : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeadBones"/> class.
        /// </summary>
        public LeadBones(int difficulty, int scoreRequirement) 
            : base(
            Phase.LeadBones,
            "A phase where all entities are too heavy to move properly.", 
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
            Monde.GameManager.Player.ShowOutline(Color.Yellow, pulse: true);
            Monde.ChangeFramerate(30f, 0);

            base.Apply(layer);
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            Monde.GameManager.Player.HideOutline();
            Monde.ChangeFramerate(60f, 0);

            base.Remove(layer);
        }
    }
}
