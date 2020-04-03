using System;
using Abyss.World.Entities.Props.Concrete;
using Abyss.World.Universe;
using Occasus.Core.Layers;

namespace Abyss.World.Phases.Concrete.Argus
{
    public class CursedChests : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CursedChests"/> class.
        /// </summary>
        public CursedChests(int difficulty, int scoreRequirement) 
            : base(
            Phase.CursedChests,
            "A phase where treasure chests become cursed and hurt the player.",
            difficulty,
            UniverseConstants.ArgusColor,
            scoreRequirement)
        {
        }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Apply(ILayer layer)
        {
            base.Apply(layer);

            if (layer.Parent.TagCache.ContainsKey(EntityTags.TreasureChest))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.TreasureChest])
                {
                    var treasureChest = (TreasureChest) e;
                    treasureChest.Activated += TreasureChestOnActivated;
                    treasureChest.ApplyCurse();
                }
            }
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            base.Remove(layer);

            if (layer.Parent.TagCache.ContainsKey(EntityTags.TreasureChest))
            {
                foreach (var e in layer.Parent.TagCache[EntityTags.TreasureChest])
                {
                    var treasureChest = (TreasureChest)e;
                    treasureChest.Activated -= TreasureChestOnActivated;
                    treasureChest.RemoveCurse();
                }
            }
        }

        private void TreasureChestOnActivated(object sender, EventArgs eventArgs)
        {
            GameManager.Player.TakeDamage(this.Name);
        }
    }
}
