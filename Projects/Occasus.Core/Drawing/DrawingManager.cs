using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using Occasus.Core.Camera;

namespace Occasus.Core.Drawing
{
    public static class DrawingManager
    {
        private static Rectangle tileBoundingRectangle;
        private static Point tileDimensions;

        private static Dictionary<int, Vector2> windowResolutions;
        private static int windowScale;

        private static Dictionary<string, float> entitySpriteDepths;
        
        // Data constants.
        private const string SpriteSheetKey = "SpriteSheet";
        private const string TileKey = "Tile";
        private const string SpriteDepthsKey = "SpriteDepths";
        private const string BaseResolutionKey = "BaseResolution";
        private const string ResolutionScalesKey = "Scales";
        private const string WidthKey = "Width";
        private const string HeightKey = "Height";
        private const string Key = "Key";
        private const string Value = "Value";

        /// <summary>
        /// Gets the base resolution width.
        /// </summary>
        public static float BaseResolutionWidth
        {
            get; private set;
        }

        /// <summary>
        /// Gets the base resolution height.
        /// </summary>
        public static float BaseResolutionHeight
        {
            get; private set;
        }

        /// <summary>
        /// Gets the width of the sprite sheet in tiles.
        /// </summary>
        public static int SpriteSheetWidth
        {
            get; private set;
        }

        /// <summary>
        /// Gets the height of the sprite sheet in tiles.
        /// </summary>
        public static int SpriteSheetHeight
        {
            get; private set;
        }

        /// <summary>
        /// Gets the width of a tile.
        /// </summary>
        public static int TileWidth
        {
            get; private set;
        }

        /// <summary>
        /// Gets the height of a tile.
        /// </summary>
        public static int TileHeight
        {
            get; private set;
        }

        /// <summary>
        /// Gets the screen's vertical center.
        /// </summary>
        public static int ScreenVerticalCenter
        {
            get
            {
                return ScreenHeight / 2;
            }
        }

        /// <summary>
        /// Gets the screen's horizontal center.
        /// </summary>
        public static int ScreenHorizontalCenter
        {
            get
            {
                return ScreenWidth / 2;
            }
        }

        /// <summary>
        /// Gets the window resolutions available to the game.
        /// </summary>
        public static IDictionary<int, Vector2> WindowResolutions
        {
            get
            {
                if (windowResolutions == null)
                {
                    windowResolutions = new Dictionary<int, Vector2>();
                }

                return windowResolutions;
            }
        }

        /// <summary>
        /// Gets or sets the factor to scale the window against.
        /// </summary>
        /// <value>
        /// The window scale.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Cannot set this window scale.</exception>
        public static int WindowScale
        {
            get
            {
                return windowScale;
            }

            set
            {
                if (value == windowScale)
                {
                    return;
                }

                if (!WindowResolutions.ContainsKey(value))
                {
                    throw new InvalidOperationException("Cannot set this window scale.");
                }

                windowScale = value;
            }
        }

        /// <summary>
        /// Gets the height of the scaled window.
        /// </summary>
        public static int ScaledWindowHeight
        {
            get
            {
                return (int)BaseResolutionHeight * WindowScale;
            }
        }

        /// <summary>
        /// Gets the width of the scaled window.
        /// </summary>
        public static int ScaledWindowWidth
        {
            get
            {
                return (int)BaseResolutionWidth * WindowScale;
            }
        }

        /// <summary>
        /// Gets the tile bounding rectangle.
        /// </summary>
        public static Rectangle TileBoundingRectangle
        {
            get
            {
                return tileBoundingRectangle;
            }
        }

        /// <summary>
        /// Gets the tile dimensions.
        /// </summary>
        public static Point TileDimensions
        {
            get
            {
                return tileDimensions;
            }
        }

        /// <summary>
        /// Gets the entity sprite depths.
        /// </summary>
        public static IDictionary<string, float> EntitySpriteDepths
        {
            get
            {
                if (entitySpriteDepths == null)
                {
                    entitySpriteDepths = new Dictionary<string, float>();
                }

                return entitySpriteDepths;
            }
        }

