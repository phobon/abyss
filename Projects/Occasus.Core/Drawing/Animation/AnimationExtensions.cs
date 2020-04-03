using System.Collections.Generic;
using System.Linq;

namespace Occasus.Core.Drawing.Animation
{
    public static class AnimationExtensions
    {
        public static IAnimation ToAnimation(this IAnimation a)
        {
            return new Animation(
                a.Name, 
                a.SpriteMapLocation, 
                a.FrameRectangle,
                a.FrameIndexes.ToList(), 
                a.DelayFrames, 
                a.Flags[AnimationFlag.PlayInFull],
                a.Flags[AnimationFlag.Looping])
                {
                    LoopLagFrames = a.LoopLagFrames,
                    HoldEndFrames = a.HoldEndFrames
                };
        }

        public static IList<IAnimation> Clone(this IEnumerable<IAnimation> animations)
        {
            var newAnimations = new List<IAnimation>();
            foreach (var a in animations)
            {
                newAnimations.Add(a.ToAnimation());
            }

            return newAnimations;
        }
    }
}
