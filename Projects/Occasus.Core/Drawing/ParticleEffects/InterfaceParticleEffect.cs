using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Components.Logic;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using System.Collections.Generic;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public abstract class InterfaceParticleEffect : Entity, IInterfaceParticleEffect
    {
        private readonly IList<IParticle> particleCloud;
        private readonly string emittingKey;

        private int maximumParticles;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceParticleEffect"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="particleDensity">The particle density.</param>
        protected InterfaceParticleEffect(
            string name,
            string description,
            ParticleDensity particleDensity)
            : base(name, description)
        {
            this.ParticleDensity = particleDensity;
            this.particleCloud = new List<IParticle>();

            // Coroutine keys.
            this.emittingKey = this.Id + "_Emitting";
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get; set;
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
        /// Emits this particle effect.
        /// </summary>
        public void Emit()
        {
            this.EmitInternal();
        }

        /// <summary>
        /// Emits this particle effect for a specified duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public void Emit(float duration)
        {
            this.EmitInternal();
            CoroutineManager.Add(emittingKey, this.ParticleFadeOutEffect(duration));
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            var deadParticles = new List<IParticle>();
            foreach (var p in this.ParticleCloud)
            {
                if (p.FramesRemaining <= 0 && p.Flags[ParticleFlag.Recycle])
                {
                    deadParticles.Add(p);
                }
                else
                {
                    p.Update(gameTime);
                }
            }

            // Recycle particles if we still want to.
            foreach (var p in deadParticles)
            {
                this.particleCloud.Remove(p);

                if (this.Flags[ParticleEffectFlag.Recycle] && this.particleCloud.Count < this.maximumParticles)
                {
                    this.particleCloud.Add(this.GetParticle());
                }
            }

            // If we have no particles left and are not set to recycling; it's ok to suspend this particle effect.
            if (this.particleCloud.Count == 0 && !this.Flags[ParticleEffectFlag.Recycle])
            {
                this.Suspend();
            }
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
        /// Gets the particle.
        /// </summary>
        /// <returns>A new particle.</returns>
        protected abstract IParticle GetParticle();

        protected override void InitializeTags()
        {
            this.Tags.Add("InterfaceParticleEffect");
        }

        protected override void InitializeFlags()
        {
            this.Flags.Add(ParticleEffectFlag.Recycle, true);
            this.Flags.Add(ParticleEffectFlag.RecycleToViewPort, false);
        }

        private void EmitInternal()
        {
            this.Flags[ParticleEffectFlag.Recycle] = true;
            this.Resume();

            this.maximumParticles = (int)this.ParticleDensity;
            for (var i = 0; i < this.maximumParticles; i++)
            {
                this.particleCloud.Add(this.GetParticle());
            }
        }

        private IEnumerator ParticleFadeOutEffect(float duration)
        {
            // Determine the number of frames that the phase requires to complete.
            var framesLeft = (float)Math.Round(duration / Engine<IGameManager<IEntity>>.DeltaTime);
            var elapsedFrames = 0;

            var cachedMaximumParticles = this.maximumParticles;
            while (elapsedFrames <= framesLeft)
            {
                this.maximumParticles = (int)Math.Round(cachedMaximumParticles * (1 - (elapsedFrames / framesLeft)));
                yield return null;
                elapsedFrames++;
            }

            this.Flags[ParticleEffectFlag.Recycle] = false;
        }
    }
}
