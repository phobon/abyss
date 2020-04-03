using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Assets;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using System.Collections;
using Occasus.Core.Input;

namespace Occasus.Core.Camera
{
    public class Camera2D : EngineComponent, ICamera2D
    {
        private const string CameraFlashKey = "CameraFlash";

        private readonly float viewportHeight;
        private readonly float viewportWidth;

        private Vector2 position;
        private IEntity focus;

        // Camera shake.
        private bool isShaking;
        private Vector2 shakeOffset;

        // Camera effects.
        private Texture2D overlayColorTexture;
        private readonly Rectangle overlayRect;
        private float overlayOpacity;
        private bool drawOverlay;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera2D"/> class.
        /// </summary>
        /// <param name="viewPortWidth">Width of the view port.</param>
        /// <param name="viewPortHeight">Height of the view port.</param>
        public Camera2D(float viewPortWidth, float viewPortHeight)
            : base("Camera2D", "Implementation of a 2D Camera")
        {
            this.viewportWidth = viewPortWidth;
            this.viewportHeight = viewPortHeight;

            // The overlay rectangle must be the size of the scaled window width otherwise this effect won't work. This is a bit of a fail in the architecture of this game, but we live on.
            this.overlayRect = new Rectangle(0, 0, DrawingManager.ScaledWindowWidth, DrawingManager.ScaledWindowHeight);

            this.Flags.Add(CameraFlag.LockFocusHorizontal, false);
            this.Flags.Add(CameraFlag.LockFocusVertical, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera2D"/> class.
        /// </summary>
        /// <param name="focus">The focus.</param>
        /// <param name="viewPortWidth">Width of the view port.</param>
        /// <param name="viewPortHeight">Height of the view port.</param>
        public Camera2D(IEntity focus, float viewPortWidth, float viewPortHeight)
            : this(viewPortWidth, viewPortHeight)
        {
            this.Focus = focus;
        }

        /// <summary>
        /// Gets or sets the position of the camera
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the camera.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public float Rotation
        {
            get; set;
        }

        /// <summary>
        /// Gets the origin of the viewport (accounts for Scale)
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        public Vector2 Origin
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public float Scale
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the screen center.
        /// </summary>
        /// <value>
        /// The screen center.
        /// </value>
        public Vector2 ScreenCenter
        {
            get; set;
        }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        /// <value>
        /// The transform.
        /// </value>
        public Matrix Transform
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the focus.
        /// </summary>
        /// <value>
        /// The focus.
        /// </value>
        public IEntity Focus
        {
            get
            {
                return this.focus;
            }

            set
            {
                if (this.focus == value)
                {
                    return;
                }

                this.focus = value;
                
                // Set flags based on the focus object state. 
                // These can be set independently, but it makes sense to get this for free when a new focus is set.
                if (this.focus == null)
                {
                    this.Flags[CameraFlag.LockFocusHorizontal] = false;
                    this.Flags[CameraFlag.LockFocusVertical] = false;
                }
                else
                {
                    this.Flags[CameraFlag.LockFocusHorizontal] = true;
                    this.Flags[CameraFlag.LockFocusVertical] = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the move speed of the camera. The camera will tween to its destination.
        /// </summary>
        /// <value>
        /// The move speed.
        /// </value>
        public float MoveSpeed
        {
            get; set;
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
        /// Initializes this camera.
        /// </summary>
        public override void Initialize()
        {
            this.ScreenCenter = new Vector2(this.viewportWidth / 2, this.viewportHeight / 2);
            this.Scale = 1;
            this.MoveSpeed = 10f;
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            // Check for lock to focus.
            if (this.Flags[CameraFlag.LockFocusHorizontal] && this.Focus != null)
            {
                this.Position = new Vector2(this.Focus.Transform.Position.X, this.Position.Y);
            }

            if (this.Flags[CameraFlag.LockFocusVertical] && this.Focus != null)
            {
                this.Position = new Vector2(this.Position.X, this.Focus.Transform.Position.Y);
            }

            this.Origin = (this.ScreenCenter + this.Offset) / this.Scale;

            // Create the Transform used by any spritebatch process.
            var originX = this.Flags[CameraFlag.LockFocusHorizontal] ? this.Origin.X : 0f;
            var originY = this.Flags[CameraFlag.LockFocusVertical] ? this.Origin.Y : 0f;

            // If the camera is shaking, multiple the origin by the shake offset.
            if (this.isShaking)
            {
                originX += this.shakeOffset.X;
                originY += this.shakeOffset.Y;
            }

            this.Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-this.Position.X, -this.Position.Y, 0) *
                        Matrix.CreateRotationZ(this.Rotation) *
                        Matrix.CreateTranslation(originX, originY, 0) *
                        Matrix.CreateScale(this.Scale);

            // Move the Camera to the position that it needs to go.
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.Focus != null)
            {
                var focusOffset = this.Focus.Transform.Position - this.Offset - this.Position;
                if (this.Flags[CameraFlag.LockFocusHorizontal])
                {
                    this.position.X += (int)(focusOffset.X * this.MoveSpeed * delta);
                }

                if (this.Flags[CameraFlag.LockFocusVertical])
                {
                    this.position.Y += (int)(focusOffset.Y * this.MoveSpeed * delta);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.drawOverlay)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                spriteBatch.Draw(overlayColorTexture, Vector2.Zero, this.overlayRect, Color.White * this.overlayOpacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Determines whether the target is in view given the specified position. This can be used to increase performance by not drawing objects directly in the viewport
        /// </summary>
        /// <param name="p">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns><c>True</c> if [is in view] [the specified position]; otherwise, <c>false</c>.</returns>
        public bool IsInView(Vector2 p, Texture2D texture)
        {
            // If the object is not within the horizontal bounds of the screen.
            if ((p.X + texture.Width) < (this.Position.X - this.Origin.X) || p.X > (this.Position.X + this.Origin.X))
            {
                return false;
            }

            // If the object is not within the vertical bounds of the screen.
            if ((p.Y + texture.Height) < (this.Position.Y - this.Origin.Y) || p.Y > (this.Position.Y + this.Origin.Y))
            {
                return false;
            }

            // Position is in view.
            return true;
        }

        /// <summary>
        /// Shakes the camera view at the specified magnitude for the specified duration.
        /// </summary>
        /// <param name="magnitude">The magnitude.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="easingFunction">The easing function.</param>
        public void Shake(float magnitude, float duration, Easer easingFunction)
        {
            if (!this.isShaking)
            {
                CoroutineManager.Add(this.ShakeCamera(magnitude, duration, easingFunction));
            }
        }

        /// <summary>
        /// Flashes the camera for a specified time and oolour.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="color">The color.</param>
        public void Flash(int duration, Color color)
        {
            CoroutineManager.Add(CameraFlashKey, this.FlashEffect(duration, color));
        }

        public void FadeOut(int duration)
        {
            CoroutineManager.Add(this.FadeOutEffect(duration, 0f, 1f));
        }

        public void FadeIn(int duration)
        {
            CoroutineManager.Add(this.FadeInEffect(duration, 1f, 0f));
        }

        private IEnumerator FadeOutEffect(int duration, float startingValue, float endingValue)
        {
            this.overlayColorTexture = TextureManager.GetColorTexture(Color.Black);
            this.drawOverlay = true;
            this.overlayOpacity = startingValue;

            var elapsedFrames = 0;
            while (elapsedFrames <= duration)
            {
                this.overlayOpacity = startingValue + ((float)elapsedFrames / (float)duration);
                elapsedFrames++;
                yield return null;
            }

            this.overlayOpacity = endingValue;
        }

        private IEnumerator FadeInEffect(int duration, float startingValue, float endingValue)
        {
            this.overlayColorTexture = TextureManager.GetColorTexture(Color.Black);
            this.overlayOpacity = startingValue;

            var elapsedFrames = 0;
            while (elapsedFrames <= duration)
            {
                this.overlayOpacity = startingValue - ((float)elapsedFrames / (float)duration);
                elapsedFrames++;
                yield return null;
            }

            this.overlayOpacity = endingValue;
            this.drawOverlay = false;
        }

        private IEnumerator FlashEffect(int duration, Color color)
        {
            this.overlayColorTexture = TextureManager.GetColorTexture(color);
            this.drawOverlay = true;
            this.overlayOpacity = 0.3f;

            var elapsedFrames = 0;
            while (elapsedFrames <= duration)
            {
                this.overlayOpacity = 0.3f - ((float)elapsedFrames / (float)duration);
                elapsedFrames++;
                yield return null;
            }

            this.drawOverlay = false;
        }

        private IEnumerator ShakeCamera(float magnitude, float duration, Easer easingFunction)
        {
            this.isShaking = true;

            var elapsedFrames = 0;
            var totalFrames = duration / Engine.DeltaTime;

            while (elapsedFrames <= totalFrames)
            {
                // Compute the magnitude and offset based on a linear fall-off.
                // We want the offset to be rounded off to whole numbers so that textures on the screen render properly. Artefacting can occur when we use floats as render offsets.
                var shakeProgress = elapsedFrames / totalFrames;
                magnitude = magnitude * (1 - easingFunction(shakeProgress));
                var shakeX = (int)(MathsHelper.NextFloat() * magnitude);
                var shakeY = (int)(MathsHelper.NextFloat() * magnitude);
                this.shakeOffset = new Vector2(shakeX, shakeY);
                yield return null;

                elapsedFrames++;
            }

            // Reset variables.
            this.isShaking = false;
            this.shakeOffset = Vector2.Zero;
        }
    }
}
