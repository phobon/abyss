using Microsoft.Xna.Framework;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public class AnimatedParticle : Particle, IAnimatedParticle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedParticle" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="position">The position.</param>
        /// <param name="velocity">The velocity.</param>
        /// <param name="initialRotation">The rotation.</param>
        /// <param name="rotationSpeed">The rotation speed.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="totalFrames">The total frames.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="frameDelay">The frame delay.</param>
        /// <param name="recycle">if set to <c>true</c> [recycle].</param>
        /// <param name="fadeIn">if set to <c>true</c> [fade in].</param>
        /// <param name="fadeOut">if set to <c>true</c> [fade].</param>
        /// <param name="trackParent">if set to <c>true</c> [track parent].</param>
        /// <param name="shrink">if set to <c>true</c> [shrink].</param>
        public AnimatedParticle(
            IParticleEffect parent,
            Vector2 position,
            Vector2 velocity, 
            float initialRotation, 
            float rotationSpeed,
            Vector2 scale, 
            int totalFrames, 
            Rectangle sourceRectangle, 
            int frameDelay,
            bool recycle, 
            bool fadeIn,
            bool fadeOut,
            bool trackParent,
            bool shrink)
            : base(parent, position, velocity, initialRotation, rotationSpeed, scale, totalFrames * frameDelay, sourceRectangle, recycle, fadeIn, fadeOut, trackParent, shrink)
        {
            this.FrameDelay = frameDelay;
        }

        /// <summary>
        /// Gets the frame delay.
        /// </summary>
        /// <value>
        /// The frame delay.
        /// </value>
        public int FrameDelay
        {
            get; private set;
        }

        /// <summary>
        /// Updates this particle.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.elapsedFrames % this.FrameDelay == 0)
            {
                // Set the new source rectangle based on the current frame.
                var rect = this.SourceRectangle;
                rect.X += this.SourceRectangle.Width;
                this.SourceRectangle = rect;
            }
        }
    }
}
