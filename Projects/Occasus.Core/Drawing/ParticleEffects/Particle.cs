using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Physics;
using System.Collections.Generic;

namespace Occasus.Core.Drawing.ParticleEffects
{
    public class Particle : IParticle
    {
        protected IParticleEffect parent;
        protected int elapsedFrames;

        private readonly int totalFrames;

        private readonly Vector2 initialScale;
        private readonly float rotationSpeed;

        private readonly int fadeInFrames;
        private readonly int fadeOutFrames;

        /// <summary>
        /// Initializes a new instance of the <see cref="Particle" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="position">The initial position.</param>
        /// <param name="velocity">The initial velocity.</param>
        /// <param name="initialRotation">The initial rotation.</param>
        /// <param name="rotationSpeed">The rotation speed.</param>
        /// <param name="scale">The initial scale.</param>
        /// <param name="totalFrames">The total frames.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="recycle">if set to <c>true</c> [recycle].</param>
        /// <param name="fadeIn">if set to <c>true</c> [fade in].</param>
        /// <param name="fadeOut">if set to <c>true</c> [fade].</param>
        /// <param name="trackParent">if set to <c>true</c> [track parent].</param>
        /// <param name="shrink">if set to <c>true</c> [shrink].</param>
        public Particle(
            IParticleEffect parent,
            Vector2 position, 
            Vector2 velocity, 
            float initialRotation, 
            float rotationSpeed,
            Vector2 scale, 
            int totalFrames, 
            Rectangle sourceRectangle,
            bool recycle,
            bool fadeIn,
            bool fadeOut,
            bool trackParent,
            bool shrink)
        {
            this.parent = parent;
            this.Transform = new Transform
                                 {
                                     Position = position,
                                     Rotation = initialRotation,
                                     Scale = scale
                                 };
            this.initialScale = scale;
            this.rotationSpeed = rotationSpeed;
            this.Velocity = velocity;

            this.totalFrames = totalFrames;
            
            // Determine the number of frames to fade in and fade out.
            if (fadeIn)
            {
                this.fadeInFrames = this.totalFrames / 5;
            }

            if (fadeOut)
            {
                this.fadeOutFrames = this.totalFrames - (this.totalFrames / 2);
            }

            this.FramesRemaining = totalFrames;
            this.SourceRectangle = sourceRectangle;

            this.Flags = new Dictionary<ParticleFlag, bool>
            {
                { ParticleFlag.Recycle, recycle }, 
                { ParticleFlag.FadeIn, fadeIn },
                { ParticleFlag.FadeOut, fadeOut },
                { ParticleFlag.Shrink, shrink },
                { ParticleFlag.Rotate, rotationSpeed > 0f },
                { ParticleFlag.TrackParent, trackParent }
            };

            this.Opacity = fadeIn ? 0f : 1f;
            this.Color = Color.White;
        }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public Texture2D Texture
        {
            get; set;
        }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        public ITransform Transform
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        public Vector2 Velocity
        {
            get; set;
        }

        /// <summary>
        /// Gets the frames remaining.
        /// </summary>
        public int FramesRemaining
        {
            get; private set;
        }

        /// <summary>
        /// Gets the source rectangle.
        /// </summary>
        /// <value>
        /// The source rectangle.
        /// </value>
        public Rectangle SourceRectangle
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public Vector2 Offset
        {
            get; set;
        }

        /// <summary>
        /// Gets the flags for this particle.
        /// </summary>
        public IDictionary<ParticleFlag, bool> Flags
        {
            get; private set;
        }

        /// <summary>
        /// Gets the opacity.
        /// </summary>
        public float Opacity
        {
            get; private set;
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
        /// Updates this particle.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (this.Flags[ParticleFlag.FadeIn] && this.elapsedFrames <= this.fadeInFrames)
            {
                this.Opacity = (float)this.elapsedFrames / (float)this.fadeInFrames;
            }

            var frameOffset = ((float)this.elapsedFrames / (float) this.totalFrames);
            if (this.Flags[ParticleFlag.FadeOut] && this.elapsedFrames >= this.fadeOutFrames)
            {
                this.Opacity = 1f - frameOffset;
            }

            if (this.Flags[ParticleFlag.Shrink])
            {
                this.Transform.Scale = initialScale - (frameOffset * initialScale);
            }

            if (this.Flags[ParticleFlag.Rotate])
            {
                var newRotation = this.Transform.Rotation + this.rotationSpeed;
                var circle = MathHelper.Pi * 2;
                newRotation = newRotation % circle;

                this.Transform.Rotation = newRotation;
            }

            this.FramesRemaining--;
            this.elapsedFrames++;

            if (this.Flags[ParticleFlag.TrackParent])
            {
                this.Transform.Position = this.parent.Parent.Transform.Position + this.Offset;
            }
            else
            {
                this.Transform.Position += this.Velocity;
            }
        }
    }
}
