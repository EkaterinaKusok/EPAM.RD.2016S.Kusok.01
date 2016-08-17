namespace UserStorage.Interfacies.Generators
{
    /// <summary>
    /// Provides functionality for generating values.
    /// </summary>
    /// <typeparam name="T"> Type of the generated object.</typeparam>
    public interface IGenerator<T>
    {
        /// <summary>
        /// Generates the new identifier of type T.
        /// </summary>
        /// <returns>The new identifier of type T.</returns>
        T GenerateNewId();

        /// <summary>
        /// Generates the new identifier of type T.
        /// </summary>
        /// <param name="currentId">The current identifier.</param>
        /// <returns>The new identifier of type T.</returns>
        T GenerateNewId(T currentId);

        /// <summary>
        /// Sets the current value of identifier.
        /// </summary>
        /// <param name="currentId">The current identifier.</param>
        /// <returns>True if function work correctly; otherwise - false.</returns>
        bool SetCurrentId(T currentId);

        /// <summary>
        /// Gets the current identifier.
        /// </summary>
        /// <returns>The current identifier of type T.</returns>
        T GetCurrentId();
    }
}
