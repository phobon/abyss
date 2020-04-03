namespace Occasus.Core
{
    public static class EngineFlag
    {
        /// <summary>
        /// Engine component should participate in the Update loop.
        /// </summary>
        public const string Active = "Active";

        /// <summary>
        /// Engine component should participate in the Draw loop.
        /// </summary>
        public const string Visible = "Visible";

        /// <summary>
        /// Engine component is relevant and should not be removed from the Game loop.
        /// </summary>
        public const string Relevant = "Relevant";

        /// <summary>
        /// Engine component should be included in non-specific collision checks. If an Entity has a collider and is referenced, it is included in a specific collision check.
        /// </summary>
        public const string Collidable = "Collidable";

        /// <summary>
        /// Engine component is initialized.
        /// </summary>
        public const string Initialized = "Initialized";

        /// <summary>
        /// Engine component has had its Begin method called.
        /// </summary>
        public const string HasBegun = "HasBegun";

        /// <summary>
        /// Engine component is excluded from a Draw loop to be drawn independently. This is usually for a lighting pass.
        /// </summary>
        public const string DeferredRender = "DeferredRender";

        /// <summary>
        /// Engine component is excluded from a Begin() call. These components usually have their Begin() called manually.
        /// </summary>
        public const string DeferredBegin = "DeferredBegin";

        /// <summary>
        /// Engine component that is new.
        /// </summary>
        public const string New = "New";

        /// <summary>
        /// Engine component should be included in cache regardless of conditions.
        /// </summary>
        public const string ForceIncludeInCache = "ForceIncludeInCache";
    }
}
