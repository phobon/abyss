using System;
using System.Collections.Generic;
using System.Linq;
using Abyss.World.Entities.Player;
using Abyss.World.Entities.Relics.Concrete;
using Occasus.Core.Maths;

namespace Abyss.World.Entities.Relics
{
    public class RelicCollection : IRelicCollection
    {
        //private readonly IList<IRelic> stompRelics;
        //private readonly IList<IRelic> dimensionShiftRelics;
        //private readonly IList<IRelic> passiveRelics;

        private readonly IList<IRelic> passiveRelics;
        private readonly IList<IRelic> activeRelics;
        private readonly IList<IRelic> cosmeticRelics;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelicCollection"/> class.
        /// </summary>
        public RelicCollection()
        {
            //this.KeyRelics = new Dictionary<string, IRelic>(StringComparer.OrdinalIgnoreCase);
            //this.PersistentRelics = new Dictionary<string, IRelic>(StringComparer.OrdinalIgnoreCase);
            //this.PowerUpRelics = new Dictionary<string, IRelic>(StringComparer.OrdinalIgnoreCase);
            //this.TransientRelics = new Dictionary<string, IRelic>(StringComparer.OrdinalIgnoreCase);

            //this.stompRelics = new List<IRelic>();
            //this.dimensionShiftRelics = new List<IRelic>();
            this.passiveRelics = new List<IRelic>();
            this.activeRelics = new List<IRelic>();
            this.cosmeticRelics = new List<IRelic>();

            this.Initialize();
        }

        /// <summary>
        /// Occurs when relics are activated.
        /// </summary>
        public event RelicsActivatedEventHandler RelicsActivated;

        /// <summary>
        /// Gets the key relics.
        /// </summary>
        //public IDictionary<string, IRelic> KeyRelics
        //{
        //    get; private set;
        //}

        /// <summary>
        /// Gets the persistent relics.
        /// </summary>
        //public IDictionary<string, IRelic> PersistentRelics
        //{
        //    get; private set;
        //}

        /// <summary>
        /// Gets the transient relics.
        /// </summary>
        //public IDictionary<string, IRelic> TransientRelics
        //{
        //    get; private set;
        //}

        /// <summary>
        /// Gets the power up relics.
        /// </summary>
        //public IDictionary<string, IRelic> PowerUpRelics
        //{
        //    get; private set;
        //}

        /// <summary>
        /// Gets the stomp relics.
        /// </summary>
        //public IEnumerable<IRelic> StompRelics
        //{
        //    get { return this.stompRelics; }
        //}

        /// <summary>
        /// Gets the dimension shift relics.
        /// </summary>
        //public IEnumerable<IRelic> DimensionShiftRelics
        //{
        //    get { return this.dimensionShiftRelics; }
        //}

        /// <summary>
        /// Gets the passive relics.
        /// </summary>
        public IDictionary<string, IRelic> PassiveRelics { get; private set; }
        public IDictionary<string, IRelic> ActiveRelics { get; private set; }
        public IDictionary<string, IRelic> CosmeticRelics { get; private set; }

        public IDictionary<string, IRelic> this[RelicType relicType]
        {
            get
            {
                switch (relicType)
                {
                    case RelicType.Passive:
                        return this.PassiveRelics;
                    case RelicType.Active:
                        return this.ActiveRelics;
                    case RelicType.Cosmetic:
                        return this.CosmeticRelics;
                    //case RelicType.Key:
                    //    return this.KeyRelics;
                    //case RelicType.Persistent:
                    //    return this.PersistentRelics;
                    //case RelicType.PowerUp:
                    //    return this.PowerUpRelics;
                    //case RelicType.Transient:
                    //    return this.TransientRelics;
                }

                throw new InvalidOperationException("Cannot find dictionary for RelicType.");
            }
        }

        public IList<IRelic> this[RelicActivationType relicActivationType]
        {
            get
            {
                switch (relicActivationType)
                {
                    case RelicActivationType.None:
                        return this.passiveRelics;
                    case RelicActivationType.Explicit:
                        return this.activeRelics;
                    case RelicActivationType.Stomp:
                        return this.passiveRelics.Where(o => o.RelicActivationType == RelicActivationType.Stomp).ToList();
                    //case RelicActivationType.Stomp:
                    //    return this.stompRelics;
                    //case RelicActivationType.DimensionShift:
                    //    return this.dimensionShiftRelics;
                }

                return null;
            }
        }

        /// <summary>
        /// Adds the relic to relevant collections and activates it required.
        /// </summary>
        /// <param name="relic">The relic.</param>
        public void AddRelic(IRelic relic)
        {
            switch (relic.RelicType)
            {
                case RelicType.Passive:
                    this.PassiveRelics[relic.Name] = relic;
                    break;
                case RelicType.Active:
                    this.ActiveRelics[relic.Name] = relic;
                    break;
                case RelicType.Cosmetic:
                    this.CosmeticRelics[relic.Name] = relic;
                    break;
                //case RelicType.Key:
                //    this.KeyRelics[relic.Name] = relic;
                //    break;
                //case RelicType.Persistent:
                //    this.PersistentRelics[relic.Name] = relic;
                //    break;
                //case RelicType.PowerUp:
                //    this.PowerUpRelics[relic.Name] = relic;
                //    break;
                //case RelicType.Transient:
                //    this.TransientRelics[relic.Name] = relic;
                //    break;
            }

            //switch (relic.RelicActivationType)
            //{
                //case RelicActivationType.Stomp:
                //    this.stompRelics.Add(relic);
                //    break;
                //case RelicActivationType.Passive:
                //    this.passiveRelics.Add(relic);
                //    break;
                //case RelicActivationType.DimensionShift:
                //    this.dimensionShiftRelics.Add(relic);
                //    break;
                //case RelicActivationType.Instant:
                //    // Relic is activated instantly.
                //    this.OnRelicsActivated(new RelicsActivatedEventArgs(relic));
                //    relic.Deactivated += this.RelicOnDeactivated;
                //    break;
            //}

            GameManager.StatisticManager.RelicsCollected.Add(relic.Name);
        }

