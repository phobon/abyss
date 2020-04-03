using System;
using System.Collections;
using System.Linq;
using Abyss.World.Entities.Relics;
using Abyss.World.Phases;
using Abyss.World.Universe;

namespace Abyss.World.Entities.Player.Components
{
    public class PhaseGem : IPhaseGem
    {
        private readonly IPlayer player;

        public event EventHandler ChargeGenerated;

        public event EventHandler ChargeUsed;

        public PhaseGem(IPlayer player) 
        {
            this.player = player;
            this.MaximumCharges = 1;
            this.Charges = 1;
        }

        public int MaximumCharges
        {
            get; private set;
        }

        public int Charges
        {
            get; private set;
        }

        public void Generate()
        {
            if (this.Charges < this.MaximumCharges)
            {
                this.Charges++;
                this.OnChargeGenerated();
            }
        }

        public bool Use()
        {
            if (this.Charges == 0)
            {
                return false;
            }

            if (Monde.GameManager.CurrentDimension == Dimension.Limbo)
            {
                return false;
            }

            // Check for different phase effects.
            if (PhaseManager.CurrentPhases.Any())
            {
                if (PhaseManager.IsPhaseActive(Phase.Marooned))
                {
                    return false;
                }
            }

            Monde.GameManager.CurrentDimension = Dimension.Limbo;

            // Remove a phase gem charge.
            this.Charges--;

            this.player.CurrentState = PlayerStates.ShiftDimension;

            // Activate dimension shift relics if applicable.
            //GameManager.RelicCollection.ActivateRelics(RelicActivationType.DimensionShift);

            // Update statistics.
            Monde.GameManager.StatisticManager.DimensionsShifted++;
            
            // Fire an event so that everything knows what's going on.
            this.OnChargeUsed();

            return true;
        }

        public void Reset()
        {
            this.MaximumCharges = 1;
            this.Generate();
        }

        private IEnumerable GenerateCharge()
        {
            this.Generate();
            yield return null;
        }

        private void OnChargeGenerated()
        {
            var handler = this.ChargeGenerated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnChargeUsed()
        {
            var handler = this.ChargeUsed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
