using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using Configurator.Creators;
using Configurator.CustomConfigSection;
using UserStorage.Interfacies.Creators;
using UserStorage.Interfacies.Generators;
using UserStorage.Interfacies.Logger;
using UserStorage.Interfacies.Network;
using UserStorage.Interfacies.ServiceInfo;
using UserStorage.Interfacies.Services;
using UserStorage.Interfacies.StateSavers;
using UserStorage.Interfacies.Storages;
using UserStorage.Interfacies.Validators;

namespace WcfServiceLibrary.WcfConfigurator
{
    public class WcfServiceConfigurator
    {
        private WcfHost masterHost;
        private List<WcfHost> slaveHosts;

        public void Start()
        {
            StartupServicesConfigSection servicesSection = (StartupServicesConfigSection)ConfigurationManager.GetSection("StartupServices");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            int masterCount = 0;

            for (int i = 0; i < servicesSection.Services.Count; i++)
            {
                if (servicesSection.Services[i].IsMaster)
                {
                    masterCount++;
                }
            }

            if (masterCount > 1)
            {
                throw new ConfigurationErrorsException("Count of masters must be one.");
            }

            string masterType = string.Empty;
            string masterHostAddress = string.Empty;
            var slavesInfo = new List<ConnectionInfo>();
            slaveHosts = new List<WcfHost>();

            for (int i = 0; i < servicesSection.Services.Count; i++)
            {
                if (servicesSection.Services[i].IsMaster)
                {
                    masterType = servicesSection.Services[i].ServiceType;
                    masterHostAddress = servicesSection.Services[i].HostAddress;
                }
                else
                {
                    var connectionInfo = new ConnectionInfo(servicesSection.Services[i].IpAddress, servicesSection.Services[i].Port);
                    slavesInfo.Add(connectionInfo);

                    var slaveDependencies = new Dictionary<Type, InstanceInfo>();
                    slaveDependencies.Add(typeof(IGenerator<int>), new InstanceInfo(servicesSection.IdGenerator.Type));
                    slaveDependencies.Add(typeof(IStateSaver), new InstanceInfo(servicesSection.Saver.Type));
                    slaveDependencies.Add(typeof(ILogger), new InstanceInfo(servicesSection.Logger.Type));
                    slaveDependencies.Add(typeof(IUserStorage), new InstanceInfo(servicesSection.UserStorage.Type));
                    slaveDependencies.Add(typeof(IReceiver), new InstanceInfo(servicesSection.Receiver.Type, connectionInfo));

                    var slaveHost = CreateServiceHost(
                        servicesSection.Services[i].ServiceType,
                        new DependencyCreator(slaveDependencies),
                        servicesSection.Services[i].HostAddress);
                    slaveHosts.Add(slaveHost);
                }
            }

            var masterDependencies = new Dictionary<Type, InstanceInfo>();
            masterDependencies.Add(typeof(IGenerator<int>), new InstanceInfo(servicesSection.IdGenerator.Type));
            masterDependencies.Add(typeof(IStateSaver), new InstanceInfo(servicesSection.Saver.Type));
            masterDependencies.Add(typeof(ILogger), new InstanceInfo(servicesSection.Logger.Type));
            masterDependencies.Add(typeof(IUserStorage), new InstanceInfo(servicesSection.UserStorage.Type));
            masterDependencies.Add(typeof(IUserValidator), new InstanceInfo(servicesSection.Validator.Type));
            masterDependencies.Add(typeof(ISender), new InstanceInfo(servicesSection.Sender.Type, slavesInfo));
            
            masterHost = CreateServiceHost(
                masterType,
                new DependencyCreator(masterDependencies),
                masterHostAddress);
        }

        public void End()
        {
            masterHost.Close();
            foreach (var slaveHost in slaveHosts)
            {
                slaveHost.Close();
            }
        }

        private WcfHost CreateServiceHost(string serviceType, IDependencyCreator creator, string hostAddress)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException($"{nameof(serviceType)} must be not null.");
            }

            Type type = Type.GetType(serviceType);

            if (type == null)
            {
                throw new NullReferenceException($"Type {serviceType} not found.");
            }

            if (type.GetInterface(typeof(IService).Name) == null ||
                type.GetConstructor(new[] { typeof(IDependencyCreator) }) == null)
            {
                throw new ArgumentException($"Unable to create service of type '{serviceType}' implementing interface '{nameof(IUserService)}'.");
            }

            string domainName = GetDomainNameFromHostAddress(hostAddress);
            AppDomain appDomain = AppDomain.CreateDomain(domainName);

            var host = (WcfHost)appDomain.CreateInstanceAndUnwrap(
                typeof(WcfHost).Assembly.FullName,
                typeof(WcfHost).FullName);

            var service = (IService)appDomain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] { creator },
                CultureInfo.InvariantCulture,
                null);

            if (service == null)
            {
                throw new ConfigurationErrorsException($"Unable to load domain of service '{domainName}'.");
            }

            host.Start(hostAddress, service);
            return host;
        }

        private string GetDomainNameFromHostAddress(string hostAddress)
        {
            return hostAddress.Substring(hostAddress.LastIndexOf('/') + 1);
        }
    }
}
