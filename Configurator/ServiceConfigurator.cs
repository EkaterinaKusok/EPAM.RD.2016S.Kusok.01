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

namespace Configurator
{
    /// <summary>
    /// Configures services.
    /// </summary>
    public class ServiceConfigurator : IServiceConfigurator
    {
        /// <summary>
        /// Gets the master service.
        /// </summary>
        public IService MasterService { get; private set; }

        /// <summary>
        /// Gets the list of slave services.
        /// </summary>
        public List<IService> SlaveServices { get; private set; }

        /// <summary>
        /// Starts this instance of service configurator.
        /// </summary>
        /// <exception cref="NullReferenceException">Unable to read section from config.</exception>
        /// <exception cref="ConfigurationErrorsException">Count of masters must be one.</exception>
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

            SlaveServices = new List<IService>();
            string masterType = string.Empty;
            var slavesInfo = new List<ConnectionInfo>();

            for (int i = 0; i < servicesSection.Services.Count; i++)
            {
                if (servicesSection.Services[i].IsMaster)
                {
                    masterType = servicesSection.Services[i].ServiceType;
                }
                else
                {
                    var connectionInfo = new ConnectionInfo(servicesSection.Services[i].IpAddress, servicesSection.Services[i].Port);
                    var slaveDependencies = new Dictionary<Type, InstanceInfo>();
                    slaveDependencies.Add(typeof(IGenerator<int>), new InstanceInfo(servicesSection.IdGenerator.Type));
                    slaveDependencies.Add(typeof(IStateSaver), new InstanceInfo(servicesSection.Saver.Type));
                    slaveDependencies.Add(typeof(ILogger), new InstanceInfo(servicesSection.Logger.Type));
                    slaveDependencies.Add(typeof(IUserStorage), new InstanceInfo(servicesSection.UserStorage.Type));
                    slaveDependencies.Add(typeof(IReceiver), new InstanceInfo(servicesSection.Receiver.Type, connectionInfo));

                    IService slaveService = CreateService($"Slave{i}", servicesSection.Services[i].ServiceType, new DependencyCreator(slaveDependencies));
                    SlaveServices.Add(slaveService);
                    slavesInfo.Add(connectionInfo);
                    (slaveService as IListener)?.ListenForUpdates();
                }
            }

            var masterDependencies = new Dictionary<Type, InstanceInfo>();
            masterDependencies.Add(typeof(IGenerator<int>), new InstanceInfo(servicesSection.IdGenerator.Type));
            masterDependencies.Add(typeof(IStateSaver), new InstanceInfo(servicesSection.Saver.Type));
            masterDependencies.Add(typeof(ILogger), new InstanceInfo(servicesSection.Logger.Type));
            masterDependencies.Add(typeof(IUserStorage), new InstanceInfo(servicesSection.UserStorage.Type));
            masterDependencies.Add(typeof(IUserValidator), new InstanceInfo(servicesSection.Validator.Type));
            masterDependencies.Add(typeof(ISender), new InstanceInfo(servicesSection.Sender.Type, slavesInfo));
            
            MasterService = CreateService("Master", masterType, new DependencyCreator(masterDependencies));
            MasterService?.Load();
        }

        /// <summary>
        /// Ends this instance of service configurator.
        /// </summary>
        public void End()
        {
            MasterService?.Save();
        }

        private IService CreateService(string serviceName, string serviceType, IDependencyCreator creator)
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
                throw new ArgumentException($"Unable to create service of type '{serviceType}' implementing interface '{nameof(IService)}'.");
            }

            AppDomain appDomain = AppDomain.CreateDomain(serviceName);

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
                throw new ConfigurationErrorsException($"Unable to load domain of service '{serviceName}'.");
            }

            return service;
        }
    }
}
