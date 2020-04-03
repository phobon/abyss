using System;

namespace Abyss.World.Entities.Player.Components
{
    public interface IPhaseGem
    {
        event EventHandler ChargeGenerated;

        event EventHandler ChargeUsed;

        int MaximumCharges { get; }

        int Charges
        {
            get;
        }

        void Generate();

        bool Use();

        void Reset();
    }
}
