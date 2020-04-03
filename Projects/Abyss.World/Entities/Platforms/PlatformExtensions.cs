namespace Abyss.World.Entities.Platforms
{
    public static class PlatformExtensions
    {
        public static IPlatformDefinition Clone(this IPlatformDefinition i)
        {
            var definition = new PlatformDefinition(i.Name, i.Position, i.Size)
                                 {
                                     SpriteLocation = i.SpriteLocation
                                 };

            foreach (var p in i.Path)
            {
                definition.Path.Add(p);
            }

            foreach (var p in i.Parameters)
            {
                definition.Parameters.Add(p);
            }

            return definition;
        }
    }
}
