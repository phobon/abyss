using Microsoft.Xna.Framework;
using Occasus.Core.Maths;
using System.Collections;

namespace Occasus.Core.Physics
{
    /// <summary>
    /// Co-routines that handle tweening between different states. Easing functions can be applied.
    /// </summary>
    /// <remarks>
    /// While this is mostly an "animation" thing, it's useful to be in the Physics namespace as it is related to the 
    /// <see cref="ITransform"/> interface.
    /// </remarks>
    public static class Tween
    {
        public static IEnumerator EaseTo(float source, float target, int framesLeft, Easer ease)
        {
            var start = source;
            var range = target - start;

            var elapsedFrames = 0;
            while (elapsedFrames <= framesLeft)
            {
                var coefficient = (float)elapsedFrames / (float)framesLeft;
                source = start + range * ease(coefficient);

                yield return null;
                elapsedFrames++;
            }

            source = target;
        }

        public static IEnumerator ExternalForceMoveTo(this ICollider c, Vector2 target, int framesLeft, Easer ease)
        {
            var start = c.ExternalForce;
            var range = target - start;

            var elapsedFrames = 0;
            while (elapsedFrames <= framesLeft)
            {
                var coefficient = (float)elapsedFrames / (float)framesLeft;
                c.ExternalForce = start + range * ease(coefficient);

                yield return null;
                elapsedFrames++;
            }

            c.ExternalForce = target;
        }

        /// <summary>
        /// Moves to the specified target with specified duration and easing function.
        /// </summary>
        /// <param name="t">The transform.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Move to co-routine.
        /// </returns>
        public static IEnumerator MoveTo(this ITransform t, Vector2 target, int framesLeft, Easer ease)
        {
            var start = t.Position;
            var range = target - start;

            var elapsedFrames = 0;
            while (elapsedFrames <= framesLeft)
            {
                var coefficient = (float)elapsedFrames / (float)framesLeft;
                t.Position = start + (range * ease(coefficient));
                //t.Position = new Vector2((float)Math.Round(t.Position.X), (float)Math.Round(t.Position.Y));

                yield return null;
                elapsedFrames++;
            }

            t.Position = target;
        }

        /// <summary>
        /// Moves to the specified target with specified duration with a linear ease.
        /// </summary>
        /// <param name="t">The transform.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Move to co-routine.
        /// </returns>
        public static IEnumerator MoveTo(this ITransform t, Vector2 target, int framesLeft)
        {
            return t.MoveTo(target, framesLeft, Ease.QuadInOut);
        }

        /// <summary>
        /// Moves to the specified target with specified duration and easing function type.
        /// </summary>
        /// <param name="t">The transform.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Move to co-routine.
        /// </returns>
        public static IEnumerator MoveTo(this ITransform t, Vector2 target, int framesLeft, EaseType ease)
        {
            return MoveTo(t, target, framesLeft, Ease.FromType(ease));
        }

        /// <summary>
        /// Moves from the specified target with specified duration and easing function.
        /// </summary>
        /// <param name="t">The transform.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Move from co-routine.
        /// </returns>
        public static IEnumerator MoveFrom(this ITransform t, Vector2 target, int framesLeft, Easer ease)
        {
            var start = t.Position;
            t.Position = target;
            return t.MoveTo(start, framesLeft, ease);
        }

        /// <summary>
        /// Moves from the specified target with specified duration with linear ease.
        /// </summary>
        /// <param name="t">The transform.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Move from co-routine.
        /// </returns>
        public static IEnumerator MoveFrom(this ITransform t, Vector2 target, int framesLeft)
        {
            return t.MoveFrom(target, framesLeft, Ease.QuadIn);
        }

        /// <summary>
        /// Moves from the specified target with specified duration and easing function.
        /// </summary>
        /// <param name="t">The transform.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Move from co-routine.
        /// </returns>
        public static IEnumerator MoveFrom(this ITransform t, Vector2 target, int framesLeft, EaseType ease)
        {
            return t.MoveFrom(target, framesLeft, Ease.FromType(ease));
        }

