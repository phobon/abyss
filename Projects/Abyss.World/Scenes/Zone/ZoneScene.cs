using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Entities.Player;
using Abyss.World.Entities.Relics;
using Abyss.World.Maps;
using Abyss.World.Phases;
using Abyss.World.Scenes.Zone.Layers;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Occasus.Core;
using Occasus.Core.Camera;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Drawing.Shaders;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Layers;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using Occasus.Core.Scenes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Abyss.World.Entities.Items;
using Abyss.World.Entities.Items.Concrete.RiftShards;
using Abyss.World.Entities.Monsters;

namespace Abyss.World.Scenes.Zone
{
    /// <summary>
    /// Scene that handles all of the logic, input and drawing functions in a gameplay context.
    /// </summary>
    public class ZoneScene : Scene
    {
        private const string BackgroundLayerKey = "Background";
        private const string GameplayLayerKey = "Gameplay";
        private const string InterfaceLayerKey = "Interface";
        
#if DEBUG
        private const string PhysicsDebugLayerKey = "PhysicsDebug";
        private const string DebugLayerKey = "Debug";
        private const string PlatformCollidersDebugKey = "Platform Colliders";
#endif
        
#if DEBUG
        private readonly List<Color> ambientLightColors = new List<Color>
            {
                Color.White,
                Color.Blue,
                Color.Red,
                Color.Green,
                Color.Aqua,
                Color.Gold
            };

        private int currentShaderIndex;
        private int currentLightIndex;
#endif
        // Caching and performance.
        private bool refreshCache;
        private readonly IList<Rectangle> levelGeometryColliders = new List<Rectangle>();
        private readonly IList<Rectangle> platformColliders = new List<Rectangle>();
        private readonly IList<ICollider> platforms = new List<ICollider>();

        private readonly IList<ICollider> monsters = new List<ICollider>();
        private readonly IList<ICollider> triggers = new List<ICollider>();
        private readonly IList<ICollider> props = new List<ICollider>();
        private readonly IList<ICollider> items = new List<ICollider>();
        private readonly IList<ICollider> hazards = new List<ICollider>();

        private IAbyssMap map;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneScene"/> class.
        /// </summary>
        public ZoneScene()
            : base("Zone", "Main gameplay scene in Abyss.")
        {
            Monde.GameManager.GameBegun += OnGameBegun;
            PhaseManager.PlaneFolded += GameManagerOnPhaseChanged;
            PhaseManager.PlaneCoalesced += GameManagerOnPhaseChanged;
        }

        /// <summary>
        /// Gets the map.
        /// </summary>
        public IAbyssMap Map
        {
            get
            {
                if (this.map == null)
                {
                    this.map = new AbyssMap();
                    this.map.Initialize();
                }

                return this.map;
            }
        }

