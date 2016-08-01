using System.Collections.Generic;
using UserStorage.Interfacies.Services;

namespace Configurator
{
    public interface IServiceConfigurator
    {
        IService MasterService { get; }

        List<IService> SlaveServices { get; }

        void Start();

        void End();
    }
}
