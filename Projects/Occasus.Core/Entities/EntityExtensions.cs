using System.Collections.Generic;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Maths;
using System.Collections;
using System.Linq;

namespace Occasus.Core.Entities
{
    public static class EntityExtensions
    {
        public static IEnumerable<ISprite> GetSprites(this IEntity e)
        {
            var sprites = new List<ISprite>();
            foreach (var s in e.Components.Values)
            {
                var sprite = s as ISprite;
                if (sprite != null)
                {
                    sprites.Add(sprite);
                }
            }

            return sprites;
        }

        public static ISprite GetSprite(this IEntity e)
        {
            if (e.Components.ContainsKey(Sprite.Tag))
            {
                return (ISprite)e.Components[Sprite.Tag];
            }

            return null;
        }

        public static IEnumerator SetOpacity(this IEntity e, float target, int duration, Easer ease)
        {
            var sprites = e.Components.Values.OfType<ISprite>().ToList();

            foreach (var s in sprites)
            {
                var start = s.Opacity;
                var range = target - start;

                var elapsedFrames = 0;
                while (elapsedFrames <= duration)
                {
                    s.Opacity = start + (range * ease((float)elapsedFrames / (float)duration));
                    yield return null;

                    elapsedFrames++;
                }

                s.Opacity = target;
            }
        }
    }
}
