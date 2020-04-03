using System;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using System.Collections.Generic;

namespace Occasus.Core.Assets
{
    public static class TextureManager
    {
        private static IDictionary<string, Texture2D> textures;
        private static IDictionary<Color, Texture2D> colorTextures;

        /// <summary>
        /// Gets the textures.
        /// </summary>
        public static IDictionary<string, Texture2D> Textures
        {
            get
            {
                if (textures == null)
                {
                    textures = new Dictionary<string, Texture2D>(StringComparer.OrdinalIgnoreCase);
                }

                return textures;
            }
        }

        /// <summary>
        /// Gets the texture for a specified colour.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>A new colour texture.</returns>
        public static Texture2D GetColorTexture(Color color)
        {
            if (colorTextures == null)
            {
                colorTextures = new Dictionary<Color, Texture2D>();
            }

            if (colorTextures.ContainsKey(color))
            {
                return colorTextures[color];
            }

            // Create a new texture and add it to the heap.
            var texture = new Texture2D(DrawingManager.GraphicsDevice, 1, 1);
            texture.SetData(new[] { color });
            colorTextures.Add(color, texture);

            return texture;
        }

        public static void LoadTextureData(string uri)
        {
            var stream = TitleContainer.OpenStream(uri);
            var doc = XDocument.Load(stream);

            foreach (var t in doc.Descendants("Textures").Descendants("Texture"))
            {
                // Texture id.
                var textureId = t.Attribute("id").Value.ToLower();
                var location = t.Attribute("location").Value;

                Textures.Add(textureId, DrawingManager.ContentManager.Load<Texture2D>(location));
            }
        }
    }
}
