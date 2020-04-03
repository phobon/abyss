using Occasus.Core.Generation;

namespace Abyss.World.Phases
{
    public interface IPhaseFactory : IFactory<IPhase>
    {
        IPhase CyclePhase(string currentPhase);

        IPhase GenerateRandomByDifficulty(int difficulty);
    }
}
