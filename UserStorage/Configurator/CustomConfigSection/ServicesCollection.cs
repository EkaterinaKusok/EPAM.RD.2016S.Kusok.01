﻿
using System.Configuration;

namespace UserStorage.Configurator.CustomConfigSection
{
    [ConfigurationCollection(typeof(ServiceElement), AddItemName = "Service")]
    public class ServicesCollection : ConfigurationElementCollection
    {
        public ServiceElement this[int index] => (ServiceElement)BaseGet(index);

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).Port;
        }
    }
}
