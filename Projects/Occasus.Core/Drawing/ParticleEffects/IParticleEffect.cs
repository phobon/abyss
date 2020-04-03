using Microsoft.Xna.Framework;
using Occasus.Core.Components;
using System.Collections.Generic;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public interface IParticleEffect : IEntityComponent
    {
        /// <summary>
        /// Gets the offset position for this particle effect.
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets the offset for this particle effect.
        /// </summary>
        Vector2 Offset { get; }

        /// <summary>
        /// Gets or sets a value indicating the maximum number of Particles can exist in this ParticleEffect.
        /// </summary>
        int MaximumParticles { get; set; }

        /// <summary>
        /// Gets the particle cloud.
        /// </summary>
        IEnumerable<IParticle> ParticleCloud { get; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        Vector2 Scale { get; set; }

        /// <summary>
        /// Gets the particle.
        /// </summary>
        /// <returns>A new particle.</returns>
        IParticle GetParticle();

        /// <summary>
        /// Emits this particle effect until it is ended.
        /// </summary>
        void Emit();
        
        /// <summary>
        /// Emits this particle effect for a set amount of time.
        /// </summary>
        void Emit(int frames);

        /// <summary>
        /// Stops this particle effect emitting.
        /// </summary>
        void Stop();
    }
}
