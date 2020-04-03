using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Layers;

namespace Occasus.Core.Scenes
{
    public interface IScene : IEngineComponent
    {
        /// <summary>
        /// Gets the collection of sorted layers.
        /// </summary>
        IDictionary<string, ILayer> Layers { get; }

        /// <summary>
        /// Gets the tag cache collection. This allows the scene to easily query groups of entities based on a particular tag.
        /// </summary>
        IDictionary<string, IList<IEntity>> TagCache { get; }

        /// <summary>
        /// Handles the input for this scene.
        /// </summary>
        /// <param name="inputState">State of the input.</param>
        void HandleInput(IInputState inputState);

        /// <summary>
        /// Adds layers to this scene.
        /// </summary>
        void AddLayers();

        /// <summary>
        /// Adds an entity to the required layer and this scene's TagCache.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="layer">The layer.</param>
        void AddEntity(IEntity entity, string layer);

        /// <summary>
        /// Removes an entity from the designated layer as well as this scene's TagCache.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="layer">The layer.</param>
        void RemoveEntity(IEntity entity, string layer);

        /// <summary>
        /// Adds an entity to the designated layer and immediately updates the layer's entity cache.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="viewPort">The view port.</param>
        /// <param name="layer">The layer.</param>
        void AddEntityUpdateCache(IEntity entity, Rectangle viewPort, string layer = null);

        /// <summary>
        /// Forces updating the entity cache for the specified layer.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        /// <param name="layer">The layer.</param>
        void UpdateEntityCache(Rectangle viewPort, string layer = null);
    }
}
