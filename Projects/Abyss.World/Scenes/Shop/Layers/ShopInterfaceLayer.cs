using Occasus.Core.Layers;
using Occasus.Core.Scenes;

namespace Abyss.World.Scenes.Shop.Layers
{
    public class ShopInterfaceLayer : Layer
    {
        public ShopInterfaceLayer(IScene parentScene)
            : base(
            parentScene, 
            "Shop Interface Layer", 
            "Interface layer for a shop.", 
            LayerType.Interface,
            2)
        {
        }
    }
}