        /// <summary>
        /// Handles the input for this scene.
        /// </summary>
        /// <param name="inputState">State of the input.</param>
        public override void HandleInput(IInputState inputState)
        {
            PlayerIndex playerIndex;

#if DEBUG
            // Check whether we should quit the game.
            if (inputState.IsNewKeyPress(Keys.Q, PlayerIndex.One, out playerIndex))
            {
                Monde.ActivateScene("GameOver");
            }

            // Check debug layer flags.
            if (inputState.IsNewKeyPress(Keys.F1, PlayerIndex.One, out playerIndex))
            {
                var debugLayer = this.Layers[PhysicsDebugLayerKey];
                if (debugLayer.Flags[EngineFlag.Active])
                {
                    debugLayer.Suspend();
                }
                else
                {
                    debugLayer.Resume();
                }
            }

            if (inputState.IsNewKeyPress(Keys.F2, PlayerIndex.One, out playerIndex))
            {
                //var debugLayer = this.Layers[DebugLayerKey];
                //if (debugLayer.Flags[EngineFlag.Active])
                //{
                //    debugLayer.Suspend();
                //}
                //else
                //{
                //    debugLayer.Resume();
                //}
            }

            if (inputState.IsNewKeyPress(Keys.Enter, PlayerIndex.One, out playerIndex))
            {
                // Shake the camera.
                DrawingManager.Camera.Shake(10f, 2f, Ease.QuadIn);
            }

            // Cycle through shaders.
            if (inputState.IsNewKeyPress(Keys.F3, PlayerIndex.One, out playerIndex))
            {
                ShaderManager.ClearActiveShaders();
                if (this.currentShaderIndex == ShaderManager.SupportedShaders.Count)
                {
                    this.currentShaderIndex = 0;
                }
                else
                {
                    ShaderManager.SupportedShaders.Values.ElementAt(this.currentShaderIndex).Activate();
                    this.currentShaderIndex++;
                }
            }

            // Set ambient light value.
            if (inputState.IsNewKeyPress(Keys.OemPlus, PlayerIndex.One, out playerIndex))
            {
                Monde.GameManager.LightsOn();
            }

            if (inputState.IsNewKeyPress(Keys.OemMinus, PlayerIndex.One, out playerIndex))
            {
                Monde.GameManager.LightsOut();
            }

            if (inputState.IsNewKeyPress(Keys.F4, PlayerIndex.One, out playerIndex))
            {
                this.currentLightIndex++;
                if (this.currentLightIndex == this.ambientLightColors.Count)
                {
                    this.currentLightIndex = 0;
                }

                DrawingManager.AmbientLightColor = this.ambientLightColors[this.currentLightIndex];
            }

            if (inputState.IsNewKeyPress(Keys.F5, PlayerIndex.One, out playerIndex))
            {
                Monde.GameManager.LightsOut();
            }

            if (inputState.IsNewKeyPress(Keys.F6, PlayerIndex.One, out playerIndex))
            {
                Monde.GameManager.LightsOn();
            }

            if (inputState.IsNewKeyPress(Keys.F7, PlayerIndex.One, out playerIndex))
            {
                Monde.ChangeFramerate(30f, 0);
            }

            if (inputState.IsNewKeyPress(Keys.F8, PlayerIndex.One, out playerIndex))
            {
                Monde.ChangeFramerate(60f, 0);
            }

            if (inputState.IsNewKeyPress(Keys.Y, PlayerIndex.One, out playerIndex))
            {
                this.AddEntityUpdateCache(ItemFactory.GetRiftShard(RiftShard.Small, Monde.GameManager.UniverseViewPort.GetRandomPoint()), Monde.GameManager.ViewPort, GameplayLayerKey);
            }
#endif

            // Handle player input.
            Monde.GameManager.Player.HandleInput(inputState);
        }

