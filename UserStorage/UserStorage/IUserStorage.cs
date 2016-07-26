using System;
using System.Collections.Generic;
using UserStorage.UserEntities;

namespace UserStorage.UserStorage
{
    public interface IUserStorage
    {
        int Add(User user);
        IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates);
        //void Delete(User user);
        void Delete(int id);

        void Save();
        void Load();
    }
}


