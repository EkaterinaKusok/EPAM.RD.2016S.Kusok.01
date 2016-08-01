using System;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.ServiceInfo
{
    [Serializable]
    public class ServiceMessage
    {
        public ServiceMessage()
        {
        }

        public ServiceMessage(User user, ServiceOperation operation)
        {
            User = user;
            Operation = operation;
        }

        public User User { get; set; }

        public ServiceOperation Operation { get; set; }
    }
}
