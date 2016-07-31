using System.Collections.Generic;
using UserStorage.Interfacies.Services;
using UserStorage.Service;

namespace Configurator
{
    interface IServiceConfigurator
    {
        IService MasterService { get; }
        List<IService> SlaveServices { get; }
        void Start();
        void End();
    }
}
