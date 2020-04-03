using Occasus.Core.Entities;

namespace Abyss.World.Entities.Platforms
{
    /// <summary>
    /// Interface for a Platform entity.
    /// </summary>
    public interface IPlatform : IEntity
    {
        /// <summary>
        /// Gets the type of the platform.
        /// </summary>
        PlatformType PlatformType { get; }
    }
}
