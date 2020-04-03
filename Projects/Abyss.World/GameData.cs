using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core.Drawing;
using Occasus.Core.Physics;

namespace Abyss.World
{
    public static class GameData
    {
        public const string VersionNumber = "1";

        private static readonly Vector2 playerStart = new Vector2(UniverseConstants.PlayerPositions[3], 0f);

        public static Vector2 PlayerStart
        {
            get
            {
                return playerStart;
            }
        }
    }
}
