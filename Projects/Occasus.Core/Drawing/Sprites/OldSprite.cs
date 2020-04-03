using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Assets;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Images;
using Occasus.Core.Entities;

namespace Occasus.Core.Drawing.Sprites
{
    /// <summary>
    /// Component that handles drawing of a sprite.
    /// </summary>
    public class OldSprite : Image, ISprite
    {
        public const string ComponentName = "Sprite";

        private const string ComponentDescription = "Graphics component that displays a texture with animation";

        private readonly IEnumerable<IAnimation> animations;

        // Drawing related.
        private bool isAnimating;

        // Sprite tiling.
        private int tileRows;
        private int tileColumns;
        private bool tileCached;

        // Sprite scrolling.
        private int textureHeightLimit;
        private float firstFrameOffset;
        private float secondFrameOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Occasus.Core.Drawing.Sprites.Sprite" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="frameSize">Size of the frame.</param>
        /// <param name="imageLayers">The image layers.</param>
        /// <param name="animations">The animations.</param>
        public OldSprite(
            IEntity parent,
            Vector2 origin,
            Vector2 frameSize,
            IEnumerable<IImageLayer> imageLayers,
            IEnumerable<IAnimation> animations)
            : base(parent, ComponentName, ComponentDescription, origin, frameSize, imageLayers)
        {
            this.animations = animations;
            this.Flags.Add(SpriteFlag.Blinking, false);
            this.Flags.Add(SpriteFlag.ScrollUp, false);
            this.Flags.Add(SpriteFlag.ScrollDown, false);
            this.Flags.Add(SpriteFlag.CropToConstraints, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Occasus.Core.Drawing.Sprites.Sprite" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="frameSize">Size of the frame.</param>
        /// <param name="imageLayer">The sprite details.</param>
        /// <param name="animations">The animation states.</param>
        public OldSprite(
            IEntity parent,
            Vector2 origin,
            Vector2 frameSize,
            IImageLayer imageLayer,
            IEnumerable<IAnimation> animations)
            : base(parent, ComponentName, ComponentDescription, origin, frameSize, imageLayer)
        {
            this.animations = animations;
            this.Flags.Add(SpriteFlag.Blinking, false);
            this.Flags.Add(SpriteFlag.ScrollUp, false);
            this.Flags.Add(SpriteFlag.ScrollDown, false);
            this.Flags.Add(SpriteFlag.CropToConstraints, false);
        }

        /// <summary>
        /// Gets the animation states.
        /// </summary>
        public IDictionary<string, IAnimation> Animations
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current animation.
        /// </summary>
        public IAnimation CurrentAnimation
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.Animations = new Dictionary<string, IAnimation>();
            foreach (var animation in this.animations)
            {
                this.Animations.Add(animation.Name, animation);
            }

            this.CurrentAnimation = this.Animations.First().Value;

            // Crop frame rectangles if required.
            if (this.Flags[SpriteFlag.CropToConstraints])
            {
                var frameWidth = (int)this.FrameSize.X;
                var frameHeight = (int)this.FrameSize.Y;
                foreach (var a in this.Animations.Values)
                {
                    var frameRect = a.FrameRectangle;

                    if (frameRect.Width > frameWidth)
                    {
                        frameRect.Width = frameWidth;
                    }

                    if (frameRect.Height > frameHeight)
                    {
                        frameRect.Height = frameHeight;
                    }

                    a.FrameRectangle = frameRect;
                }
            }

            // Set up frame offsets.
            if (this.Flags[SpriteFlag.ScrollDown])
            {
                this.textureHeightLimit = (2 * this.CurrentAnimation.FrameRectangle.Height) - 1;
                this.secondFrameOffset = this.CurrentAnimation.FrameRectangle.Height;
            }
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var layer in this.Layers.Values)
            {
                if (layer.IsVisible)
                {
                    var layerColor = layer.Color;
                    if (this.Color != Color.White)
                    {
                        // We want to tint the layer colour based on what the overriding colour is.
                        layerColor = Color.Lerp(layer.Color, this.Color, 0.8f);
                    }

                    // This isn't the best solution; but we want to be able to control each layer's opacity as well as the overall opacity.
                    var layerOpacity = this.Opacity * layer.Opacity;

                    // Map to the correct layer frame.
                    // TODO: Cache these.
                    var layerFrame = this.CurrentAnimation.FrameRectangle;
                    layerFrame.X += this.CurrentAnimation.FrameIndexes.ElementAt(this.CurrentAnimation.CurrentFrameIndex) * layerFrame.Width;

                    // Offset to the correct position on the spritemap.
                    layerFrame.X += layer.SourceFrame.X;
                    layerFrame.Y += layer.SourceFrame.Y;

                    // Tile the texture if required.
                    if (this.Flags[SpriteFlag.Tile])
                    {
                        if (!this.tileCached)
                        {
                            if (this.Parent.Collider == null)
                            {
                                throw new InvalidOperationException("Cannot tile sprite if parent's collider is null.");
                            }

                            this.tileColumns = this.Parent.Collider.BoundingBox.Width / layerFrame.Width;
                            this.tileRows = this.Parent.Collider.BoundingBox.Height / layerFrame.Height;
                            this.tileCached = true;
                        }

                        for (var x = 0; x < this.tileColumns; x++)
                        {
                            for (var y = 0; y < this.tileRows; y++)
                            {
                                var position = this.Parent.Transform.Position;
                                position.X += x * layerFrame.Width;
                                position.Y += y * layerFrame.Height;

                                spriteBatch.Draw(TextureManager.Textures[layer.TextureId], position, layerFrame, layerColor * layerOpacity, this.Parent.Transform.Rotation, this.Origin, this.Parent.Transform.Scale, this.SpriteEffects, layer.Depth);
                            }
                        }
                    }
                    else if (this.Flags[SpriteFlag.ScrollDown])
                    {
                        // Initial setup.
                        var frameHeight = layerFrame.Height;
                        var secondLayerFrame = layerFrame;

                        this.firstFrameOffset += 0.5f;
                        this.secondFrameOffset += 0.5f;
                        if (this.firstFrameOffset > this.textureHeightLimit)
                        {
                            this.firstFrameOffset = 0;
                        }

                        if (this.secondFrameOffset > this.textureHeightLimit)
                        {
                            this.secondFrameOffset = 0;
                        }

                        layerFrame.Y = frameHeight - (int)this.firstFrameOffset;
                        secondLayerFrame.Y = frameHeight - (int)this.secondFrameOffset;

                        spriteBatch.Draw(TextureManager.Textures[layer.TextureId], this.Parent.Transform.Position, layerFrame, layerColor * layerOpacity, this.Parent.Transform.Rotation, this.Origin, this.Parent.Transform.Scale, this.SpriteEffects, layer.Depth);
                        spriteBatch.Draw(TextureManager.Textures[layer.TextureId], this.Parent.Transform.Position, secondLayerFrame, layerColor * layerOpacity, this.Parent.Transform.Rotation, this.Origin, this.Parent.Transform.Scale, this.SpriteEffects, layer.Depth);
                    }
                    else if (this.Flags[SpriteFlag.ScrollUp])
                    {

                    }
                    else
                    {
                        spriteBatch.Draw(TextureManager.Textures[layer.TextureId], this.Parent.Transform.Position, layerFrame, layerColor * layerOpacity, this.Parent.Transform.Rotation, this.Origin, this.Parent.Transform.Scale, this.SpriteEffects, layer.Depth);
                    }
                }
            }
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            if (this.CurrentAnimation != null && !this.isAnimating)
            {
                this.StartAnimation();
            }
        }

