using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Atlasser
{
    public static class Colours
    {
        private static readonly Random random = new Random();

        public static IList<SolidColorBrush> SolidColorBrushes = new List<SolidColorBrush>
        {
            new SolidColorBrush(Color.FromArgb(255, 27, 161, 226)),
            new SolidColorBrush(Color.FromArgb(255, 130, 90, 43)),
            new SolidColorBrush(Color.FromArgb(255, 96, 169, 23)),
            new SolidColorBrush(Color.FromArgb(255, 0, 138, 0)),
            new SolidColorBrush(Color.FromArgb(255, 164, 196, 0)),
            new SolidColorBrush(Color.FromArgb(255, 216, 0, 115)),
            new SolidColorBrush(Color.FromArgb(255, 215, 192, 0)),
            new SolidColorBrush(Color.FromArgb(255, 250, 104, 0)),
            new SolidColorBrush(Color.FromArgb(255, 242, 91, 211)),
            new SolidColorBrush(Color.FromArgb(255, 229, 20, 0)),
            new SolidColorBrush(Color.FromArgb(255, 170, 0, 255)),
            new SolidColorBrush(Color.FromArgb(255, 102, 102, 102)),
            new SolidColorBrush(Color.FromArgb(255, 0, 171, 169)),
            new SolidColorBrush(Color.FromArgb(255, 84, 179, 227)),
            new SolidColorBrush(Color.FromArgb(255, 117, 168, 66)),
            new SolidColorBrush(Color.FromArgb(255, 250, 141, 62)),
            new SolidColorBrush(Color.FromArgb(255, 230, 72, 57))
        };

        public static SolidColorBrush GetRandomBrush()
        {
            return SolidColorBrushes[random.Next(SolidColorBrushes.Count)];
        }
    }
}
