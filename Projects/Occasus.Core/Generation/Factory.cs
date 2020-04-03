
namespace Occasus.Core.Generation
{
    public abstract class Factory<T>: IFactory<T>
    {
        /// <summary>
        /// Generates a random <see cref="T" />.
        /// </summary>
        /// <returns>
        /// A random <see cref="T" />.
        /// </returns>
        public abstract T GenerateRandom();

        /// <summary>
        /// Generates a <see cref="T" /> corresponding to the given identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// A <see cref="T" /> corresponding to the given identifier.
        /// </returns>
        public abstract T GenerateById(string id);

        /// <summary>
        /// Initialises this factory.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Loads the content of this factory.
        /// </summary>
        public abstract void LoadContent();
    }
}
