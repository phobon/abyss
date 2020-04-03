using System.Collections.Generic;

namespace Abyss.World.Entities.Player
{
    public static class PlayerConstants
    {
        private static Dictionary<int, int> maximumRiftValues;

        public static IDictionary<int, int> MaximumRiftValues
        {
            get
            {
                if (maximumRiftValues == null)
                {
                    maximumRiftValues = new Dictionary<int, int>
                                            {
                                                { 1, 50 }, 
                                                { 2, 100 }, 
                                                { 3, 150 }, 
                                                { 4, 200 }, 
                                                { 5, 250 }, 
                                                { 6, 300 }, 
                                                { 7, 350 }, 
                                                { 8, 400 }, 
                                                { 9, 450 }, 
                                                { 10, 500 }
                                            };
                }

                return maximumRiftValues;
            }
        }
    }
}
