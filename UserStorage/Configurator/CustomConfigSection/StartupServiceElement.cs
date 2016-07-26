using System.Configuration;

namespace UserStorage.Configurator.CustomConfigSection
{
    public class StartupServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}
