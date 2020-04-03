using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Occasus.Core.Maps.Definitions
{
    public interface IActorSpawnDefinition : ISpawnDefinition
    {
        /// <summary>
        /// Gets the path.
        /// </summary>
        IList<Vector2> Path { get; }
    }
}
