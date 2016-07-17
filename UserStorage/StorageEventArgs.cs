using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage
{
    public class StorageEventArgs : EventArgs
    {
        public User User { get; private set; }
        public int LastId { get; private set; }
        public bool IsDelete { get; private set; }

        public StorageEventArgs(User user, int lastId, bool isDelete)
        {
            this.User = user;
            this.LastId = lastId;
            this.IsDelete = isDelete;
        }
    }
}
