using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Service;
using UserStorage.UserEntities;
using System.Configuration;

namespace UserStorage.Configurator
{
    public class Configurator
    {
        private IService masterService;
        private List<IService> slaveServices;

        public void Start()
        {
            ServicesConfigSection servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("Services");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            if (servicesSection.Master.Count != 1)
            {
                throw new ConfigurationErrorsException("Count of masters must be one.");
            }

            IService masterService = new MasterService();

            slaveServices = new List<IService>();

            for (int i = 0; i < servicesSection.Slave.Count; i++)
            {
                IService slaveService = new SlaveService(masterService);
            }
        }

        public void End()
        {
            ((IService)masterService).SaveState();
        }
    }

}
