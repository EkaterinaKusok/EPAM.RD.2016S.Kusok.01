using System;
using System.ServiceModel;
using WcfServiceLibrary;

namespace WcfClient
{
    public class WcfCreator
    {
        public static ServiceHost CreateWcf(WcfService service)
        {
            Uri serviceUri = new Uri($"http://127.0.0.1:5050/Service/" + service.Name);
            ServiceHost host = new ServiceHost(service, serviceUri);
            host.Open();

            foreach (Uri uri in host.BaseAddresses)
            {
                Console.WriteLine("\t{0}", uri.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Number of dispatchers listening : {0}", host.ChannelDispatchers.Count);
            Console.WriteLine();
            return host;
        }
    }
}
