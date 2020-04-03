using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections.Generic;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public abstract class FullScreenParticleEffect : Entity, IFullScreenParticleEffect
    {
        private readonly IList<IParticle> particleCloud = new List<IParticle>();
        private readonly IList<IParticle> allParticles = new List<IParticle>();
        private readonly IList<IParticle> deadParticles = new List<IParticle>();

        private int cachedHighVerticalBoundary;
        private int cachedLowVerticalBoundary;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullScreenParticleEffect"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="particleDensity">The particle density.</param>
        protected FullScreenParticleEffect(
            string name,
            string description,
            ParticleDensity particleDensity)
            : base(name, description)
        {
            this.Tags.Add("FullScreenParticleEffect");
            this.ParticleDensity = particleDensity;
            this.Flags.Add(ParticleEffectFlag.Recycle, true);
            this.Flags.Add(ParticleEffectFlag.RecycleToViewPort, true);

            // Setup lighting.
            this.Tags.Add(Lighting.Lighting.DeferredRenderEntity);
            this.Flags[EngineFlag.DeferredRender] = true;
        }

        /// <summary>
        /// Gets the particle density.
        /// </summary>
        public ParticleDensity ParticleDensity
        {
            get; private set;
        }

        /// <summary>
        /// Gets the particle cloud.
        /// </summary>
        public IEnumerable<IParticle> ParticleCloud
        {
            get
            {
                return this.particleCloud;
            }
        }

        /// <summary>
        /// Updates the particle cache.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        public void UpdateParticleCache(Rectangle viewPort)
        {
            this.cachedHighVerticalBoundary = viewPort.Top * DrawingManager.TileHeight;
            this.cachedLowVerticalBoundary = viewPort.Bottom * DrawingManager.TileHeight;
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            foreach (var p in this.ParticleCloud)
            {
                if (p.FramesRemaining <= 0 && p.Flags[ParticleFlag.Recycle])
                {
                    this.deadParticles.Add(p);
                }
                else
                {
                    p.Update(gameTime);
                }
            }

            // Recycle particles.
            foreach (var p in this.deadParticles)
            {
                this.allParticles.Remove(p);
                this.particleCloud.Remove(p);

                // Determine a slightly randomized position for the new particle.
                var particle = this.GetParticle();
                var position = p.Transform.Position;
                position.X = MathsHelper.Random(0, DrawingManager.ScreenWidth);
                if (this.Flags[ParticleEffectFlag.RecycleToViewPort])
                {
                    position.Y = MathsHelper.Random(this.cachedHighVerticalBoundary, this.cachedLowVerticalBoundary);
                }
                else
                {
                    position.Y += MathsHelper.Random(-1, 1);
                }

                particle.Transform.Position = position;

                // Add the particle to both the cloud and the cache so that it shows up straight away.
                this.allParticles.Add(particle);
                this.particleCloud.Add(particle);
            }

            // Clear the dead particle list.
            this.deadParticles.Clear();
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var p in this.ParticleCloud)
            {
                var color = Color.White;
                color *= p.Opacity;
                spriteBatch.Draw(p.Texture, p.Transform.Position, p.SourceRectangle, color, p.Transform.Rotation, Vector2.Zero, p.Transform.Scale, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            var particleDensity = (int)this.ParticleDensity;
            for (var i = 0; i < particleDensity; i++)
            {
                var x = MathsHelper.Random(PhysicsManager.MapWidth);
                var y = MathsHelper.Random(0, 20);

                var particle = this.GetParticle();
                particle.Transform.Position = new Vector2(x * DrawingManager.TileWidth, y * DrawingManager.TileHeight);
                this.allParticles.Add(particle);
                this.particleCloud.Add(particle);
            }

            base.Initialize();
        }

        /// <summary>
        /// Gets the particle.
        /// </summary>
        /// <returns>A new particle.</returns>
        protected abstract IParticle GetParticle();
    }
}