namespace Occasus.Core.Drawing.Lighting
{
    public enum LightSourceType
    {
        /// <summary>
        /// A light cast by a single point in space that has no volume.
        /// </summary>
        Point,

        /// <summary>
        /// Similar to a point light but the light has volume, allowing a penumbra effect.
        /// </summary>
        Sphere,

        /// <summary>
        /// Colume of light is a cone instead of a sphere (like a flashlight).
        /// </summary>
        Spot
    }
}
