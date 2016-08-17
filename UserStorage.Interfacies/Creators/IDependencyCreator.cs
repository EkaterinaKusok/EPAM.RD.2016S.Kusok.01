using System.Collections.Generic;

namespace UserStorage.Interfacies.Creators
{
    /// <summary>
    /// Provides functionality for creating dependencies.
    /// </summary>
    public interface IDependencyCreator
    {
        /// <summary>
        /// Creates the instance of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The instance of type T.</returns>
        T CreateInstance<T>();

        /// <summary>
        /// Creates the instance of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters for creating.</param>
        /// <returns>The instance of type T.</returns>
        T CreateInstance<T>(params object[] parameters);
    }
}
