using System;
using System.Collections.Generic;
using UserStorage.UserEntities;
using UserStorage.Validator;

namespace UserStorage.UserStorage
{
    public interface IUserStorage
    {
        int Add(User user, IUserValidator validator = null);
        IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates);
        void Delete(User user);
        void Delete(int id);
    }
}


