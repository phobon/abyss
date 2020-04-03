namespace Occasus.Core.Layers
{
    public enum LayerType
    {
        /// <summary>
        /// Layer is not transformed to the camera; rather it is static. This is really only used for backgrounds.
        /// </summary>
        Static,

        /// <summary>
        /// Layer is transformed to the camera.
        /// </summary>
        TransformedToCamera,

        /// <summary>
        /// An Interface layer type is static, but also does not get drawn to a back buffer prior to rendering.
        /// </summary>
        Interface
    }
}