        public void RemoveRelic(IRelic relic)
        {
            switch (relic.RelicType)
            {
                case RelicType.Passive:
                    this.PassiveRelics[relic.Name] = null;
                    break;
                case RelicType.Active:
                    this.ActiveRelics[relic.Name] = null;
                    break;
                case RelicType.Cosmetic:
                    this.CosmeticRelics[relic.Name] = null;
                    break;
                    //case RelicType.Key:
                    //    this.KeyRelics[relic.Name] = null;
                    //    break;
                    //case RelicType.Persistent:
                    //    this.PersistentRelics[relic.Name] = null;
                    //    break;
                    //case RelicType.PowerUp:
                    //    this.PowerUpRelics[relic.Name] = null;
                    //    break;
                    //case RelicType.Transient:
                    //    this.TransientRelics[relic.Name] = null;
                    //    break;
            }

            //switch (relic.RelicActivationType)
            //{
            //    case RelicActivationType.Stomp:
            //        this.stompRelics.Remove(relic);
            //        GameManager.Player.PlatformStomped -= PlayerOnPlatformStomped;
            //        break;
            //    case RelicActivationType.Passive:
            //        this.passiveRelics.Remove(relic);
            //        break;
            //    case RelicActivationType.DimensionShift:
            //        this.dimensionShiftRelics.Remove(relic);
            //        break;
            //    case RelicActivationType.Instant:
            //        // Relic is activated instantly.
            //        this.OnRelicsActivated(new RelicsActivatedEventArgs(relic));
            //        relic.Deactivated += this.RelicOnDeactivated;
            //        break;
            //}

            relic.Deactivate(null);
        }

        /// <summary>
        /// Resets the relic collection.
        /// </summary>
        public void Reset()
        {
            this.passiveRelics.Clear();
            this.activeRelics.Clear();
            this.cosmeticRelics.Clear();

            //var relicKeys = this.TransientRelics.Keys.ToList();
            //foreach (var t in relicKeys)
            //{
            //    if (this.TransientRelics[t] != null)
            //    {
            //        this.TransientRelics[t].Deactivate(null);
            //    }

            //    this.TransientRelics[t] = null;
            //}

            //relicKeys = this.PowerUpRelics.Keys.ToList();
            //foreach (var p in relicKeys)
            //{
            //    if (this.PowerUpRelics[p] != null)
            //    {
            //        this.PowerUpRelics[p].Deactivate(null);
            //    }

            //    this.PowerUpRelics[p] = null;
            //}

            //this.stompRelics.Clear();
            //this.passiveRelics.Clear();
            //this.dimensionShiftRelics.Clear();
        }

        /// <summary>
        /// Initializes the relic collection. This will remove all relics.
        /// </summary>
        public void Initialize()
        {
            this.passiveRelics.Clear();
            this.activeRelics.Clear();
            this.cosmeticRelics.Clear();

            this.PassiveRelics = RelicKeys.PassiveRelics;
            this.ActiveRelics = RelicKeys.ActiveRelics;
            this.CosmeticRelics = RelicKeys.CosmeticRelics;

            //this.stompRelics.Clear();
            //this.passiveRelics.Clear();
            //this.dimensionShiftRelics.Clear();

            //this.TransientRelics.Clear();
            //this.PowerUpRelics.Clear();
            //this.KeyRelics.Clear();
            //this.PersistentRelics.Clear();

            //this.TransientRelics = RelicKeys.TransientRelics;
            //this.PowerUpRelics = RelicKeys.PowerupRelics;
            //this.KeyRelics = RelicKeys.KeyRelics;
            //this.PersistentRelics = RelicKeys.PersistentRelics;

            GameManager.Player.PlatformStomped += PlayerOnPlatformStomped;
        }

        /// <summary>
        /// Activates relics of a particular activation type.
        /// </summary>
        /// <param name="activationType">Type of the activation.</param>
        public void ActivateRelics(RelicActivationType activationType)
        {
            var activatedRelics = new List<IRelic>();
            foreach (var r in this[activationType])
            {
                if (MathsHelper.Random() <= r.ActivationChance)
                {
                    activatedRelics.Add(r);
                }
            }

            if (activatedRelics.Count > 0)
            {
                this.OnRelicsActivated(new RelicsActivatedEventArgs(activatedRelics));
            }
        }

        private void OnRelicsActivated(RelicsActivatedEventArgs args)
        {
            var handler = this.RelicsActivated;
            if (handler != null)
            {
                handler(args);
            }
        }

        //private void RelicOnDeactivated(object sender, EventArgs eventArgs)
        //{
        //    var relic = (IRelic)sender;
        //    this.PowerUpRelics[relic.Name] = null;
        //    relic.Deactivated -= this.RelicOnDeactivated;
        //}

        private void PlayerOnPlatformStomped(PlatformStompedEventArgs platformStompedEventArgs)
        {
            foreach (StompRelic relic in this[RelicActivationType.Stomp])
            {
                relic.CurrentPlatform = platformStompedEventArgs.Platform;
            }
        }
    }
}
