using System;
using System.Collections.Generic;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.ServiceInfo
{
    [Serializable]
    public class StorageState
    {
        public int CurrentId { get; set; }
        public IEnumerable<User> Users { get; set; }
        
        public StorageState()
        {
            Users = new List<User>();
        }
    }
}