        /// <summary>
        /// Scales a transform to the specified value with specified duration and easing function.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Scale to co-routine.
        /// </returns>
        public static IEnumerator ScaleTo(this ITransform t, Vector2 target, int framesLeft, Easer ease)
        {
            var start = t.Scale;
            var range = target - start;

            var elapsedFrames = 0;
            while (elapsedFrames <= framesLeft)
            {
                var coefficient = (float)elapsedFrames / (float)framesLeft;
                t.Scale = start + range * ease(coefficient);
                yield return null;

                elapsedFrames++;
            }

            t.Scale = target;
        }

        /// <summary>
        /// Scales a transform to the specified value with specified duration with a linear ease.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Scale to co-routine.
        /// </returns>
        public static IEnumerator ScaleTo(this ITransform t, Vector2 target, int framesLeft)
        {
            return t.ScaleTo(target, framesLeft, Ease.QuadIn);
        }

        /// <summary>
        /// Scales a transform to the specified value with specified duration and easing function type.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Scale to co-routine.
        /// </returns>
        public static IEnumerator ScaleTo(this ITransform t, Vector2 target, int framesLeft, EaseType ease)
        {
            return t.ScaleTo(target, framesLeft, Ease.FromType(ease));
        }

        /// <summary>
        /// Scales a transform from the specified value with specified duration and easing function.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Scale from co-routine.
        /// </returns>
        public static IEnumerator ScaleFrom(this ITransform t, Vector2 target, int framesLeft, Easer ease)
        {
            var start = t.Scale;
            t.Scale = target;
            return t.ScaleTo(start, framesLeft, ease);
        }

        /// <summary>
        /// Scales a transform from the specified value with specified duration with a linear ease.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Scale from co-routine.
        /// </returns>
        public static IEnumerator ScaleFrom(this ITransform t, Vector2 target, int framesLeft)
        {
            return t.ScaleFrom(target, framesLeft, Ease.QuadIn);
        }

        /// <summary>
        /// Scales a transform from the specified value with specified duration and easing function type.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Scale from co-routine.
        /// </returns>
        public static IEnumerator ScaleFrom(this ITransform t, Vector2 target, int framesLeft, EaseType ease)
        {
            return t.ScaleFrom(target, framesLeft, Ease.FromType(ease));
        }

        /// <summary>
        /// Rotates a transform to the specified value with specified duration and easing function.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Rotate to co-routine.
        /// </returns>
        public static IEnumerator RotateTo(this ITransform t, float target, int framesLeft, Easer ease)
        {
            var start = t.Rotation;

            var elapsedFrames = 0;
            while (elapsedFrames <= framesLeft)
            {
                var coefficient = (float)elapsedFrames / (float)framesLeft;
                t.Rotation = MathHelper.Lerp(start, target, ease(coefficient));
                yield return null;
                elapsedFrames++;
            }

            t.Rotation = target;
        }

        /// <summary>
        /// Rotates a transform to the specified value with specified duration with a linear ease.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Rotate to co-routine.
        /// </returns>
        public static IEnumerator RotateTo(this ITransform t, float target, int framesLeft)
        {
            return t.RotateTo(target, framesLeft, Ease.QuadIn);
        }

        /// <summary>
        /// Rotates a transform to the specified value with specified duration and easing function type.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Rotate to co-routine.
        /// </returns>
        public static IEnumerator RotateTo(this ITransform t, float target, int framesLeft, EaseType ease)
        {
            return t.RotateTo(target, framesLeft, Ease.FromType(ease));
        }

        /// <summary>
        /// Rotates a transform from the specified value with specified duration and easing function.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Rotate from co-routine.
        /// </returns>
        public static IEnumerator RotateFrom(this ITransform t, float target, int framesLeft, Easer ease)
        {
            var start = t.Rotation;
            t.Rotation = target;
            return t.RotateTo(start, framesLeft, ease);
        }

        /// <summary>
        /// Rotates a transform from the specified value with specified duration with a linear ease.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Rotate from co-routine.
        /// </returns>
        public static IEnumerator RotateFrom(this ITransform t, float target, int framesLeft)
        {
            return t.RotateFrom(target, framesLeft, Ease.QuadIn);
        }

