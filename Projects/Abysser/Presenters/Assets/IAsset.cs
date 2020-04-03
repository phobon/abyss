using Microsoft.Xna.Framework;

namespace Abysser.Presenters.Assets
{
    public interface IAsset
    {
        string Name { get; }

        string Description { get; }

        AssetType AssetType { get; }

        Vector2 Position { get; set; }
    }
}
