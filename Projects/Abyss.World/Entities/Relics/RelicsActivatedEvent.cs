using System;
using System.Collections.Generic;

namespace Abyss.World.Entities.Relics
{
    public delegate void RelicsActivatedEventHandler(RelicsActivatedEventArgs e);

    public class RelicsActivatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelicsActivatedEventArgs" /> class.
        /// </summary>
        /// <param name="relics">The relics.</param>
        public RelicsActivatedEventArgs(IEnumerable<IRelic> relics)
        {
            this.Relics = relics;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelicsActivatedEventArgs"/> class.
        /// </summary>
        /// <param name="relic">The relic.</param>
        public RelicsActivatedEventArgs(IRelic relic)
        {
            this.Relics = new List<IRelic> { relic };
        }

        public IEnumerable<IRelic> Relics
        {
            get; private set;
        }
    }
}
