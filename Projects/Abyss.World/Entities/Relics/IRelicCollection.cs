using System.Collections.Generic;

namespace Abyss.World.Entities.Relics
{
    public interface IRelicCollection
    {
        /// <summary>
        /// Occurs when relics are activated.
        /// </summary>
        event RelicsActivatedEventHandler RelicsActivated;

        ///// <summary>
        ///// Gets the key relics.
        ///// </summary>
        //IDictionary<string, IRelic> KeyRelics { get; }

        ///// <summary>
        ///// Gets the persistent relics.
        ///// </summary>
        //IDictionary<string, IRelic> PersistentRelics { get; }

        ///// <summary>
        ///// Gets the transient relics.
        ///// </summary>
        //IDictionary<string, IRelic> TransientRelics { get; }

        ///// <summary>
        ///// Gets the power up relics.
        ///// </summary>
        //IDictionary<string, IRelic> PowerUpRelics { get; }

        ///// <summary>
        ///// Gets the stomp relics.
        ///// </summary>
        //IEnumerable<IRelic> StompRelics { get; }

        ///// <summary>
        ///// Gets the dimension shift relics.
        ///// </summary>
        //IEnumerable<IRelic> DimensionShiftRelics { get; }

        /// <summary>
        /// Gets the passive relics.
        /// </summary>
        IDictionary<string, IRelic> PassiveRelics { get; }
        IDictionary<string, IRelic> ActiveRelics { get; }
        IDictionary<string, IRelic> CosmeticRelics { get; }

        IDictionary<string, IRelic> this[RelicType relicType] { get; }

        /// <summary>
        /// Adds the relic to relevant collections and activates it required.
        /// </summary>
        /// <param name="relic">The relic.</param>
        void AddRelic(IRelic relic);

        /// <summary>
        /// Removes the relic.
        /// </summary>
        /// <param name="relic">The relic.</param>
        void RemoveRelic(IRelic relic);

        /// <summary>
        /// Resets the relic collection. This will remove all Transient and PowerUp relics.
        /// </summary>
        void Reset();

        /// <summary>
        /// Initializes the relic collection. This will remove all relics.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Activates relics of a particular activation type.
        /// </summary>
        /// <param name="activationType">Type of the activation.</param>
        void ActivateRelics(RelicActivationType activationType);
    }
}
