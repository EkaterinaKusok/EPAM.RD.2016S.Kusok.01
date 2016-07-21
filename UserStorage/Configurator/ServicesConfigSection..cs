using System.Configuration;

namespace UserStorage.Configurator
{

    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Master")]
        public ServiceDescription Master => (ServiceDescription)base["Master"];

        [ConfigurationProperty("Slave")]
        public ServiceDescription Slave => (ServiceDescription)base["Slave"];
    }

}
