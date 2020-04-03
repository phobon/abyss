using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Occasus.Core.Drawing.Shaders
{
    public abstract class Shader : IShader
    {
        private readonly string technique;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shader" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="usages">The usage.</param>
        /// <param name="drawOffset">The draw offset.</param>
        protected Shader(string name, string description, string technique, IEnumerable<ShaderUsage> usages = null, Vector2 drawOffset = new Vector2())
        {
            this.Name = name;
            this.Description = description;
            this.technique = technique;

            this.Usages = new Dictionary<ShaderUsage, bool>
            {
                { ShaderUsage.ApplyBeforeScale, false },
                { ShaderUsage.IncludeDeferredRenderEntities, false },
            };

            if (usages != null)
            {
                foreach (var su in usages)
                {
                    this.Usages[su] = true;
                }
            }

            this.DrawOffset = drawOffset;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the effect.
        /// </summary>
        public Effect Effect { get; protected set; }

        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the usage of this particular shader.
        /// </summary>
        public IDictionary<ShaderUsage, bool> Usages { get; private set; }

        /// <summary>
        /// Gets the draw offset for this shader. This is only applicable to fullscreen effects that alter rendered pixel positions in the shader.
        /// eg: Vertical and Horizontal flip.
        /// </summary>
        public Vector2 DrawOffset { get; private set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public abstract void LoadContent();

        public void Activate()
        {
            ShaderManager.ActiveShaders.Add(this);
            this.Effect.CurrentTechnique = this.Effect.Techniques[this.technique];
            this.IsActive = true;
        }

        public void Deactivate()
        {
            ShaderManager.ActiveShaders.Remove(this);
            this.Effect.CurrentTechnique = null;
            this.IsActive = false;
        }
    }
}
