using System;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;

namespace Abysser.Presenters.Assets
{
    public interface ITexture : IAsset
    {
        Point AtlasLocation { get; }

        CroppedBitmap Texture { get; set; }
    }
}
