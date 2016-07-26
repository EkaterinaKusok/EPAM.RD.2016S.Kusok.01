using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using UserStorage.Configurator.CustomConfigSection;
using UserStorage.Generator;
using UserStorage.Service;
using UserStorage.ServiceInfo;
using UserStorage.StateSaver;
using UserStorage.Validator;

namespace UserStorage.Configurator
{
    public class ServiceConfigurator : IServiceConfigurator
    {
        public IService MasterService { get; private set; }
        public List<IService> SlaveServices { get; private set; }

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

            if (masterCount != 1)
            {
                throw new ConfigurationErrorsException("Count of masters must be one.");
            }

            SlaveServices = new List<IService>();
            string masterType = string.Empty;
            var slavesInfo = new List<ConnectionInfo>();

            var generator = CreateInstance<IGenerator<int> >(servicesSection.IdGenerator.Type);
            var saver = CreateInstance<IStateSaver>(servicesSection.Saver.Type);
            var validator = CreateInstance<IUserValidator>(servicesSection.Validators.Type);

            for (int i = 0; i < servicesSection.Services.Count; i++)
            {
                if (servicesSection.Services[i].IsMaster)
                {
                    masterType = servicesSection.Services[i].ServiceType;
                }
                else
                {
                    IService slaveService = CreateSlaveService(servicesSection.Services[i], i, generator, saver, validator);
                    SlaveServices.Add(slaveService);
                    slavesInfo.Add(new ConnectionInfo(servicesSection.Services[i].IpAddress, servicesSection.Services[i].Port));
                    slaveService.ListenForUpdates();
                }
            }

            MasterService = CreateMasterService(masterType, generator, saver, validator, slavesInfo);
            MasterService.Load();
        }

        public void Stop()
        {
            MasterService.Save();
        }

        private IService CreateMasterService(string serviceType, IGenerator<int> generator, IStateSaver saver, IUserValidator validator, IEnumerable<ConnectionInfo> slavesInfo)
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

            if (type.GetConstructor(new[] { typeof(IGenerator<int>), typeof(IStateSaver), typeof(IUserValidator), typeof(IEnumerable<ConnectionInfo>) }) == null)
            {
                throw new ArgumentException($"Unable to create service of type '{serviceType}'.");
            }

            AppDomain appDomain = AppDomain.CreateDomain("Master");

            var master = (IService)appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true,
                BindingFlags.CreateInstance, null,
                new object[] { generator, saver, validator, slavesInfo },
                CultureInfo.InvariantCulture, null);

            if (master == null)
            {
                throw new ConfigurationErrorsException("Unable to load domain of master service.");
            }
            return master;
        }

        private IService CreateSlaveService(ServiceElement service, int slaveIndex, IGenerator<int> generator, IStateSaver saver, IUserValidator validator)
        {
            if (service.ServiceType == null)
            {
                throw new ArgumentNullException($"{nameof(service.ServiceType)} must be not null.");
            }

            Type type = Type.GetType(service.ServiceType);

            if (type == null)
            {
                throw new NullReferenceException($"Type {service.ServiceType} not found.");
            }

            if (type.GetConstructor(new[] { typeof(IGenerator<int>), typeof(IStateSaver), typeof(IUserValidator), typeof(ConnectionInfo) }) == null)
            {
                throw new ArgumentException($"Unable to create service of type '{service.ServiceType}'.");
            }

            AppDomain appDomain = AppDomain.CreateDomain($"Slave{slaveIndex}");

            var slave = (IService)appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true,
                BindingFlags.CreateInstance, null,
                new object[] { generator, saver, validator, new ConnectionInfo(service.IpAddress, service.Port) },
                CultureInfo.InvariantCulture, null);

            if (slave == null)
            {
                throw new ConfigurationErrorsException($"Unable to load domain of slave service #{slaveIndex}.");
            }
            return slave;
        }

        private T CreateInstance<T>(string instanceType)
        {
            if (instanceType == null)
            {
                throw new ArgumentNullException($"{nameof(instanceType)} must be not null.");
            }

            Type type = Type.GetType(instanceType);

            if (type == null)
            {
                throw new NullReferenceException($"Type '{instanceType}' not found.");
            }

            if (type.GetInterface(typeof(T).Name) == null || type.GetConstructor(new Type[] { }) == null)
            {
                throw new ArgumentException($"Unable to create instance of type '{instanceType}' implementing interface '{typeof(T).Name}'.");
            }

            return (T)Activator.CreateInstance(type);
        }
    }
}
