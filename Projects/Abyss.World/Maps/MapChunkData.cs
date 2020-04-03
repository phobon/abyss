namespace Abyss.World.Maps
{
    public static class MapChunkData
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

        // Layout types.
        public const string NormalPlatforms = "platforms";
        public const string SpecialPlatforms = "specialPlatforms";

        // Spawn types.
        public const string MonsterSpawns = "monsterSpawns";
        public const string ItemSpawns = "itemSpawns";
        public const string PropSpawns = "propSpawns";
        public const string TriggerSpawns = "triggerSpawns";
        public const string HazardSpawns = "hazardSpawns";

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
