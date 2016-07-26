using System;
using System.Collections.Generic;
using UserStorage.UserEntities;

namespace UserStorage.StateSaver
{
    [Serializable]
    public class UserState
    {
        public int CurrentId { get; set; }
        public IEnumerable<User> Users { get; set; }
        
        public UserState()
        {
            Users = new List<User>();
        }
    }
}
