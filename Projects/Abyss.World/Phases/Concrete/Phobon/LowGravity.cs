using Abyss.World.Universe;
using Microsoft.Xna.Framework;

using Occasus.Core.Layers;

namespace Abyss.World.Phases.Concrete.Phobon
{
    public class LowGravity : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LowGravity"/> class.
        /// </summary>
        public LowGravity(int difficulty, int scoreRequirement)
            : base(
            Phase.LowGravity,
            "A phase where entities in the Abyss fall at a reduced speed.",
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
            var newSpeed = new Vector2(Monde.GameManager.Player.Collider.MovementSpeed.X, 3);
            Monde.GameManager.Player.Collider.MovementSpeed = newSpeed; 
            
            base.Apply(layer);
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            var newSpeed = new Vector2(Monde.GameManager.Player.Collider.MovementSpeed.X, Monde.GameManager.Player.FallSpeed);
            Monde.GameManager.Player.Collider.MovementSpeed = newSpeed;

            base.Remove(layer);
        }
    }
}
