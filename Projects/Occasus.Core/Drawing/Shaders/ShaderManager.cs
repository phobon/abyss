using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Occasus.Core.Drawing.Shaders
{
    public static class ShaderManager
    {
        private static readonly IList<IShader> includeDeferredRenderEntityShaders = new List<IShader>();
        private static readonly IList<IShader> applyBeforeScaleShaders = new List<IShader>();

        private static IDictionary<string, IShader> supportedShaders;
        private static ObservableCollection<IShader> activeShaders;

        /// <summary>
        /// Gets the supported shaders.
        /// </summary>
        public static IDictionary<string, IShader> SupportedShaders => supportedShaders ?? (supportedShaders = new Dictionary<string, IShader>());

        /// <summary>
        /// Gets the active shaders.
        /// </summary>
        public static ObservableCollection<IShader> ActiveShaders
        {
            get
            {
                if (activeShaders == null)
                {
                    activeShaders = new ObservableCollection<IShader>();
                    activeShaders.CollectionChanged += ActiveShadersCollectionChanged;
                }

                return activeShaders;
            }
        }

        public static IEnumerable<IShader> IncludeDeferredRenderEntityShaders => includeDeferredRenderEntityShaders;

        public static IEnumerable<IShader> ApplyBeforeScaleShaders => applyBeforeScaleShaders;

        public static void ClearActiveShaders()
        {
            var shaders = ActiveShaders.ToList();
            foreach (var s in shaders)
            {
                s.Deactivate();
            }
        }

        public static void Destroy()
        {
            activeShaders.CollectionChanged -= ActiveShadersCollectionChanged;
        }

        private static void ActiveShadersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (IShader i in e.OldItems)
                {
                    if (i.Usages[ShaderUsage.IncludeDeferredRenderEntities])
                    {
                        includeDeferredRenderEntityShaders.Remove(i);
                    }

                    if (i.Usages[ShaderUsage.ApplyBeforeScale])
                    {
                        applyBeforeScaleShaders.Remove(i);
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (IShader i in e.NewItems)
                {
                    if (i.Usages[ShaderUsage.IncludeDeferredRenderEntities])
                    {
                        includeDeferredRenderEntityShaders.Add(i);
                    }

                    if (i.Usages[ShaderUsage.ApplyBeforeScale])
                    {
                        applyBeforeScaleShaders.Add(i);
                    }
                }
            }
        }
    }
}
