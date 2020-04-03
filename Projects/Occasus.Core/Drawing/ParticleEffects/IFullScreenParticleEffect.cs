using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using System.Collections.Generic;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public interface IFullScreenParticleEffect : IEntity
    {
        /// <summary>
        /// Gets the particle density.
        /// </summary>
        ParticleDensity ParticleDensity { get; }

        /// <summary>
        /// Gets the particle cloud.
        /// </summary>
        IEnumerable<IParticle> ParticleCloud { get; }

        /// <summary>
        /// Updates the particle cache.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        void UpdateParticleCache(Rectangle viewPort);
    }
}
