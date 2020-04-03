using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using System.Collections.Generic;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public interface IInterfaceParticleEffect : IEntity
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the particle density.
        /// </summary>
        ParticleDensity ParticleDensity { get; }

        /// <summary>
        /// Gets the particle cloud.
        /// </summary>
        IEnumerable<IParticle> ParticleCloud { get; }
        
        /// <summary>
        /// Emits this particle effect.
        /// </summary>
        void Emit();

        /// <summary>
        /// Emits this particle effect for a specified duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        void Emit(float duration);
    }
}
