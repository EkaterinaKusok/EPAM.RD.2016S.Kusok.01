using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UserStorage.ServiceInfo;
using UserStorage.UserEntities;
using UserStorage.UserStorage;
using System.Web.Script.Serialization;
using UserStorage.Generator;
using UserStorage.StateSaver;
using UserStorage.Validator;

namespace UserStorage.Service
{
    [Serializable]
    public class MasterService : MarshalByRefObject,IService 
    {
        private readonly IUserStorage userStorage;
        public string Name { get; }
        public ServiceMode Mode => ServiceMode.Master;
        private readonly IEnumerable<ConnectionInfo> slavesInfo;

        public MasterService(string name, IUserStorage userStorage)
        {
            if (userStorage == null)
            {
                throw new ArgumentNullException(nameof(userStorage));
            }
            this.userStorage = userStorage;
            this.Name = name;
        }

        public MasterService(IGenerator<int> idGenerator, IStateSaver saver, IUserValidator validator, IEnumerable<ConnectionInfo> slavesInfo)
        {
            userStorage = new MemoryUserStorage(idGenerator, validator, saver);
            this.slavesInfo = slavesInfo;
        }

        public int Add(User user)
        {
            int id = this.userStorage.Add(user);
            user.Id = id;
            SendMessageToSlaves(new ServiceMessage(user, ServiceOperation.Add));
            return id;
        }

        public void Delete(int id)
        {
            var users = userStorage.SearchForUser(u => u.Id == id);
            this.userStorage.Delete(id);

            foreach (var user in users)
            {
                SendMessageToSlaves(new ServiceMessage(user, ServiceOperation.Remove));
            }
        }

        public IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            return this.userStorage.SearchForUser(predicates);
        }

        public void Save()
        {
            this.userStorage.Save();
        }

        public void Load()
        {
            this.userStorage.Load();
        }

        public void ListenForUpdates()
        {
            throw new NotSupportedException();
        }

        private void SendMessageToSlaves<T>(T message)
        {
            var serializer = new JavaScriptSerializer();
            string serializedMessage = serializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(serializedMessage);

            foreach (var slave in slavesInfo)
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(slave.IPAddress, slave.Port);
                    using (NetworkStream stream = client.GetStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
        }
    }
}
