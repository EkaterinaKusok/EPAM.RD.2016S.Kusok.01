using System.Configuration;

namespace Configurator.CustomConfigSection
{
    /// <summary>
    /// Represents a service configuration element within a configuration file.
    /// </summary>
    public class ServiceElement : ConfigurationElement
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is master.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is master; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("isMaster", DefaultValue = false, IsRequired = false)]
        public bool IsMaster
        {
            get { return (bool)base["isMaster"]; }
            set { base["isMaster"] = value; }
        }

        /// <summary>
        /// Gets or sets the type of the service.
        /// </summary>
        [ConfigurationProperty("serviceType", IsRequired = true)]
        public string ServiceType
        {
            get { return (string)base["serviceType"]; }
            set { base["serviceType"] = value; }
        }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        [ConfigurationProperty("ipAddress", IsRequired = true)]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["ipAddress"] = value; }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }

        /// <summary>
        /// Gets or sets the host address.
        /// </summary>
        [ConfigurationProperty("hostAddress", IsRequired = true)]
        public string HostAddress
        {
            get { return (string)base["hostAddress"]; }
            set { base["hostAddress"] = value; }
        }
        #endregion
    }
}
