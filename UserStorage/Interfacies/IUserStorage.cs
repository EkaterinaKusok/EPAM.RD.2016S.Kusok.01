using System;
using System.Collections.Generic;
using UserStorage.Entities;

namespace UserStorage.Interfacies
{
    public interface IUserStorage
    {
        int Add(User user, IUserValidator validator = null);
        IEnumerable<int> SearchForUser(params Func<User, bool>[] predicates);
        void Delete(User user);
        void Delete(int id);

        IEnumerable<User> GetAllUsers();
        IEnumerable<int> AddUsers(IEnumerable<User> users);
        void DeleteAllUsers();
        void SetCurrentId(int currentId);
    }
}


