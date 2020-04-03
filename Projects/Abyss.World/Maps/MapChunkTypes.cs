namespace Abyss.World.Maps
{
    public static class MapChunkTypes
    {
        // Basic types.
        public const string Normal = "basic";
        public const string Treasure = "treasure";
        public const string Empty = "empty";

        // Tutorial types.
        public const string Movement = "movement";
        public const string DimensionShift = "dimensionshift";
        public const string Key = "key";
        public const string Begin = "begin";

#if DEBUG
        public const string Moving = "moving";
        public const string Crumbling = "crumbling";
        public const string Crushing = "crushing";
        public const string Icy = "icy";
        public const string Springy = "springy";
        public const string Volatile = "volatile";
        public const string Doodads = "doodads";
        public const string Monsters = "monsters";
#endif
    }
}
