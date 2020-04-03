using System.Linq;
using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;

namespace Occasus.Core.Drawing.Sprites
{
    public static class SpriteDataExtensions
    {
        public static ISprite ToSprite(this ISpriteData s, IEntity parent)
        {
            var sprite = new Sprite(parent, s.Origin, s.FrameSize, s.Layers, s.AnimationStates.Clone());
            sprite.Flags[SpriteFlag.Tile] = s.TileTexture;
            return sprite;
        }

        public static ISprite ToSprite(this ISpriteData s, IEntity parent, Vector2 origin, Vector2 frameSize)
        {
            var sprite = new Sprite(parent, origin, frameSize, s.Layers, s.AnimationStates.Clone());
            sprite.Flags[SpriteFlag.Tile] = s.TileTexture;
            return sprite;
        }

        public static IAnimatedParticle ToAnimatedParticle(
            this ISpriteData s, 
            IParticleEffect effect, 
            Vector2 position,
            Vector2 velocity,
            float initialRotation, 
            float rotationSpeed,
            Vector2 scale, 
            bool recycle, 
            bool fadeIn,
            bool fadeOut, 
            int frameDelay, 
            bool trackParent, 
            bool shrink)
        {
            var particle = new AnimatedParticle(effect, position, velocity, initialRotation, rotationSpeed, scale, s.AnimationStates.First().TotalFrames, s.Layers.First().SourceFrame, frameDelay, recycle, fadeIn, fadeOut, trackParent, shrink)
            {
                Texture = TextureManager.Textures["Particles"]
            };

            return particle;
        }
    }
}
