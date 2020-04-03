using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Components;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using System.Collections.Generic;
using Occasus.Core.Components.Logic;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public abstract class ParticleEffect : EntityComponent, IParticleEffect
    {
        protected readonly IList<IParticle> particleCloud;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffect" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="maximumParticles">The maximum particles.</param>
        /// <param name="offset">The offset.</param>
        protected ParticleEffect(
            IEntity parent,
            string name, 
            string description, 
            int maximumParticles, 
            Vector2 offset,
            Vector2 scale)
            : base(parent, name, description)
        {
            this.MaximumParticles = maximumParticles;
            this.Offset = offset;

            this.particleCloud = new List<IParticle>();
            this.Flags.Add(ParticleEffectFlag.Recycle, true);
            this.Flags.Add(ParticleEffectFlag.RecycleToViewPort, false);

            this.Scale = scale;
        }

        /// <summary>
        /// Gets the offset position for this particle effect.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.Parent.Transform.Position + this.Offset;
            }
        }

        /// <summary>
        /// Gets the offset for this particle effect.
        /// </summary>
        public Vector2 Offset
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating the maximum number of Particles can exist in this ParticleEffect.
        /// </summary>
        public int MaximumParticles
        {
            get; set;
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
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public Vector2 Scale
        {
            get; set;
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            base.Update(gameTime, inputState);

            var deadParticles = new List<IParticle>();
            foreach (var p in this.ParticleCloud)
            {
                if (p.FramesRemaining <= 0)
                {
                    deadParticles.Add(p);
                }
                else
                {
                    p.Update(gameTime);
                }
            }

            // Recycle particles if required.
            foreach (var p in deadParticles)
            {
                this.particleCloud.Remove(p);

                // Only add more particles if this effect is active, recycling and the particle should be recycled (phew).
                if (p.Flags[ParticleFlag.Recycle])
                {
                    this.particleCloud.Add(this.GetParticle());
                }
            }

            // Suspend this particle effect if we don't recycle particles and there are none left in the cloud.
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
                var color = p.Color;
                color *= p.Opacity;
                var origin = p.Flags[ParticleFlag.Rotate]
                    ? new Vector2(p.SourceRectangle.Center.X, p.SourceRectangle.Center.Y)
                    : Vector2.Zero;
                spriteBatch.Draw(p.Texture, p.Transform.Position, p.SourceRectangle, color, p.Transform.Rotation, origin, p.Transform.Scale, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Gets the particle.
        /// </summary>
        /// <returns>
        /// A new particle.
        /// </returns>
        public virtual IParticle GetParticle()
        {
            return null;
        }

        /// <summary>
        /// Emits this particle effect until it is ended.
        /// </summary>
        public virtual void Emit()
        {
            // Set this particle effect to recycle.
            this.Flags[ParticleEffectFlag.Recycle] = true;

            this.Resume();

            for (var i = 0; i < this.MaximumParticles; i++)
            {
                this.particleCloud.Add(this.GetParticle());
            }
        }

        /// <summary>
        /// Emits this particle effect for a set amount of time.
        /// </summary>
        /// <param name="frames"></param>
        public void Emit(int frames)
        {
            this.Emit();
            CoroutineManager.Add(EmitEffect(frames).GetEnumerator());
        }

        /// <summary>
        /// Stops this particle effect emitting.
        /// </summary>
        public void Stop()
        {
            this.Flags[ParticleEffectFlag.Recycle] = false;
        }

        private IEnumerable EmitEffect(int frames)
        {
            for (var i = 0; i < frames; i++)
            {
                yield return null;
            }

            this.Stop();
        }
    }
}
