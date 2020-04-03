using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Drawing.Shaders;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Layers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Occasus.Core.Scenes
{
    public abstract class Scene : EngineComponent, IScene
    {
        private readonly IList<IEntity> defferedRenderEntities = new List<IEntity>();
        private readonly IList<ILayer> targetLayers = new List<ILayer>();

        private float lightSourceOpacity = 1f;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        protected Scene(string name, string description)
            : base(name, description)
        {
            this.Layers = new Dictionary<string, ILayer>();
            this.TagCache = new Dictionary<string, IList<IEntity>>();
        }

        /// <summary>
        /// Gets the collection of sorted layers.
        /// </summary>
        public IDictionary<string, ILayer> Layers 
        { 
            get; private set;
        }

        /// <summary>
        /// Gets the tag cache collection. This allows the scene to easily query groups of entities based on a particular tag.
        /// </summary>
        public IDictionary<string, IList<IEntity>> TagCache
        {
            get; private set;
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            base.Update(gameTime, inputState);

            // Handle input for the scene.
            this.HandleInput(inputState);

            // Update each layer in the hierarchy.
            foreach (var layer in this.Layers.Values)
            {
                if (layer.Flags[EngineFlag.Active])
                {
                    layer.Update(gameTime, inputState);
                }
            }

            this.CleanUp();

#if DEBUG
#endif
        }

        //public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    base.Draw(gameTime, spriteBatch);

        //    // Get the set of layers we need to transform to camera and draw a basic scene.
        //    this.targetLayers.Clear();
        //    foreach (var l in this.Layers.Values)
        //    {
        //        if (l.LayerType == LayerType.TransformedToCamera && l.Flags[EngineFlag.Visible])
        //        {
        //            this.targetLayers.Add(l);
        //        }
        //    }

        //    // Draw the color map. Note that any entities that are tagged as being EngineFlags.DeferRender are excluded from this draw call.
        //    DrawingManager.GraphicsDevice.SetRenderTarget(DrawingManager.ColorMapRenderTarget);
        //    DrawingManager.GraphicsDevice.Clear(Color.Transparent);

        //    foreach (var layer in targetLayers)
        //    {
        //        layer.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, DrawingManager.Camera.Transform);
        //        layer.Draw(gameTime, layer.SpriteBatch);
        //        layer.SpriteBatch.End();
        //    }

        //    // Perform a lighting pass. This involves getting all light source components; even those from deferred rendering entities.
        //    DrawingManager.GraphicsDevice.SetRenderTarget(DrawingManager.DeferredLightingRenderTarget);
        //    DrawingManager.GraphicsDevice.Clear(Color.Transparent);

        //    DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, DrawingManager.Camera.Transform);
        //    foreach (var l in this.Layers.Values)
        //    {
        //        foreach (var light in l.LightingCache)
        //        {
        //            if (light.Flags[EngineFlag.Active])
        //            {
        //                light.Draw(gameTime, DrawingManager.SpriteBatch);
        //            }
        //        }
        //    }
        //    DrawingManager.SpriteBatch.End();

        //    // Change to our final scene render target.
        //    DrawingManager.GraphicsDevice.SetRenderTarget(DrawingManager.ShaderRenderTarget);
        //    DrawingManager.GraphicsDevice.Clear(Color.Transparent);

        //    DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

        //    // Draw a black shadow mask to blend against.
        //    DrawingManager.LightingShader.CurrentTechnique = DrawingManager.LightingShader.Techniques["BlackMaskTechnique"];
        //    DrawingManager.LightingShader.CurrentTechnique.Passes[0].Apply();
        //    DrawingManager.SpriteBatch.Draw(DrawingManager.ColorMapRenderTarget, Vector2.Zero, Color.White);

        //    // Draw our scene with all deferred lighting included.
        //    DrawingManager.LightingShader.CurrentTechnique = DrawingManager.LightingShader.Techniques["DeferredLightingTechnique"];

        //    DrawingManager.LightingShader.Parameters["AmbientIntensity"].SetValue(DrawingManager.AmbientLightValue);
        //    DrawingManager.LightingShader.Parameters["AmbientColor"].SetValue(DrawingManager.AmbientLightColor.ToVector4());
        //    DrawingManager.LightingShader.Parameters["ShadowMap"].SetValue(DrawingManager.DeferredLightingRenderTarget);

        //    DrawingManager.LightingShader.CurrentTechnique.Passes[0].Apply();
        //    DrawingManager.SpriteBatch.Draw(DrawingManager.ColorMapRenderTarget, Vector2.Zero, Color.White);
        //    DrawingManager.SpriteBatch.End();

        //    // We need to be creative with our deferred render entities here. Some may need to be affected by our final shader; others may not.
        //    this.defferedRenderEntities.Clear();
        //    var containsDeferredRenderEntities = false;
        //    if (this.TagCache.ContainsKey(Lighting.DeferredRenderEntity))
        //    {
        //        containsDeferredRenderEntities = true;
        //        foreach (var t in targetLayers)
        //        {
        //            foreach (var e in t.Entities)
        //            {
        //                if (e.Flags[EngineFlag.DeferredRender])
        //                {
        //                    this.defferedRenderEntities.Add(e);
        //                }
        //            }
        //        }

        //        if (ShaderManager.CurrentShader != null && ShaderManager.CurrentShader.Usage == ShaderUsage.IncludeDeferredRenderEntities)
        //        {
        //            DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, DrawingManager.Camera.Transform);

        //            foreach (var e in this.TagCache[Lighting.DeferredRenderEntity])
        //            {
        //                e.Draw(gameTime, DrawingManager.SpriteBatch);

        //                // Remove the entity from the deferred render entity list so those can draw after the shader is drawn.
        //                this.defferedRenderEntities.Remove(e);
        //            }

        //            DrawingManager.SpriteBatch.End();
        //        }
        //    }

        //    // Change to the back buffer.
        //    DrawingManager.GraphicsDevice.SetRenderTarget(DrawingManager.BackBufferRenderTarget);
        //    DrawingManager.GraphicsDevice.Clear(Color.Transparent);

        //    DrawingManager.LightingShader.CurrentTechnique = null;
        //    DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);

        //    // Draw static layers.
        //    foreach (var layer in this.Layers.Values)
        //    {
        //        if (layer.LayerType == LayerType.Static)
        //        {
        //            layer.Draw(gameTime, DrawingManager.SpriteBatch);
        //        }
        //    }

        //    DrawingManager.SpriteBatch.Draw(DrawingManager.ShaderRenderTarget, Vector2.Zero, Color.White);
        //    DrawingManager.SpriteBatch.End();

        //    // Draw any deferred render entities that are not effected by the final shader effect.
        //    if (containsDeferredRenderEntities)
        //    {
        //        DrawingManager.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, DrawingManager.Camera.Transform);

        //        foreach (var e in this.defferedRenderEntities)
        //        {
        //            e.Draw(gameTime, DrawingManager.SpriteBatch);
        //        }

        //        DrawingManager.SpriteBatch.End();
        //    }

        //    // Scale the final render target.
        //    DrawingManager.GraphicsDevice.SetRenderTarget(DrawingManager.ScaleRenderTarget);
        //    DrawingManager.GraphicsDevice.Clear(Color.Transparent);
        //    DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);
        //    DrawingManager.SpriteBatch.Draw(DrawingManager.BackBufferRenderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, DrawingManager.WindowScale, SpriteEffects.None, 0f);
        //    DrawingManager.SpriteBatch.End();

        //    // Draw the final scene.
        //    DrawingManager.GraphicsDevice.SetRenderTarget(null);
        //    DrawingManager.GraphicsDevice.Clear(Color.Transparent);

        //    DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);

        //    if (ShaderManager.CurrentShader != null)
        //    {
        //        ShaderManager.CurrentShader.Effect.CurrentTechnique.Passes[0].Apply();
        //    }

        //    DrawingManager.SpriteBatch.Draw(DrawingManager.ScaleRenderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        //    DrawingManager.SpriteBatch.End();

        //    // Draw any interface layers.
        //    DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);
        //    foreach (var layer in this.Layers.Values)
        //    {
        //        if (layer.LayerType == LayerType.Interface && layer.Flags[EngineFlag.Visible])
        //        {
        //            layer.Draw(gameTime, DrawingManager.SpriteBatch);
        //        }
        //    }

        //    DrawingManager.SpriteBatch.End();
        //}

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // Get the set of layers we need to transform to camera and draw a basic scene.
            this.targetLayers.Clear();
            foreach (var l in this.Layers.Values)
            {
                if (l.LayerType == LayerType.TransformedToCamera && l.Flags[EngineFlag.Visible])
                {
                    this.targetLayers.Add(l);
                }
            }

            // Draw the color map. Note that any entities that are tagged as being EngineFlags.DeferRender are excluded from this draw call.
            DrawingManager.GraphicsDevice.SetRenderTarget(DrawingManager.ColorMapRenderTarget);
            DrawingManager.GraphicsDevice.Clear(Color.Transparent);

            foreach (var layer in targetLayers)
            {
                layer.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, DrawingManager.Camera.Transform);
                layer.Draw(gameTime, layer.SpriteBatch);
                layer.SpriteBatch.End();
            }

            // Perform a lighting pass. This involves getting all light source components; even those from deferred rendering entities.
            DrawingManager.GraphicsDevice.SetRenderTarget(DrawingManager.DeferredLightingRenderTarget);
            DrawingManager.GraphicsDevice.Clear(Color.Transparent);

            DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, DrawingManager.Camera.Transform);
            foreach (var l in this.Layers.Values)
            {
                foreach (var light in l.LightingCache)
                {
                    if (light.Flags[EngineFlag.Active])
                    {
                        light.Draw(gameTime, DrawingManager.SpriteBatch);
                    }
                }
            }
            DrawingManager.SpriteBatch.End();

            // Change to our final scene render target.
            DrawingManager.GraphicsDevice.SetRenderTarget(DrawingManager.ShaderRenderTarget);
            DrawingManager.GraphicsDevice.Clear(Color.Transparent);

            // Draw static layers.
            DrawingManager.LightingShader.CurrentTechnique = null;
            DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
            
            foreach (var layer in this.Layers.Values)
            {
                if (layer.LayerType == LayerType.Static)
                {
                    layer.Draw(gameTime, DrawingManager.SpriteBatch);
                }
            }

            DrawingManager.SpriteBatch.Draw(DrawingManager.ShaderRenderTarget, Vector2.Zero, Color.White);
            DrawingManager.SpriteBatch.End();

            DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            // Draw a black shadow mask to blend against.
            DrawingManager.LightingShader.CurrentTechnique = DrawingManager.LightingShader.Techniques["BlackMaskTechnique"];
            DrawingManager.LightingShader.CurrentTechnique.Passes[0].Apply();
            DrawingManager.SpriteBatch.Draw(DrawingManager.ColorMapRenderTarget, Vector2.Zero, Color.White);

            // Draw our scene with all deferred lighting included.
            DrawingManager.LightingShader.CurrentTechnique = DrawingManager.LightingShader.Techniques["DeferredLightingTechnique"];

            DrawingManager.LightingShader.Parameters["AmbientIntensity"].SetValue(DrawingManager.AmbientLightValue);
            DrawingManager.LightingShader.Parameters["AmbientColor"].SetValue(DrawingManager.AmbientLightColor.ToVector4());
            DrawingManager.LightingShader.Parameters["ShadowMap"].SetValue(DrawingManager.DeferredLightingRenderTarget);

            DrawingManager.LightingShader.CurrentTechnique.Passes[0].Apply();
            DrawingManager.SpriteBatch.Draw(DrawingManager.ColorMapRenderTarget, Vector2.Zero, Color.White);
            DrawingManager.SpriteBatch.End();

            // We need to be creative with our deferred render entities here. Some may need to be affected by our final shader; others may not.
            this.defferedRenderEntities.Clear();
            var containsDeferredRenderEntities = false;
            if (this.TagCache.ContainsKey(Lighting.DeferredRenderEntity))
            {
                containsDeferredRenderEntities = true;
                foreach (var t in targetLayers)
                {
                    foreach (var e in t.Entities)
                    {
                        if (e.Flags[EngineFlag.DeferredRender])
                        {
                            this.defferedRenderEntities.Add(e);
                        }
                    }
                }

                if (ShaderManager.CurrentShader != null && ShaderManager.CurrentShader.Usages[ShaderUsage.IncludeDeferredRenderEntities])
                {
                    DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, DrawingManager.Camera.Transform);

                    foreach (var e in this.TagCache[Lighting.DeferredRenderEntity])
                    {
                        e.Draw(gameTime, DrawingManager.SpriteBatch);

                        // Remove the entity from the deferred render entity list so those can draw after the shader is drawn.
                        this.defferedRenderEntities.Remove(e);
                    }

                    DrawingManager.SpriteBatch.End();
                }
            }

            // Draw any deferred render entities that are not effected by the final shader effect.
            if (containsDeferredRenderEntities)
            {
                DrawingManager.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, DrawingManager.Camera.Transform);

                foreach (var e in this.defferedRenderEntities)
                {
                    e.Draw(gameTime, DrawingManager.SpriteBatch);
                }

                DrawingManager.SpriteBatch.End();
            }

            // If we have shaders that apply before the final scene, do that here. This has to be a little bit creative.
            // This mainly applies to Vertical and Horizontal flipping, but the general gist is that we have to do some interesting things
            // to ensure that certain shaders that make changes to pixel screen coordinates must be done on a pre-scaled image, otherwise it all turns
            // to garbage.
            if (ShaderManager.CurrentShader != null && ShaderManager.CurrentShader.Usages[ShaderUsage.ApplyBeforeScale])
            {
                // Apply the shader as required using the shader's draw offset (set in the shader itself) using a different buffer.
                DrawingManager.GraphicsDevice.SetRenderTarget(DrawingManager.ScaleRenderTarget);
                DrawingManager.GraphicsDevice.Clear(Color.Transparent);
                DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);
                ShaderManager.CurrentShader.Effect.CurrentTechnique.Passes[0].Apply();
                DrawingManager.SpriteBatch.Draw(DrawingManager.ShaderRenderTarget, ShaderManager.CurrentShader.DrawOffset, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                DrawingManager.SpriteBatch.End();

                // Draw the final scene to the backbuffer
                DrawingManager.GraphicsDevice.SetRenderTarget(null);
                DrawingManager.GraphicsDevice.Clear(Color.Transparent);
                DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);
                DrawingManager.SpriteBatch.Draw(DrawingManager.ScaleRenderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, DrawingManager.WindowScale, SpriteEffects.None, 0f);
                DrawingManager.SpriteBatch.End();
            }
            else
            {
                // Draw the final scene as normal, applying whatever shaders we want.
                // TODO: This could probably be further optimized, but fuck it, it works.
                DrawingManager.GraphicsDevice.SetRenderTarget(null);
                DrawingManager.GraphicsDevice.Clear(Color.Transparent);

                DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);

                var drawOffset = Vector2.Zero;
                if (ShaderManager.CurrentShader != null && !ShaderManager.CurrentShader.Usages[ShaderUsage.ApplyBeforeScale])
                {
                    ShaderManager.CurrentShader.Effect.CurrentTechnique.Passes[0].Apply();
                    drawOffset = ShaderManager.CurrentShader.DrawOffset;
                }

                DrawingManager.SpriteBatch.Draw(DrawingManager.ShaderRenderTarget, drawOffset, null, Color.White, 0, Vector2.Zero, DrawingManager.WindowScale, SpriteEffects.None, 0f);
                DrawingManager.SpriteBatch.End();
            }

            // Draw any interface layers.
            DrawingManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.Identity);
            foreach (var layer in this.Layers.Values)
            {
                if (layer.LayerType == LayerType.Interface && layer.Flags[EngineFlag.Visible])
                {
                    layer.Draw(gameTime, DrawingManager.SpriteBatch);
                }
            }