        /// <summary>
        /// Adds layers to this scene.
        /// </summary>
        public override void AddLayers()
        {
            this.Layers.Add(BackgroundLayerKey, new ZoneBackgroundLayer(this));
            this.Layers.Add(GameplayLayerKey, new ZoneGameplayLayer(this));
            this.Layers.Add(InterfaceLayerKey, new ZoneInterfaceLayer(this));

#if DEBUG
            this.Layers.Add(PhysicsDebugLayerKey, new ZonePhysicsDebugLayer(this));
#endif
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            // Update the GameManager.
            Monde.GameManager.Update(gameTime, inputState);

            // Update the cache to ensure we only update and draw entities that are relevant.
            if (this.refreshCache)
            {
                Layers[GameplayLayerKey].UpdateEntityCache(Monde.GameManager.ViewPort);
                ((IFullScreenParticleEffect)this.TagCache["FullScreenParticleEffect"][0]).UpdateParticleCache(Monde.GameManager.ViewPort);

                // Refresh the level geometry.
                this.levelGeometryColliders.Clear();
                foreach (var r in this.Map.ViewPortTileBoundingBoxes(Monde.GameManager.ViewPort))
                {
                    this.levelGeometryColliders.Add(r);
                }

                this.refreshCache = false;
            }

            // Platforms and colliders.
            this.platforms.Clear();
            this.platformColliders.Clear();
            foreach (var r in this.levelGeometryColliders)
            {
                this.platformColliders.Add(r);
            }
            
            if (this.TagCache.ContainsKey(EntityTags.Platform))
            {
                foreach (var p in this.TagCache[EntityTags.Platform])
                {
                    if (p.Flags[EngineFlag.Active] && p.Flags[EngineFlag.Collidable])
                    {
                        this.platforms.Add(p.Collider);
                    }
                }

                foreach (var p in platforms)
                {
                    this.platformColliders.Add(p.QualifiedBoundingBox);
                }
            }

            // Update entities and handle input.
            base.Update(gameTime, inputState);

            // TODO: Figure out a better way to handle this.
            Monde.GameManager.Player.CurrentProp = null;

            // Handle physics and determine whether relevantActor cache should be invalidated.
            var previousPosition = Monde.GameManager.Player.Transform.GridPosition;

            if (Monde.GameManager.Player.Flags[EngineFlag.Active])
            {
                if (!Monde.GameManager.Player.Collider.Flags[PhysicsFlag.CollidesWithEnvironment])
                {
                    PhysicsManager.Apply(gameTime, Monde.GameManager.Player.Collider, new List<Rectangle>());
                }
                else
                {
                    PhysicsManager.Apply(gameTime, Monde.GameManager.Player.Collider, platformColliders);
                }
            }

            // Handle collisions with other entities.
            this.monsters.Clear();
            this.triggers.Clear();
            this.props.Clear();
            this.items.Clear();
            this.hazards.Clear();
            foreach (var e in this.Layers[GameplayLayerKey].Entities)
            {
                if (e.Flags[EngineFlag.Collidable])
                {
                    if (e.Tags.Contains(EntityTags.Monster))
                    {
                        this.monsters.Add(e.Collider);
                    }
                    else if (e.Tags.Contains(EntityTags.Trigger))
                    {
                        this.triggers.Add(e.Collider);
                    }
                    else if (e.Tags.Contains(EntityTags.Prop))
                    {
                        this.props.Add(e.Collider);
                    }
                    else if (e.Tags.Contains(EntityTags.Item))
                    {
                        this.items.Add(e.Collider);
                    }
                    else if (e.Tags.Contains(EntityTags.Hazard))
                    {
                        this.hazards.Add(e.Collider);
                    }
                }
            }

            if (Monde.GameManager.Player.Flags[EngineFlag.Collidable])
            {
                PhysicsManager.HandleCollisionsWithEntities(Monde.GameManager.Player.Collider, this.props);
                PhysicsManager.HandleCollisionsWithEntities(Monde.GameManager.Player.Collider, this.monsters);
                PhysicsManager.HandleCollisionsWithEntities(Monde.GameManager.Player.Collider, this.items);
                PhysicsManager.HandleCollisionsWithEntities(Monde.GameManager.Player.Collider, this.platforms);
                PhysicsManager.HandleCollisionsWithEntities(Monde.GameManager.Player.Collider, this.triggers);
                if (this.TagCache.ContainsKey(EntityTags.Hazard))
                {
                    PhysicsManager.HandleCollisionsWithEntities(Monde.GameManager.Player.Collider, this.hazards);
                }
            }

            // Handle collisions between monsters and triggers.
            //foreach (var t in triggers)
            //{
            //    PhysicsHelper.HandleCollisionsWithEntities(t, monsters);
            //}

            if (previousPosition.Y != Monde.GameManager.Player.Transform.GridPosition.Y)
            {
                this.refreshCache = true;
                Monde.GameManager.CurrentDepth++;
                Monde.GameManager.StatisticManager.TotalScore += 5;
            }

            // Apply physics to monsters as required.
            foreach (var c in this.monsters)
            {
                PhysicsManager.Apply(gameTime, c, platformColliders);
            }

#if DEBUG
            Monde.Debugger.Add(PlatformCollidersDebugKey, platformColliders.Count.ToString(CultureInfo.InvariantCulture));
#endif
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Clear caching collections.
            this.levelGeometryColliders.Clear();
            this.platformColliders.Clear();

            // Reset dimension.
            Monde.GameManager.CurrentDimension = Dimension.Limbo;

            // Reset the game manager and player.
            Monde.GameManager.Initialize();
            Monde.GameManager.Player = new Player();
            Monde.GameManager.Player.Initialize();

            if (Monde.GameManager.RelicCollection == null)
            {
                Monde.GameManager.RelicCollection = new RelicCollection();
            }
            else
            {
                Monde.GameManager.RelicCollection.Reset();
            }

            // Reset the camera to the correct focus.
            DrawingManager.Camera.Focus = Monde.GameManager.Player;
            DrawingManager.Camera.Offset = new Vector2(0, -Monde.GameManager.Player.Collider.BoundingBox.Height);
            DrawingManager.Camera.Flags[CameraFlag.LockFocusHorizontal] = false;

            // Hook up player events.
            Monde.GameManager.RelicCollection.RelicsActivated += RelicCollectionOnRelicsActivated;
            
            // Clear all the entities out of the Gameplay layer.
            var gameplayLayer = this.Layers[GameplayLayerKey];

            // Add the player to the gameplay layer so that it can update and draw properly.
            this.AddEntity(Monde.GameManager.Player, GameplayLayerKey);
            this.AddEntity(Monde.GameManager.Player.Barrier, GameplayLayerKey);

            // Generate a new map and add static entities as required.
            // TODO: Change the full screen particle effect based on the current area (probably can't be done in the scene, rather in the layer).
            this.Map.Initialize();
            var mapEntities = this.Map.Generate(Monde.GameManager.GameMode, 5);
            gameplayLayer.AddEntity(this.Map);
            gameplayLayer.AddEntity(ParticleEffectFactory.GetFullScreenParticleEffect(ZoneType.Normal, ParticleDensity.Low));

            // Generate a new map and place the entities so they can update properly.
            foreach (var pair in mapEntities)
            {
                foreach (var e in pair.Value)
                {
                    gameplayLayer.AddEntity(e);
                }
            }

            this.refreshCache = true;
        }

        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public override void End()
        {
            // Remove the effects of all the current phases.
            foreach (var p in PhaseManager.CurrentPhases)
            {
                p.Remove(this.Layers[GameplayLayerKey]);
            }

            base.End();

            // Unhook GameManager events.
            Monde.GameManager.RelicCollection.RelicsActivated -= RelicCollectionOnRelicsActivated;

            Monde.GameManager.End();
        }

