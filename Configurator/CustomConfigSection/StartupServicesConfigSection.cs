using System.Configuration;

namespace Configurator.CustomConfigSection
{
    /// <summary>
    /// Represents a startup service config section within a configuration file.
    /// </summary>
    public class StartupServicesConfigSection : ConfigurationSection
    {
        #region Properties

        /// <summary>
        /// Gets user storage from custom section within a configuration file.
        /// </summary>
        [ConfigurationProperty("UserStorage")]
        public StartupServiceElement UserStorage => (StartupServiceElement)base["UserStorage"];

        /// <summary>
        /// Gets ID generator from custom section within a configuration file.
        /// </summary>
        [ConfigurationProperty("IdGenerator")]
        public StartupServiceElement IdGenerator => (StartupServiceElement)base["IdGenerator"];

        /// <summary>
        /// Gets state saver from custom section within a configuration file.
        /// </summary>
        [ConfigurationProperty("Saver")]
        public StartupServiceElement Saver => (StartupServiceElement)base["Saver"];

        /// <summary>
        /// Gets logger from custom section within a configuration file.
        /// </summary>
        [ConfigurationProperty("Logger")]
        public StartupServiceElement Logger => (StartupServiceElement)base["Logger"];

        /// <summary>
        /// Gets sender from custom section within a configuration file.
        /// </summary>
        [ConfigurationProperty("Sender")]
        public StartupServiceElement Sender => (StartupServiceElement)base["Sender"];

        /// <summary>
        /// Gets receiver from custom section within a configuration file.
        /// </summary>
        [ConfigurationProperty("Receiver")]
        public StartupServiceElement Receiver => (StartupServiceElement)base["Receiver"];

        /// <summary>
        /// Gets validator from custom section within a configuration file.
        /// </summary>
        [ConfigurationProperty("Validator")]
        public StartupServiceElement Validator => (StartupServiceElement)base["Validator"];

        /// <summary>
        /// Gets collection of services from custom section within a configuration file.
        /// </summary>
        [ConfigurationProperty("Services")]
        public ServicesCollection Services => (ServicesCollection)base["Services"];
        #endregion
    }
}
