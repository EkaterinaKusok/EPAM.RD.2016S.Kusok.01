using System.Configuration;

namespace Configurator.CustomConfigSection
{
    /// <summary>
    /// Represents a configuration element containing a collection of service elements.
    /// </summary>
    [ConfigurationCollection(typeof(ServiceElement), AddItemName = "Service")]
    public class ServicesCollection : ConfigurationElementCollection
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="ServiceElement"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="ServiceElement"/>.
        /// </value>
        /// <param name="index">The index in array of service elements.</param>
        /// <returns></returns>
        public ServiceElement this[int index] => (ServiceElement)BaseGet(index);
        #endregion

        #region ServicesCollectio Methods

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A newly created <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).Port;
        }
        #endregion
    }
}