        /// <summary>
        /// Transitions to a new animation state.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        public void GoToAnimation(string stateName)
        {
            if (!this.Animations.ContainsKey(stateName))
            {
                throw new InvalidOperationException("Cannot go to animation state.");
            }

            if (this.CurrentAnimation.Name == stateName)
            {
                return;
            }

            // Stop the current animation.
            if (this.CurrentAnimation != null)
            {
                // If the current state should be played in full, holding end and is still animating, queue the next animation.
                if (this.isAnimating && (this.CurrentAnimation.Flags[AnimationFlag.PlayInFull] || this.CurrentAnimation.Flags[AnimationFlag.HoldEnd]))
                {
                    CoroutineManager.Add(this.QueueAnimation(stateName));
                    return;
                }

                this.StopAnimation();
            }

            // Go to the correct animation state.
            this.CurrentAnimation = this.Animations[stateName];
            this.StartAnimation();
        }

        /// <summary>
        /// Blinks this sprite for a set amount of frames for a set duration.
        /// </summary>
        /// <param name="blinkFrames">The frame count.</param>
        /// <param name="durationFrames">The duration.</param>
        public void Blink(int blinkFrames, int durationFrames)
        {
            // If this sprite should blink, check that here.
            CoroutineManager.Add(this.Id + "_Blink", this.BlinkEffect(blinkFrames, durationFrames));
        }

        /// <summary>
        /// Gets the length of an animation in frames. Takes into account number of frames and delay between each frame.
        /// </summary>
        /// <param name="animationName">Name of the animation.</param>
        /// <returns>
        /// Length of the animation in frames.
        /// </returns>
        public int GetAnimationFrameLength(string animationName)
        {
            var animation = this.Animations[animationName];
            return animation.TotalFrames + (animation.DelayFrames * animation.TotalFrames);
        }

