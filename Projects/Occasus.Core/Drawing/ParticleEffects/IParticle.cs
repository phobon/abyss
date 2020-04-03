using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Physics;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public interface IParticle
    {
        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        Texture2D Texture { get; set; }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        ITransform Transform { get; }

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        Vector2 Velocity { get; set; }

        /// <summary>
        /// Gets the frames remaining.
        /// </summary>
        /// <value>
        /// The frames remaining.
        /// </value>
        int FramesRemaining { get; }

        /// <summary>
        /// Gets the source rectangle.
        /// </summary>
        /// <value>
        /// The source rectangle.
        /// </value>
        Rectangle SourceRectangle { get; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        Vector2 Offset { get; set; }

        /// <summary>
        /// Gets the flags for this particle.
        /// </summary>
        IDictionary<ParticleFlag, bool> Flags { get; }

        /// <summary>
        /// Gets the opacity.
        /// </summary>
        float Opacity { get; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        Color Color { get; set; }

        /// <summary>
        /// Updates this particle.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        void Update(GameTime gameTime);
    }
}
