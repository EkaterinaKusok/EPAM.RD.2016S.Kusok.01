using UserStorage.Interfacies.ServiceInfo;
using UserStorage.Interfacies.Storages;

namespace UserStorage.Interfacies.Services
{
    /// <summary>
    /// Provides functionality for working with users.
    /// </summary>
    /// <seealso cref="UserStorage.Interfacies.Storages.IUserStorage" />
    public interface IService : IUserStorage
    {
        /// <summary>
        /// Gets the service mode.
        /// </summary>
        ServiceMode Mode { get; }
    }
}
