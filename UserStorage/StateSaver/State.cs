using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.UserEntities;

namespace UserStorage.StateSaver
{
    [Serializable]
    public class State
    {
        public int LastId;
        public IEnumerable<User> Users { get; set; }

        public State(IEnumerable<User> users, int lastId)
        {
            this.Users = users.ToList();
            this.LastId = lastId;
        }

        public State()
        {
        }
    }
}
