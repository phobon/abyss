#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Abyss.World;

using Occasus.Core;
using Occasus.Core.Drawing;

#endregion

namespace Abyss.Win7
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