        /// <summary>
        /// Gets or sets the size of the texture in this engine instance.
        /// </summary>
        /// <value>
        /// The size of the texture.
        /// </value>
        public static float TextureSize
        {
            get; set;
        }

        /// <summary>
        /// Gets the width of the screen.
        /// </summary>
        public static int ScreenWidth
        {
            get
            {
                return GraphicsDevice.PresentationParameters.BackBufferWidth;
            }
        }

        /// <summary>
        /// Gets the height of the screen.
        /// </summary>
        public static int ScreenHeight
        {
            get
            {
                return GraphicsDevice.PresentationParameters.BackBufferHeight;
            }
        }

        /// <summary>
        /// Gets or sets the graphics device.
        /// </summary>
        /// <value>
        /// The graphics device.
        /// </value>
        public static GraphicsDevice GraphicsDevice
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the sprite batch.
        /// </summary>
        /// <value>
        /// The sprite batch.
        /// </value>
        public static SpriteBatch SpriteBatch
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the content manager.
        /// </summary>
        /// <value>
        /// The content manager.
        /// </value>
        public static ContentManager ContentManager
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the camera.
        /// </summary>
        /// <value>
        /// The camera.
        /// </value>
        public static ICamera2D Camera
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public static SpriteFont Font
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the lighting shader.
        /// </summary>
        /// <value>
        /// The lighting shader.
        /// </value>
        public static Effect LightingShader
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the lighting render target.
        /// </summary>
        /// <value>
        /// The lighting render target.
        /// </value>
        public static RenderTarget2D DeferredLightingRenderTarget
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the color map render target.
        /// </summary>
        /// <value>
        /// The color map render target.
        /// </value>
        public static RenderTarget2D ColorMapRenderTarget
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the shader render target.
        /// </summary>
        /// <value>
        /// The shader render target.
        /// </value>
        public static RenderTarget2D ShaderRenderTarget
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the scale render target.
        /// </summary>
        /// <value>
        /// The scale render target.
        /// </value>
        public static RenderTarget2D ScaleRenderTarget
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the ambient light value.
        /// </summary>
        /// <value>
        /// The ambient light value.
        /// </value>
        public static float AmbientLightValue
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the color of the ambient light.
        /// </summary>
        /// <value>
        /// The color of the ambient light.
        /// </value>
        public static Color AmbientLightColor
        {
            get; set;
        }

        /// <summary>
        /// Clones the render target.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>A new render target with the properties of the provided device.</returns>
        public static RenderTarget2D CloneRenderTarget(GraphicsDevice device)
        {
            return new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);
        }

        /// <summary>
        /// Loads the platform data.
        /// </summary>
        /// <remarks>
        /// This data includes base resolution data and any resolution scales we want to use. This is platform specific.
        /// </remarks>
        /// <param name="platformData">The platform data.</param>
        public static void LoadPlatformData(JToken platformData)
        {
            // Resolution and scale data.
            BaseResolutionWidth = (float)platformData[BaseResolutionKey][WidthKey];
            BaseResolutionHeight = (float)platformData[BaseResolutionKey][HeightKey];

            var scales = (int)platformData[ResolutionScalesKey];
            for (var i = 1; i <= scales; i++)
            {
                WindowResolutions.Add(i, new Vector2(BaseResolutionWidth * i, BaseResolutionHeight * i));
            }
        }

        /// <summary>
        /// Loads the drawing data.
        /// </summary>
        /// <param name="drawingData">The drawing data.</param>
        public static void LoadDrawingData(JToken drawingData)
        {
            // Spritesheet data.
            SpriteSheetWidth = (int)drawingData[SpriteSheetKey][WidthKey];
            SpriteSheetHeight = (int)drawingData[SpriteSheetKey][HeightKey];

            // Tile data.
            TileWidth = (int)drawingData[TileKey][WidthKey];
            TileHeight = (int)drawingData[TileKey][HeightKey];

            tileBoundingRectangle = new Rectangle(0, 0, TileWidth, TileHeight);
            tileDimensions = new Point(TileWidth, TileHeight);

            // Sprite depths.
            foreach (var depth in drawingData[SpriteDepthsKey])
            {
                EntitySpriteDepths.Add((string)depth[Key], (float)depth[Value]);
            }
        }
    }
}
