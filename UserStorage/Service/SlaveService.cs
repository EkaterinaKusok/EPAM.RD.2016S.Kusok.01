using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Web.Script.Serialization;
using UserStorage.Generator;
using UserStorage.ServiceInfo;
using UserStorage.StateSaver;
using UserStorage.UserEntities;
using UserStorage.UserStorage;
using UserStorage.Validator;

namespace UserStorage.Service
{
    [Serializable]
    public class SlaveService : MarshalByRefObject, IService
    {
        private readonly TcpListener server;
        private readonly IUserStorage userStorage;
        public string Name { get; }
        public ServiceMode Mode => ServiceMode.Slave;
        
        public SlaveService(IGenerator<int> idGenerator, IStateSaver saver, IUserValidator validator,
            ConnectionInfo info)
        {
            userStorage = new MemoryUserStorage(idGenerator, validator, saver);
            server = new TcpListener(info.IPAddress, info.Port);
            server.Start();
        }

        public async void ListenForUpdates()
        {
            var serializer = new JavaScriptSerializer();
            try
            {
                while (true)
                {
                    using (TcpClient client = await server.AcceptTcpClientAsync())
                    using (NetworkStream stream = client.GetStream())
                    {
                        string serializedMessage = string.Empty;
                        byte[] data = new byte[1024];

                        while (stream.DataAvailable)
                        {
                            int i = await stream.ReadAsync(data, 0, data.Length);
                            serializedMessage += Encoding.UTF8.GetString(data, 0, i);
                        }
                        ServiceMessage message = serializer.Deserialize<ServiceMessage>(serializedMessage);
                        UpdateOnModifying(message);
                    }
                }
            }
            finally
            {
                server.Stop();
            }
        }

        private void UpdateOnModifying(ServiceMessage message)
        {
            switch (message.Operation)
            {
                case ServiceOperation.Add:
                    userStorage.Add(message.User);
                    break;
                case ServiceOperation.Remove:
                    userStorage.Delete(message.User.Id);
                    break;
            }
        }
        
        public int Add(User user)
        {
            server.Stop();
            throw new NotSupportedException();
        }

        public IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            return this.userStorage.SearchForUser(predicates);
        }

        public void Delete(int id)
        {
            server.Stop();
            throw new NotSupportedException();
        }

        public void Save()
        {
            server.Stop();
            throw new NotSupportedException();
        }

        public void Load()
        {
            server.Stop();
            throw new NotSupportedException();
        }
    }
}
