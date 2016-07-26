using System.Configuration;

namespace UserStorage.Configurator.CustomConfigSection
{
    public class StartupServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("IdGenerator")]
        public StartupServiceElement IdGenerator => (StartupServiceElement)base["IdGenerator"];

        [ConfigurationProperty("Saver")]
        public StartupServiceElement Saver => (StartupServiceElement)base["Saver"];

        [ConfigurationProperty("Validator")]
        public StartupServiceElement Validators => (StartupServiceElement)base["Validator"];

        [ConfigurationProperty("Services")]
        public ServicesCollection Services => (ServicesCollection)base["Services"];
    }
}
