using System.Collections.Generic;

namespace Occasus.Core.Components
{
    public interface ICompositeEntityComponent
    {
        /// <summary>
        /// Gets the parent.
        /// </summary>
        IEntityComponent ParentComponent { get; }

        /// <summary>
        /// Gets the components of this component.
        /// </summary>
        IList<IEntityComponent> Components { get; }
    }
}
