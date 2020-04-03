using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Assets;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Images;
using Occasus.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Occasus.Core.Drawing.Sprites
{
    /// <summary>
    /// Component that handles drawing of a sprite.
    /// </summary>
    public class Sprite : Image, ISprite
    {
        public const string Tag = "Sprite";

        private const string ComponentDescription = "Graphics component that displays a texture with animation";

        private readonly IEnumerable<IAnimation> animations;
        private readonly string blinkKey;
        private readonly string squashKey;

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

        private int holdAnimationFrames;
        private int loopLagFrames;
        private int delayFrames;

        private bool isHoldingAnimation;
        private bool isLaggingLoopAnimation;

        // Squash and stretch.
        private float squashFactor = 1f;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="frameSize">Size of the frame.</param>
        /// <param name="imageLayers">The image layers.</param>
        /// <param name="animations">The animations.</param>
        public Sprite(
            IEntity parent,
            Vector2 origin,
            Vector2 frameSize,
            IEnumerable<IImageLayer> imageLayers,
            IEnumerable<IAnimation> animations)
            : base(parent, Tag, ComponentDescription, origin, frameSize, imageLayers)
        {
            this.animations = animations;
            this.Flags.Add(SpriteFlag.Blinking, false);
            this.Flags.Add(SpriteFlag.ScrollUp, false);
            this.Flags.Add(SpriteFlag.ScrollDown, false);
            this.Flags.Add(SpriteFlag.CropToConstraints, false);

            this.blinkKey = this.Id + "_Blink";
            this.squashKey = this.Id + "_Squash";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="frameSize">Size of the frame.</param>
        /// <param name="imageLayer">The sprite details.</param>
        /// <param name="animations">The animation states.</param>
        public Sprite(
            IEntity parent,
            Vector2 origin,
            Vector2 frameSize,
            IImageLayer imageLayer,
            IEnumerable<IAnimation> animations)
            : base(parent, Tag, ComponentDescription, origin, frameSize, imageLayer)
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
            get; private set;
        }

        /// <summary>
        /// Gets the current animation.
        /// </summary>
        public IAnimation CurrentAnimation
        {
            get; private set;
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.Animations = new Dictionary<string, IAnimation>(StringComparer.InvariantCultureIgnoreCase);
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
        
        private void ProcessFrame()
        {
            // Back out if we're not animating.
            if (!this.isAnimating)
            {
                return;
            }

            if (this.CurrentAnimation.TotalFrames == 1)
            {
                this.isAnimating = false;
                return;
            }

            // Based on the flag, pause the animation for the correct amount of time.
            if (this.isHoldingAnimation)
            {
                if (this.holdAnimationFrames < this.CurrentAnimation.HoldEndFrames)
                {
                    this.holdAnimationFrames++;
                    return;
                }

                this.holdAnimationFrames = 0;
                this.isHoldingAnimation = false;

                if (!this.CurrentAnimation.Flags[AnimationFlag.Looping])
                {
                    this.isAnimating = false;
                }
                else
                {
                    this.CurrentAnimation.CurrentFrameIndex = 0;
                }

                return;
            }

            if (this.isLaggingLoopAnimation)
            {
                // If there is a loop lag time here, pause for that amount of time rather than the regular timestep.
                // This is only relevant for looping animations.
                if (this.loopLagFrames < this.CurrentAnimation.LoopLagFrames)
                {
                    this.loopLagFrames++;
                    return;
                }

                this.loopLagFrames = 0;
                this.isLaggingLoopAnimation = false;
                return;
            }

            if (this.CurrentAnimation.DelayFrames > 0)
            {
                if (this.delayFrames < this.CurrentAnimation.DelayFrames)
                {
                    this.delayFrames++;
                    return;
                }

                this.delayFrames = 0;
            }

            // Advance the frame index; looping or clamping as appropriate.
            if (this.CurrentAnimation.Flags[AnimationFlag.Looping])
            {
                var newFrameIndex = this.CurrentAnimation.CurrentFrameIndex + 1;
                if (newFrameIndex >= this.CurrentAnimation.TotalFrames)
                {
                    if (this.CurrentAnimation.Flags[AnimationFlag.HoldEnd])
                    {
                        this.isHoldingAnimation = true;
                    }
                    else
                    {
                        this.CurrentAnimation.CurrentFrameIndex = 0;
                        this.isLaggingLoopAnimation = this.CurrentAnimation.LoopLagFrames > 0f;
                    }
                }
                else
                {
                    this.CurrentAnimation.CurrentFrameIndex = newFrameIndex;
                }
            }
            else
            {
                var newFrameIndex = this.CurrentAnimation.CurrentFrameIndex + 1;
                if (newFrameIndex >= this.CurrentAnimation.TotalFrames)
                {
                    // If this animation is set to hold its end, then we need to queue animations until it's finished.
                    if (this.CurrentAnimation.Flags[AnimationFlag.HoldEnd])
                    {
                        this.isHoldingAnimation = true;
                    }
                    else
                    {
                        this.isAnimating = false;
                    }
                }
                else
                {
                    this.CurrentAnimation.CurrentFrameIndex = newFrameIndex;
                }
            }
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // We process the current frame here, because we don't want to update the frame multiple times if the game is running slowly.
            // This is a trait of XNA where if we run with a fixed timestep, the game will make sure updates are called and skip draws if the game needs to catch up.
            // I'm not sure if this is the right way to do things, but we'll see if it works and doesn't look like garbage (like the alternative)
            this.ProcessFrame();

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
                    var layerFrame = this.CurrentAnimation.CurrentFrame;
                    
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
                        // TODO: These need to be cached at some point.
                        var frameHeight = layerFrame.Height;
                        var secondLayerFrame = layerFrame;
                        if (frameHeight != this.CurrentAnimation.FrameRectangle.Height)
                        {
                            frameHeight = this.CurrentAnimation.FrameRectangle.Height;
                            layerFrame.Height = frameHeight;
                            secondLayerFrame.Height = frameHeight;
                        }

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
            CoroutineManager.Add(blinkKey, this.BlinkEffect(blinkFrames, durationFrames));
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
            CoroutineManager.Add(squashKey, this.SquashEffect(factor, durationFrames));
        }

        public void Stretch(float factor, int durationFrames)
        {
        }

        public IEnumerator SquashEffect(float factor, float duration)
        {
            yield return null;
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
            this.CurrentAnimation.CurrentFrameIndex = 0;
        }

        private void StopAnimation()
        {
            this.isAnimating = false;
        }
    }
}
