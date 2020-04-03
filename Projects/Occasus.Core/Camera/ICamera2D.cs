using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Entities;
using System.Collections.Generic;

using Occasus.Core.Maths;

namespace Occasus.Core.Camera
{
    public interface ICamera2D : IEngineComponent
    {
        /// <summary>
        /// Gets or sets the position of the camera
        /// </summary>
        /// <value>The position.</value>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the move speed of the camera. The camera will tween to its destination.
        /// </summary>
        /// <value>The move speed.</value>
        float MoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the camera.
        /// </summary>
        /// <value>The rotation.</value>
        float Rotation { get; set; }

        /// <summary>
        /// Gets the origin of the viewport (accounts for Scale)
        /// </summary>        
        /// <value>The origin.</value>
        Vector2 Origin { get; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        Vector2 Offset { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        float Scale { get; set; }

        /// <summary>
        /// Gets or sets the screen center.
        /// </summary>
        /// <value>
        /// The screen center.
        /// </value>
        Vector2 ScreenCenter { get; set; }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        /// <value>
        /// The transform.
        /// </value>
        Matrix Transform { get; }

        /// <summary>
        /// Gets or sets the focus.
        /// </summary>
        /// <value>
        /// The focus.
        /// </value>
        IEntity Focus { get; set; }

        /// <summary>
        /// Determines whether the target is in view given the specified position. This can be used to increase performance by not drawing objects directly in the viewport
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns><c>True</c> if the target is in view at the specified position; otherwise, <c>false</c>.</returns>
        bool IsInView(Vector2 pos, Texture2D texture);

        /// <summary>
        /// Shakes the camera view at the specified magnitude for the specified duration.
        /// </summary>
        /// <param name="magnitude">The magnitude.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="easingFunction">The easing function.</param>
        void Shake(float magnitude, float duration, Easer easingFunction);

        /// <summary>
        /// Flashes the camera for a specified time and oolour.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="color">The color.</param>
        void Flash(int duration, Color color);

        /// <summary>
        /// Fades the camera's view out.
        /// </summary>
        /// <param name="duration">The duration.</param>
        void FadeOut(int duration);

        /// <summary>
        /// Fades the camera's view in.
        /// </summary>
        /// <param name="duration">The duration.</param>
        void FadeIn(int duration);
    }
}
