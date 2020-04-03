using System;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Player
{
    public delegate void DimensionShiftedEventHandler(DimensionShiftedEventArgs e);

    public delegate void StatChangedEventHandler(StatChangedEventArgs e);

    public delegate void PlatformStompedEventHandler(PlatformStompedEventArgs e);

    public class DimensionShiftedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionShiftedEventArgs"/> class.
        /// </summary>
        /// <param name="newDimension">The new dimension.</param>
        public DimensionShiftedEventArgs(Dimension newDimension)
        {
            this.NewDimension = newDimension;
        }

        /// <summary>
        /// Gets the new dimension.
        /// </summary>
        public Dimension NewDimension
        {
            get; private set;
        }
    }

    public class StatChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newLife">The new life.</param>
        public StatChangedEventArgs(int newLife)
        {
            this.NewLife = newLife;
        }

        /// <summary>
        /// Gets the new life.
        /// </summary>
        public int NewLife
        {
            get; private set;
        }
    }

    public class PlatformStompedEventArgs : EventArgs
    {
        public PlatformStompedEventArgs(Rectangle platform)
        {
            this.Platform = platform;
        }

        /// <summary>
        /// Gets the new life.
        /// </summary>
        public Rectangle Platform
        {
            get; private set;
        }
    }
}
