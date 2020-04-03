using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Abyss.World.Universe
{
    public static class UniverseConstants
    {
        /// <summary>
        /// The base ambient colour.
        /// </summary>
        public static Color AmbientColor = new Color(255, 245, 204);

        /// <summary>
        /// Specific plane colours.
        /// </summary>
        public static Color ArgusColor = new Color(59, 72, 172);
        public static Color DioninColor = new Color(250, 248, 58);
        public static Color PhobonColor = new Color(221, 58, 250);
        public static Color ValusColor = new Color(255, 18, 18);

        /// <summary>
        /// The base colour for Neutral.
        /// </summary>
        public static Color NeutralColor = new Color(234, 6, 254);

        /// <summary>
        /// The base colour for the Limbo dimension.
        /// </summary>
        public static Color LimboDimensionColor = new Color(0, 144, 227);

        /// <summary>
        /// The base colour for the Normal dimension.
        /// </summary>
        public static Color NormalDimensionColor = new Color(227, 0, 0);

        /// <summary>
        /// The Speedrun seed.
        /// </summary>
        /// <remarks>
        /// This seed is static and should produce an identical level every time it is played. This can be run as many times as desired and
        /// will eventually be part of a leaderboard.
        /// </remarks>
        public const int SpeedrunSeed = 903175778;

        /// <summary>
        /// Gets the daily seed.
        /// </summary>
        /// <remarks>
        /// This seed cycles every day. Players can only attempt this level once per day.
        /// </remarks>
        public static int DailySeed
        {
            get
            {
                return (int)DateTime.Today.Ticks;
            }
        }

        /// <summary>
        /// The player positions.
        /// </summary>
        public static float[] PlayerPositions = { 0f, 16f, 32f, 48f, 64f, 80f, 96f, 112f, 128f, 144f };

        public const int PlayerPositionLowConstraint = 0;
        public const int PlayerPositionHighConstraint = 9;
    }
}
