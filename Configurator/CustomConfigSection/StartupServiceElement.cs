using System.Configuration;

namespace Configurator.CustomConfigSection
{
    /// <summary>
    /// Represents a startup service element within a configuration file.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class StartupServiceElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}
