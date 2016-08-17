using System.Collections.Generic;
using UserStorage.Interfacies.Services;

namespace Configurator
{
    /// <summary>
    /// Provides functionality for service configurating.
    /// </summary>
    public interface IServiceConfigurator
    {
        /// <summary>
        /// Gets the master service.
        /// </summary>
        IService MasterService { get; }

        /// <summary>
        /// Gets the list of slave services.
        /// </summary>
        List<IService> SlaveServices { get; }

        /// <summary>
        /// Starts this instance of service configurator.
        /// </summary>
        void Start();

        /// <summary>
        /// Ends this instance of service configurator.
        /// </summary>
        void End();
    }
}