        /// <summary>
        /// Rotates a transform from the specified value with specified duration and easing function type.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Rotate from co-routine.
        /// </returns>
        public static IEnumerator RotateFrom(this ITransform t, float target, int framesLeft, EaseType ease)
        {
            return t.RotateFrom(target, framesLeft, Ease.FromType(ease));
        }

        /// <summary>
        /// Curves to the specified value with specified duration and easing function.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="control">The control.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Curve to co-routine.
        /// </returns>
        public static IEnumerator CurveTo(this ITransform t, Vector2 control, Vector2 target, int framesLeft, Easer ease)
        {
            var start = t.Position;

            var elapsedFrames = 0;
            while (elapsedFrames <= framesLeft)
            {
                var z = ease((float)elapsedFrames / (float)framesLeft);
                var newPosition = Vector2.Zero;
                newPosition.X = start.X * (1 - z) * (1 - z) + control.X * 2 * (1 - z) * z + target.X * z * z;
                newPosition.Y = start.Y * (1 - z) * (1 - z) + control.Y * 2 * (1 - z) * z + target.Y * z * z;
                t.Position = newPosition;

                yield return null;
                elapsedFrames++;
            }

            t.Position = target;
        }

        /// <summary>
        /// Curves to the specified value with specified duration with a linear ease.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="control">The control.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Curve to co-routine.
        /// </returns>
        public static IEnumerator CurveTo(this ITransform t, Vector2 control, Vector2 target, int framesLeft)
        {
            return t.CurveTo(control, target, framesLeft, Ease.QuadIn);
        }

        /// <summary>
        /// Curves to the specified value with specified duration and easing function type.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="control">The control.</param>
        /// <param name="target">The target.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Curve to co-routine.
        /// </returns>
        public static IEnumerator CurveTo(this ITransform t, Vector2 control, Vector2 target, int framesLeft, EaseType ease)
        {
            return t.CurveTo(control, target, framesLeft, Ease.FromType(ease));
        }

        /// <summary>
        /// Curves from the specified value with specified duration and easing function.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="control">The control.</param>
        /// <param name="start">The start.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Curve from co-routine.
        /// </returns>
        public static IEnumerator CurveFrom(this ITransform t, Vector2 control, Vector2 start, int framesLeft, Easer ease)
        {
            var target = t.Position;
            t.Position = start;
            return t.CurveTo(control, target, framesLeft, ease);
        }

        /// <summary>
        /// Curves from the specified value with specified duration with a linear ease.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="control">The control.</param>
        /// <param name="start">The start.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Curve from co-routine.
        /// </returns>
        public static IEnumerator CurveFrom(this ITransform t, Vector2 control, Vector2 start, int framesLeft)
        {
            return t.CurveFrom(control, start, framesLeft, Ease.QuadIn);
        }

        /// <summary>
        /// Curves from the specified value with specified duration and easing function type.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="control">The control.</param>
        /// <param name="start">The start.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <param name="ease">The ease.</param>
        /// <returns>
        /// Curve from co-routine.
        /// </returns>
        public static IEnumerator CurveFrom(this ITransform t, Vector2 control, Vector2 start, int framesLeft, EaseType ease)
        {
            return t.CurveFrom(control, start, framesLeft, Ease.FromType(ease));
        }

        /// <summary>
        /// Shakes the transform for a specified amount and duration.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Shake co-routine.
        /// </returns>
        public static IEnumerator Shake(this ITransform t, Vector2 amount, int framesLeft)
        {
            var start = t.Position;

            var elapsedFrames = 0;
            while (elapsedFrames <= framesLeft)
            {
                var shake = new Vector2(MathsHelper.Random((int)-amount.X, (int)amount.X), MathsHelper.Random((int)-amount.Y, (int)amount.Y));
                t.Position = start + shake;
                yield return null;
                elapsedFrames++;
            }

            t.Position = start;
        }

        /// <summary>
        /// Shakes the transform for a specified amount and duration.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="framesLeft">The frames left.</param>
        /// <returns>
        /// Shake co-routine.
        /// </returns>
        public static IEnumerator Shake(this ITransform t, float amount, int framesLeft)
        {
            return t.Shake(new Vector2(amount, amount), framesLeft);
        }
    }
}
