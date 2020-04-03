using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework;

namespace Abysser.Presenters.Assets
{
    public class TilePresenter : AssetPresenter, ITile
    {
        public TilePresenter(string name, string description, int[] atlasLocation)
            : base(name, description, AssetType.Texture, new Rectangle(0, 0, AssetConstants.TextureSize, AssetConstants.TextureSize))
        {
            this.AtlasLocation = new Point(atlasLocation[0], atlasLocation[1]);
        }

        public Point AtlasLocation
        {
            get; private set;
        }

        public CroppedBitmap Texture
        {
            get; set;
        }
    }
}
