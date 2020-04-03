using System;
using System.IO;
using Abyss.World;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Occasus.Core.Drawing;

namespace Abyss.Win8
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Get window resolutions first.
            var stream = TitleContainer.OpenStream("Content\\platformdata.json");
            var resolutionData = JToken.ReadFrom(new JsonTextReader(new StreamReader(stream)));
            DrawingManager.LoadPlatformData(resolutionData);

#if DEBUG
            // TODO: I don't like how this works; look at abstracting this somehow.
            DrawingManager.WindowScale = 4;
            using (var game = new GameWorld(DrawingManager.ScaledWindowWidth, DrawingManager.ScaledWindowHeight, 60))
            {
                game.Run();
            }
#else
            try
            {
                DrawingManager.WindowScale = 4;
                using (var game = new GameWorld((int)DrawingManager.ScaledWindowWidth, (int)DrawingManager.ScaledWindowHeight, 60))
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                Logger.Log(e);
                throw;
            }
#endif
        }
    }
#endif
}
