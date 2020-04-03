using Occasus.Core.Entities;

namespace Occasus.Core.Components
{
    public interface IEntityComponent : IEngineComponent
    {
        /// <summary>
        /// Gets the parent entity of this component.
        /// </summary>
        IEntity Parent { get; }
    }
}
