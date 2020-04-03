using System.Linq;
using Abyss.World.Entities.Items;
using Abyss.World.Entities.Player;
using Abyss.World.Entities.Player.Components;
using Abyss.World.Maps;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Layers;
using Occasus.Core.Scenes;

namespace Abyss.World.Scenes.Zone.Layers
{
    /// <summary>
    /// Gameplay layer for the Zone scene. Draws the map, enemies, items, props, hazards and the player.
    /// </summary>
    public class ZoneGameplayLayer : Layer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneGameplayLayer"/> class.
        /// </summary>
        /// <param name="parentScene">The parent scene.</param>
        public ZoneGameplayLayer(IScene parentScene)
            : base(
            parentScene, 
            "Zone Gameplay Layer", 
            "Gameplay layer for a zone", 
            LayerType.TransformedToCamera,
            1)
        {
        }


        /// <summary>
        /// Updates the entity cache.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        public override void UpdateEntityCache(Rectangle viewPort)
        {
            // Determine any items that are still relevant.
            var cachedEntities = this.entities.ToList();
            foreach (var e in cachedEntities)
            {
                if (e is IMap || e is IFullScreenParticleEffect || e is IPlayer || e is IBarrier)
                {
                    continue;
                }

                // If the entity is no longer relevant; suspend it and remove it.
                if (e.Transform.GridPosition.Y < GameManager.GameViewPort.Top && e.Transform.GridPosition.Y > GameManager.GameViewPort.Bottom)
                {
                    e.Suspend();
                    this.entities.Remove(e);
                    e.Flags[EngineFlag.Relevant] = false;
                }
            }

            foreach (var e in this.allEntities)
            {
                // Check if this item is irrelevant or not. If it is, just ignore it.
                if (!e.Flags[EngineFlag.Relevant])
                {
                    continue;
                }

                // Check if this item is cached or not. If it is, just ignore it.
                var alreadyCached = false;
                foreach (var cachedEntity in cachedEntities)
                {
                    if (e == cachedEntity)
                    {
                        alreadyCached = true;
                        break;
                    }
                }

                if (alreadyCached)
                {
                    continue;
                }

                // This item is new and hasn't been cached yet, so determine if it's relevant.
                if (e.Flags[EngineFlag.ForceIncludeInCache] || (e.Transform.GridPosition.Y >= GameManager.GameViewPort.Top && e.Transform.GridPosition.Y <= GameManager.GameViewPort.Bottom))
                {
                    this.entities.Add(e);

                    // If an entity hasn't been initialized; initialize it here.
                    if (e.Flags[EngineFlag.Relevant] && !e.Flags[EngineFlag.Initialized])
                    {
                        e.Initialize();
                    }
                }
            }

            // Process cached entities that are new.
            foreach (var e in this.entities)
            {
                if (e.Flags[EngineFlag.New])
                {
                    // Make sure we only process these things once.
                    e.Flags[EngineFlag.New] = false;

                    // If the entity is deferred begin and hasn't begun, skip it; else resume it.
                    if (e.Flags[EngineFlag.DeferredBegin] && !e.Flags[EngineFlag.HasBegun])
                    {
                        continue;
                    }

                    if (!e.Flags[EngineFlag.HasBegun])
                    {
                        e.Begin();
                    }
                    else
                    {
                        e.Resume();
                    }

                    // Determine whether this entity is an item or not. If it is, it's safe to make it hover.
                    var item = e as IItem;
                    if (item != null)
                    {
                        item.Hover();
                    }
                }
            }

            // Update lighting cache as normal.
            this.UpdateLightingCache();
        }

        protected override void InitializeEntityCache()
        {
            // There are always certain entities that should be relevant at all times.
            foreach (var e in this.allEntities)
            {
                // Map should always be relevant.
                if (e is IMap)
                {
                    this.entities.Add(e);
                    continue;
                }

                // Any fullscreen particle effects should always be relevant.
                if (e is IFullScreenParticleEffect)
                {
                    this.entities.Add(e);
                    continue;
                }

                // The player and the player shield should always be relevant.
                if (e is IPlayer || e is IBarrier)
                {
                    this.entities.Add(e);

                    // If an entity hasn't been initialized; initialize it here.
                    if (e.Flags[EngineFlag.Relevant] && !e.Flags[EngineFlag.Initialized])
                    {
                        e.Initialize();
                    }
                }
            }
            

            this.UpdateEntityCache(GameManager.GameViewPort);
        }
    }
}
