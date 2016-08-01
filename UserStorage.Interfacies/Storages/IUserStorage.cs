using System;
using System.Collections.Generic;
using UserStorage.Interfacies.UserEntities;

namespace UserStorage.Interfacies.Storages
{
    public interface IUserStorage
    {
        int Add(User user);

        IList<User> SearchForUser(params Func<User, bool>[] predicates);

        // void Delete(User user);
        void Delete(int id);

        void Save();

        void Load();
    }
}
