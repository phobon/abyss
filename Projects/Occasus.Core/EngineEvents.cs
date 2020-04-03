using System;

namespace Occasus.Core
{
    /// <summary>
    /// Custom event handler for when the engine framerate changes.
    /// </summary>
    /// <param name="e">The <see cref="FramerateChangedEventArgs"/> instance containing the event data.</param>
    public delegate void FramerateChangedEventHandler(FramerateChangedEventArgs e);

    public class FramerateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FramerateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="framerate">The framerate.</param>
        public FramerateChangedEventArgs(int framerate)
        {
            this.Framerate = framerate;
        }

        /// <summary>
        /// Gets the framerate.
        /// </summary>
        public int Framerate
        {
            get; private set;
        }
    }
}
