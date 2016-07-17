using System;
using System.Collections.Generic;
using System.Linq;

namespace UserStorage.Entities
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
