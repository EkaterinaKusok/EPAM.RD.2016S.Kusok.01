using System;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.ServiceInfo
{
    public class UserEventArgs : EventArgs
    {
        public UserEventArgs(User user, ServiceOperation operation)
        {
            this.User = user;
            this.Operation = operation;
        }

        public User User { get; }

        public ServiceOperation Operation { get; }
    }
}
