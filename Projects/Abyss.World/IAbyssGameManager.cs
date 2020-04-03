using Abyss.World.Entities.Player;
using Abyss.World.Entities.Relics;
using Abyss.World.Scoring;
using Abyss.World.Universe;
using Occasus.Core;

namespace Abyss.World
{
    public interface IAbyssGameManager
    {
        /// <summary>
        /// Occurs when the dimension shifts.
        /// </summary>
        event DimensionShiftedEventHandler DimensionShifted;

        /// <summary>
        /// Gets or sets the game mode.
        /// </summary>
        GameMode GameMode { get; set; }

        /// <summary>
        /// Gets or sets the current zone.
        /// </summary>
        ZoneType CurrentZone { get; set; }

        /// <summary>
        /// Gets or sets the current difficulty.
        /// </summary>
        int CurrentDifficulty { get; set; }

        /// <summary>
        /// Gets or sets the current depth.
        /// </summary>
        int CurrentDepth { get; set; }

        /// <summary>
        /// Gets or sets the current dimension.
        /// </summary>
        Dimension CurrentDimension { get; set; }

        /// <summary>
        /// Gets the relics that have been collected.
        /// </summary>
        IRelicCollection RelicCollection { get; set; }

        /// <summary>
        /// Gets or sets the statistic manager.
        /// </summary>
        IStatisticManager StatisticManager { get; set; }

        /// <summary>
        /// Lightses the out.
        /// </summary>
        void LightsOut();

        /// <summary>
        /// Lightses the on.
        /// </summary>
        void LightsOn();
    }
}
