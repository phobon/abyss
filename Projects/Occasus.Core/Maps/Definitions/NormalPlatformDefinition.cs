using Microsoft.Xna.Framework;

namespace Occasus.Core.Maps.Definitions
{
    public class NormalPlatformDefinition : PlatformDefinition
    {
        public NormalPlatformDefinition(Rectangle rect) 
            : base("normal", rect, Point.Zero)
        {
        }
    }
}
