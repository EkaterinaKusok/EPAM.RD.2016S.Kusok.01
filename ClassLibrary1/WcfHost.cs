using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using UserStorage.Interfacies.Services;

namespace WcfServiceLibrary
{
    public class WcfHost : MarshalByRefObject
    {
        private ServiceHost host;
        private IService service;

        public void Start(string address, IService service)
        {
            Uri baseAddress = new Uri(address);
            var newService = new WcfService(service);
            this.host = new ServiceHost(newService, baseAddress);
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            this.host.Description.Behaviors.Add(smb);

            (service as IService)?.Load();
            (service as IListener)?.ListenForUpdates();
            this.service = service;

            this.host.Open();

            Console.WriteLine($"Domain: {AppDomain.CurrentDomain.FriendlyName}");
            Console.WriteLine($"The service is ready at {baseAddress}");
            Console.WriteLine();
        }

        public void Close()
        {
            this.host.Close();
            (this.host as IDisposable)?.Dispose();
        }
    }
}
