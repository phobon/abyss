using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Drawing.ParticleEffects.Concrete;
using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Occasus.Core;

namespace Abyss.World.Entities.Props.Concrete
{
    public class ShopEntrance : ActiveProp
    {
        private static readonly Rectangle boundingBox = new Rectangle(7, 0, 50, 64);

        public ShopEntrance(Vector2 initialPosition)
            : base(
            "ShopEntrance", 
            "Entrance to the inter-dimensional shop.", 
            initialPosition,
            boundingBox,
            Vector2.Zero)
        {
        }

        /// <summary>
        /// Activates the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void Activate(IPlayer player)
        {
            base.Activate(player);
            Monde.ActivateScene("Shop");
            (this.Components["PowerParticleEffect"]).Suspend();
        }

        protected override void InitializeComponents()
        {
            this.AddComponent(Power.ComponentName, ParticleEffectFactory.GetParticleEffect(this, Power.ComponentName, new Vector2(this.Collider.BoundingBox.Center.X, this.Collider.BoundingBox.Top), Color.SkyBlue));

        }
    }
}
