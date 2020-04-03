using System.Collections.Generic;
using Abyss.World.Universe;

namespace Abyss.World.Maps
{
    public static class MapData
    {
        public const int ChunkWidth = 10;
        public const int ChunkHeight = 17;

        private static List<ZoneType> zoneTypes;

        private static IDictionary<ZoneType, int> maximumKeysPerLevel;
        private static IDictionary<ZoneType, int> maximumShopsPerLevel;
        private static IDictionary<ZoneType, int> maximumWarpsPerLevel;

        public static IEnumerable<ZoneType> ZoneTypes
        {
            get
            {
                if (zoneTypes == null)
                {
                    zoneTypes = new List<ZoneType>
                        {
                            ZoneType.Normal,
                            ZoneType.Electric,
                            ZoneType.Fire,
                            ZoneType.Ice,
                            ZoneType.Shadow,
                            ZoneType.Void
                        };
                }

                return zoneTypes;
            }
        }

        public static IDictionary<ZoneType, int> MaximumKeysPerLevel
        {
            get
            {
                if (maximumKeysPerLevel == null)
                {
                    maximumKeysPerLevel = new Dictionary<ZoneType, int>
                        {
                            { ZoneType.Normal, 3 },
                            { ZoneType.Electric, 3 },
                            { ZoneType.Fire, 3 },
                            { ZoneType.Ice, 3 },
                            { ZoneType.Shadow, 3 },
                            { ZoneType.Void, 3 }
                        };
                }

                return maximumKeysPerLevel;
            }
        }

        public static IDictionary<ZoneType, int> MaximumShopsPerLevel
        {
            get
            {
                if (maximumShopsPerLevel == null)
                {
                    maximumShopsPerLevel = new Dictionary<ZoneType, int>
                        {
                            { ZoneType.Normal, 2 },
                            { ZoneType.Electric, 2 },
                            { ZoneType.Fire, 2 },
                            { ZoneType.Ice, 2 },
                            { ZoneType.Shadow, 2 },
                            { ZoneType.Void, 2 }
                        };
                }

                return maximumShopsPerLevel;
            }
        }

        public static IDictionary<ZoneType, int> MaximumWarpsPerLevel
        {
            get
            {
                if (maximumWarpsPerLevel == null)
                {
                    maximumWarpsPerLevel = new Dictionary<ZoneType, int>
                        {
                            { ZoneType.Normal, 1 },
                            { ZoneType.Electric, 1 },
                            { ZoneType.Fire, 1 },
                            { ZoneType.Ice, 1 },
                            { ZoneType.Shadow, 1 },
                            { ZoneType.Void, 1 }
                        };
                }

                return maximumWarpsPerLevel;
            }
        }
    }
}