#if DEBUG
            // Draw debug information.
            Engine.Debugger.Draw(gameTime, DrawingManager.SpriteBatch);
#endif

            DrawingManager.SpriteBatch.End();
        }

        /// <summary>
        /// Handles the input for this scene.
        /// </summary>
        /// <param name="inputState">State of the input.</param>
        public abstract void HandleInput(IInputState inputState);

        /// <summary>
        /// Adds layers to this scene.
        /// </summary>
        public abstract void AddLayers();

        /// <summary>
        /// Adds an entity to the required layer and this scene's TagCache. This method will not update the Layer's entity cache.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="layer">The layer.</param>
        public void AddEntity(IEntity entity, string layer)
        {
            if (!this.Layers.ContainsKey(layer))
            {
                Debug.WriteLine("AddEntity() - Layer: " + layer + " does not exist.");
                return;
            }

            this.Layers[layer].AddEntity(entity);
        }

        /// <summary>
        /// Removes an entity from the designated layer as well as this scene's TagCache.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="layer">The layer.</param>
        public void RemoveEntity(IEntity entity, string layer)
        {
            if (!this.Layers.ContainsKey(layer))
            {
                Debug.WriteLine("RemoveEntity() - Layer: " + layer + " does not exist.");
                return;
            }

            this.Layers[layer].RemoveEntity(entity);
        }

        /// <summary>
        /// Adds an entity to the designated layer and immediately updates the layer's entity cache.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="viewPort">The view port.</param>
        /// <param name="layer">The layer.</param>
        public void AddEntityUpdateCache(IEntity entity, Rectangle viewPort, string layer = null)
        {
            this.AddEntity(entity, layer);
            this.UpdateEntityCache(viewPort, layer);
        }

        /// <summary>
        /// Forces updating the entity cache for the specified layer.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        /// <param name="layer">The layer.</param>
        public void UpdateEntityCache(Rectangle viewPort, string layer = null)
        {
            if (!this.Layers.ContainsKey(layer))
            {
                Debug.WriteLine("UpdateEntityCache() - Layer: " + layer + " does not exist.");
                return;
            }

            if (layer == null)
            {
                foreach (var l in this.Layers.Values)
                {
                    l.UpdateEntityCache(viewPort);
                }
            }
            else
            {
                this.Layers[layer].UpdateEntityCache(viewPort);
            }
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Add layers as required.
            this.AddLayers();

            foreach (var l in this.Layers.Values)
            {
                l.Initialize();
            }
        }
        
        /// <summary>
        /// Ends the Engine Component.
        /// </summary>
        public override void End()
        {
            base.End();

            // End all of the layers in this scene and remove them.
            foreach (var l in this.Layers.Values)
            {
                l.End();
            }

            this.Layers.Clear();

            // Clear the Tag Cache.
            this.TagCache.Clear();
        }

        /// <summary>
        /// Suspends the Engine Component from updating or drawing.
        /// </summary>
        public override void Suspend()
        {
            base.Suspend();

            foreach (var l in this.Layers.Values)
            {
                l.Suspend();
            }
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            foreach (var l in this.Layers.Values)
            {
                if (!l.Flags[EngineFlag.DeferredBegin])
                {
                    l.Begin();
                }
            }
        }

        /// <summary>
        /// Resumes updating and drawing of the Engine Component.
        /// </summary>
        public override void Resume()
        {
            base.Resume();

            foreach (var l in this.Layers.Values)
            {
                l.Resume();
            }
        }

        private void CleanUp()
        {
            // Clean up all irrelevent entities in each layer so they are removed from the game.
            foreach (var layer in this.Layers.Values)
            {
                if (layer.Flags[EngineFlag.Active])
                {
                    foreach (var e in layer.Entities.ToList())
                    {
                        if (!e.Flags[EngineFlag.Relevant])
                        {
                            layer.RemoveEntity(e);
                            e.Dispose();
                        }
                    }
                }
            }
        }
    }
}
