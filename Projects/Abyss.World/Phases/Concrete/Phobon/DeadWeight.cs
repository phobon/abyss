using Abyss.World.Universe;
using Occasus.Core.Layers;
using Occasus.Core.Physics;

namespace Abyss.World.Phases.Concrete.Phobon
{
    public class DeadWeight : PhaseBase
    {
        private const int TerminalVelocity = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeadWeight"/> class.
        /// </summary>
        public DeadWeight(int difficulty, int scoreRequirement) 
            : base(
            Phase.DeadWeight,
            "A phase where living entities can reach a terminal velocity in the Abyss.", 
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
            Monde.GameManager.Player.Collider.Collision += ColliderOnCollision;
            base.Apply(layer);
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            Monde.GameManager.Player.Collider.Collision -= ColliderOnCollision;
            base.Remove(layer);
        }

        private void ColliderOnCollision(CollisionEventArgs collisionEventArgs)
        {
            if (Monde.GameManager.Player.Collider.UngroundedFrames > TerminalVelocity)
            {
                // The only way a player can survive a fall at terminal velocity is to bop an enemy.
                if (collisionEventArgs.CollisionType == CollisionTypes.Environment)
                {
                    Monde.GameManager.Player.TakeDamage("DeadWeight");
                }
            }
        }
    }
}
