﻿namespace Occasus.Core.Drawing.Lighting
{
    public static class Lighting
    {
        /// <summary>
        /// Entity is deferred from rendering until after a lighting pass.
        /// </summary>
        public static string DeferredRender = "DeferredRender";

        /// <summary>
        /// Entity is a deferred light source.
        /// </summary>
        public static string DeferredLightSource = "DeferredLightSource";

        public static string HasDeferredRenderComponents = "HasDeferredRenderComponents";
    }
}
