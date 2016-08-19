using UserStorage.Interfacies.ServiceInfo;

namespace UserStorage.Interfacies.StateSavers
{
    /// <summary>
    /// Provides functionality for saving state.
    /// </summary>
    public interface IStateSaver
    {
        /// <summary>
        /// Loads the state.
        /// </summary>
        /// <returns></returns>
        StorageState LoadState();

        /// <summary>
        /// Saves the state.
        /// </summary>
        /// <param name="state">The storage state.</param>
        void SaveState(StorageState state);
    }
}
