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
        public static float BaseResolutionWidth { get; private set; }

        /// <summary>
        /// Gets the base resolution height.
        /// </summary>
        public static float BaseResolutionHeight { get; private set; }

        /// <summary>
        /// Gets the width of the sprite sheet in tiles.
        /// </summary>
        public static int SpriteSheetWidth { get; private set; }

        /// <summary>
        /// Gets the height of the sprite sheet in tiles.
        /// </summary>
        public static int SpriteSheetHeight { get; private set; }

        /// <summary>
        /// Gets the width of a tile.
        /// </summary>
        public static int TileWidth { get; private set; }

        /// <summary>
        /// Gets the height of a tile.
        /// </summary>
        public static int TileHeight { get; private set; }

        /// <summary>
        /// Gets the screen's vertical center.
        /// </summary>
        public static int ScreenVerticalCenter => ScreenHeight / 2;

        /// <summary>
        /// Gets the screen's horizontal center.
        /// </summary>
        public static int ScreenHorizontalCenter => ScreenWidth / 2;

        /// <summary>
        /// Gets the window resolutions available to the game.
        /// </summary>
        public static IDictionary<int, Vector2> WindowResolutions => windowResolutions ?? (windowResolutions = new Dictionary<int, Vector2>());

        /// <summary>
        /// Gets or sets the factor to scale the window against.
        /// </summary>
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
        public static int ScaledWindowHeight => (int)BaseResolutionHeight * WindowScale;

        /// <summary>
        /// Gets the width of the scaled window.
        /// </summary>
        public static int ScaledWindowWidth => (int)BaseResolutionWidth * WindowScale;

        /// <summary>
        /// Gets the tile bounding rectangle.
        /// </summary>
        public static Rectangle TileBoundingRectangle => tileBoundingRectangle;

        /// <summary>
        /// Gets the tile dimensions.
        /// </summary>
        public static Point TileDimensions => tileDimensions;

        /// <summary>
        /// Gets the entity sprite depths.
        /// </summary>
        public static IDictionary<string, float> EntitySpriteDepths => entitySpriteDepths ?? (entitySpriteDepths = new Dictionary<string, float>());

        /// <summary>
        /// Gets or sets the size of the texture in this engine instance.
        /// </summary>
        public static float TextureSize { get; set; }

        /// <summary>
        /// Gets the width of the screen.
        /// </summary>
        public static int ScreenWidth => GraphicsDevice.PresentationParameters.BackBufferWidth;

        /// <summary>
        /// Gets the height of the screen.
        /// </summary>
        public static int ScreenHeight => GraphicsDevice.PresentationParameters.BackBufferHeight;

        /// <summary>
        /// Gets or sets the graphics device.
        /// </summary>
        public static GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>
        /// Gets or sets the sprite batch.
        /// </summary>
        public static SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Gets or sets the content manager.
        /// </summary>
        public static ContentManager ContentManager { get; set; }

        /// <summary>
        /// Gets or sets the camera.
        /// </summary>
        public static ICamera2D Camera { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public static SpriteFont Font { get; set; }

        /// <summary>
        /// Gets or sets the lighting shader.
        /// </summary>
        public static Effect LightingShader { get; set; }

        /// <summary>
        /// Gets or sets the lighting render target.
        /// </summary>
        public static RenderTarget2D DeferredLightingRenderTarget { get; set; }

        /// <summary>
        /// Gets or sets the color map render target.
        /// </summary>
        public static RenderTarget2D ColorMapRenderTarget { get; set; }

        /// <summary>
        /// Gets or sets the shader render target.
        /// </summary>
        public static RenderTarget2D ShaderRenderTarget { get; set; }

        /// <summary>
        /// Gets or sets the scale render target.
        /// </summary>
        public static RenderTarget2D ScaleRenderTarget { get; set; }

        /// <summary>
        /// Gets or sets the ambient light value.
        /// </summary>
        public static float AmbientLightValue { get; set; }

        /// <summary>
        /// Gets or sets the color of the ambient light.
        /// </summary>
        public static Color AmbientLightColor { get; set; }

        /// <summary>
        /// Clones the render target.
        /// </summary>
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