        private void RelicCollectionOnRelicsActivated(RelicsActivatedEventArgs relicsActivatedEventArgs)
        {
            foreach (var r in relicsActivatedEventArgs.Relics)
            {
                // Depending on the scope of entities that need to be affected, either use the full TagCache, or just take a subset of
                // cached entities to reduce the amount of processing that needs to be done.
                if (r.UsesWholeEntityCache)
                {
                    var entityCache = new List<IEntity>();

                    // Use the entire tag cache here, but limit it to particular tags.
                    foreach (var s in r.RelevantEntityTags)
                    {
                        entityCache.AddRange(this.TagCache[s]);
                    }

                    r.Activate(entityCache);
                }
                else
                {
                    var entityCache = new List<IEntity>();
                    foreach (var s in r.RelevantEntityTags)
                    {
                        foreach (var e in this.Layers[GameplayLayerKey].Entities)
                        {
                            var matchFound = false;
                            foreach (var t in e.Tags)
                            {
                                if (t.Equals(s))
                                {
                                    entityCache.Add(e);
                                    matchFound = true;
                                    break;
                                }
                            }

                            if (matchFound)
                            {
                                break;
                            }
                        }
                    }

                    r.Activate(entityCache);
                }
            }
        }

        private void GameManagerOnPhaseChanged(PhaseChangedEventArgs phaseChangedEventArgs)
        {
            if (this.Layers.ContainsKey(GameplayLayerKey))
            {
                if (phaseChangedEventArgs.PreviousPhases != null)
                {
                    foreach (var p in phaseChangedEventArgs.PreviousPhases)
                    {
                        p.Remove(this.Layers[GameplayLayerKey]);
                    }
                }

                if (phaseChangedEventArgs.NewPhase != null)
                {
                    phaseChangedEventArgs.NewPhase.Apply(this.Layers[GameplayLayerKey]);
                }
            }
        }

        private void OnGameBegun(object sender, EventArgs eventArgs)
        {
            this.Layers[InterfaceLayerKey].Begin();
        }
    }
}
