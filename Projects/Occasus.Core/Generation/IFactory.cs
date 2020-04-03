namespace Occasus.Core.Generation
{
    public interface IFactory<T>
    {
        /// <summary>
        /// Generates a random <see cref="T"/>.
        /// </summary>
        /// <returns>A random <see cref="T"/>.</returns>
        T GenerateRandom();

        /// <summary>
        /// Generates a <see cref="T"/> corresponding to the given identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="T"/> corresponding to the given identifier.</returns>
        T GenerateById(string id);

        /// <summary>
        /// Initialises this factory.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Loads the content of this factory.
        /// </summary>
        void LoadContent();
    }
}