        public void Squash(float factor, int durationFrames)
        {
            throw new NotImplementedException();
        }

        public void Stretch(float factor, int durationFrames)
        {
            throw new NotImplementedException();
        }

        private IEnumerator BlinkEffect(int frameCount, float duration)
        {
            var elapsedFrames = 0;

            while (elapsedFrames <= duration)
            {
                // Every time the frameCount is met, switch the flag from visible to not visible.
                if (elapsedFrames % frameCount == 0)
                {
                    this.Flags[EngineFlag.Visible] = !this.Flags[EngineFlag.Visible];
                }

                yield return null;

                elapsedFrames++;
            }

            this.Flags[EngineFlag.Visible] = true;
        }

        private void StartAnimation()
        {
            this.isAnimating = true;

            if (!this.CurrentAnimation.Flags[AnimationFlag.Static])
            {
                var id = this.GetCoroutineId();
                CoroutineManager.Add(id, this.PlayAnimation());
                return;
            }

            if (this.CurrentAnimation.Flags[AnimationFlag.PlayInFull])
            {
                CoroutineManager.Add(this.GetCoroutineId(), this.PlayStaticInFull());
            }
        }

        private void StopAnimation()
        {
            this.isAnimating = false;
            CoroutineManager.Remove(this.GetCoroutineId());
        }

        private IEnumerator PlayStaticInFull()
        {
            yield return Coroutines.Pause(this.CurrentAnimation.HoldEndFrames);
            this.StopAnimation();
        }

        /// <summary>
        /// Queues a particular animation to play after the current state has completed.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <returns>Queue animation coroutine.</returns>
        private IEnumerator QueueAnimation(string stateName)
        {
            // Pause this routine while this animation state is animating.
            while (this.isAnimating)
            {
                yield return null;
            }

            this.StopAnimation();

            this.CurrentAnimation = this.Animations[stateName];
            this.StartAnimation();
        }

        /// <summary>
        /// Coroutine to play an animation state in looping, regular or in full.
        /// </summary>
        /// <returns>Play animation state coroutine.</returns>
        private IEnumerator PlayAnimation()
        {
            this.CurrentAnimation.CurrentFrameIndex = 0;
            while (this.isAnimating)
            {
                var lagNextAnimationLoop = false;
                var holdEnd = false;

                // Advance the frame index; looping or clamping as appropriate.
                if (this.CurrentAnimation.Flags[AnimationFlag.Looping])
                {
                    var newFrameIndex = this.CurrentAnimation.CurrentFrameIndex + 1;
                    if (newFrameIndex == this.CurrentAnimation.TotalFrames && this.CurrentAnimation.Flags[AnimationFlag.HoldEnd])
                    {
                        this.CurrentAnimation.CurrentFrameIndex = newFrameIndex;
                        holdEnd = true;
                    }
                    else if (newFrameIndex >= this.CurrentAnimation.TotalFrames)
                    {
                        this.CurrentAnimation.CurrentFrameIndex = 0;

                        // Ensure we handle lagging the next animation loop if applicable.
                        lagNextAnimationLoop = this.CurrentAnimation.LoopLagFrames > 0f;
                    }
                    else
                    {
                        this.CurrentAnimation.CurrentFrameIndex = newFrameIndex;
                    }
                }
                else
                {
                    this.CurrentAnimation.CurrentFrameIndex = Math.Min(this.CurrentAnimation.CurrentFrameIndex + 1, this.CurrentAnimation.TotalFrames - 1);
                    if (this.CurrentAnimation.CurrentFrameIndex == this.CurrentAnimation.TotalFrames)
                    {
                        // If this animation is set to hold its end, then we need to queue animations until it's finished.
                        if (this.CurrentAnimation.Flags[AnimationFlag.HoldEnd])
                        {
                            holdEnd = true;
                        }
                        else
                        {
                            this.isAnimating = false;
                        }
                    }
                }

                // Based on the flag, pause the animation for the correct amount of time.
                if (holdEnd)
                {
                    yield return Coroutines.Pause(this.CurrentAnimation.HoldEndFrames);

                    if (!this.CurrentAnimation.Flags[AnimationFlag.Looping])
                    {
                        this.isAnimating = false;
                    }
                }

                // If there is a loop lag time here, pause for that amount of time rather than the regular timestep.
                // This is only relevant for looping animations; but makes sense to be here.
                if (lagNextAnimationLoop)
                {
                    yield return Coroutines.Pause(this.CurrentAnimation.LoopLagFrames);
                }
                else
                {
                    // Pause for the next frame.
                    yield return Coroutines.Pause(this.CurrentAnimation.DelayFrames);
                }
            }
        }



        private string GetCoroutineId()
        {
            return this.Parent.Id + "_Animation_" + this.CurrentAnimation.Name;
        }
    }
}
