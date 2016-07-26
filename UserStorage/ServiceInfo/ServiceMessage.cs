using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.UserEntities;

namespace UserStorage.ServiceInfo
{
    [Serializable]
    public class ServiceMessage
    {
        public User User { get; set; }
        public ServiceOperation Operation { get; set; }

        public ServiceMessage() { }

        public ServiceMessage(User user, ServiceOperation operation)
        {
            User = user;
            Operation = operation;
        }
    }
}
