using Microsoft.Xna.Framework;

namespace Occasus.Core.Maps.Definitions
{
    public interface IAreaSpawnDefinition : ISpawnDefinition
    {
        Rectangle Area { get; }
    }
}
