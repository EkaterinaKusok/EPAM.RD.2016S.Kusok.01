using System.Configuration;

namespace Configurator.CustomConfigSection
{
    public class StartupServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("UserStorage")]
        public StartupServiceElement UserStorage => (StartupServiceElement)base["UserStorage"];

        [ConfigurationProperty("IdGenerator")]
        public StartupServiceElement IdGenerator => (StartupServiceElement)base["IdGenerator"];

        [ConfigurationProperty("Saver")]
        public StartupServiceElement Saver => (StartupServiceElement)base["Saver"];

        [ConfigurationProperty("Logger")]
        public StartupServiceElement Logger => (StartupServiceElement)base["Logger"];

        [ConfigurationProperty("Sender")]
        public StartupServiceElement Sender => (StartupServiceElement)base["Sender"];

        [ConfigurationProperty("Receiver")]
        public StartupServiceElement Receiver => (StartupServiceElement)base["Receiver"];

        [ConfigurationProperty("Validator")]
        public StartupServiceElement Validator => (StartupServiceElement)base["Validator"];

        [ConfigurationProperty("Services")]
        public ServicesCollection Services => (ServicesCollection)base["Services"];
    }
}
