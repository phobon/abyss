using Microsoft.Xna.Framework;

namespace Abysser.Presenters.Assets
{
    public class TriggerPresenter : AssetPresenter, ITrigger
    {
        public TriggerPresenter(string name, string description, Rectangle boundingRectangle) 
            : base(name, description, AssetType.Trigger, boundingRectangle)
        {
        }
    }
}
