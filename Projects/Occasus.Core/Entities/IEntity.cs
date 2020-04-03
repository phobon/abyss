using Occasus.Core.Components;
using Occasus.Core.Physics;
using System.Collections.Generic;
using Occasus.Core.States;

namespace Occasus.Core.Entities
{
    public interface IEntity : IEngineComponent, IStateMachine
    {
        /// <summary>
        /// Gets the transform.
        /// </summary>
        ITransform Transform { get; }

        /// <summary>
        /// Gets a list of entity components associated with this entity.
        /// </summary>
        /// <remarks>
        /// Entity components are reusable objects that implement some sort of base-level functionality; such as drawing a sprite, etc.
        /// </remarks>
        IDictionary<string, IEntityComponent> Components { get; }

        /// <summary>
        /// Gets or sets an object that is used in physics collision methods.
        /// </summary>
        /// <remarks>
        /// If the Collider object is null, then this object should not be used in collision detection.
        /// </remarks>
        ICollider Collider { get; set; }

        /// <summary>
        /// Gets a collection of tags associated with this entity.
        /// </summary>
        /// <remarks>
        /// Used primarily to accelerate lookups in a parent Scene. The Scene caches these tags as entities are created.
        /// </remarks>
        IList<string> Tags { get; }
    }
}
