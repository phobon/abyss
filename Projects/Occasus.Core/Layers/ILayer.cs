using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Components;
using Occasus.Core.Entities;
using System.Collections.Generic;

using Occasus.Core.Scenes;

namespace Occasus.Core.Layers
{
    public interface ILayer : IEngineComponent
    {
        /// <summary>
        /// Gets the Scene this Layer belongs to.
        /// </summary>
        IScene Parent { get; }

        /// <summary>
        /// Gets the depth of this layer. Layers are depth sorted and drawn by a Scene.
        /// </summary>
        int Depth { get; }

        /// <summary>
        /// Gets the type of Layer this is.
        /// </summary>
        LayerType LayerType { get; }

        /// <summary>
        /// Gets the active, relevant entities.
        /// </summary>
        IEnumerable<IEntity> Entities { get; }

        /// <summary>
        /// Gets the lighting cache collection. Light sources are cached as part of regular game execution.
        /// </summary>
        IEnumerable<IEntityComponent> LightingCache { get; }

        /// <summary>
        /// Gets the spritebatch for this particular layer.
        /// </summary>
        SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Adds an entity to the layer. This also adds the entity to the parent Scene's TagCache.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <remarks>
        /// Note that this does not add an entity to the Entity cache; just into an internal collection that is then iterated over when updating the cache. This is purely for performance reasons.
        /// </remarks>
        void AddEntity(IEntity entity);

        /// <summary>
        /// Removes an entity from this layer.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void RemoveEntity(IEntity entity);

        /// <summary>
        /// Updates the entity cache.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        void UpdateEntityCache(Rectangle viewPort);
    }
}
