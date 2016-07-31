using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Service;

namespace UserStorage.Configurator
{
    public interface IServiceConfigurator
    {
        IService MasterService { get; }
        List<IService> SlaveServices { get; }
        void Start();
        void Stop();
    }
}
