using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace UserStorage.Configurator
{

    public class ServiceDescription : ConfigurationElement
    {
        [ConfigurationProperty("count", DefaultValue = 0, IsKey = false, IsRequired = true)]
        public int Count
        {
            get { return ((int)(base["count"])); }
            set { base["count"] = value; }
        }
    }

}
