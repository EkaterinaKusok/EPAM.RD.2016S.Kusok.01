using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.Entities;
using UserStorage.Interfacies;

namespace UserStorage
{
    public class UserStorageInMemory : IUserStorage
    {
        private List<User> users;

        private readonly IGenerator<int> idGenerator = new CustomIdGenerator();
        private readonly IValidator validator = new CustomValidator();

        public UserStorageInMemory()
        {
            users = new List<User>();
        }

        
        public int Add(User user)
        {
            if (!validator.Validate(user))
            {
                throw new ValidationException();
            }
            user.Id = idGenerator.GenerateNewId();
            users.Add(user);
            return user.Id;
        }

        public void Delete(int id)
        {
            var user = users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException( string.Format("User with Id = {0} doesn't exist", id));
            }
            users.RemoveAll(u => u.Id == id);
        }

        public void Delete(User user)
        {
            var removingUser = users.SingleOrDefault(u => u.Equals(user));
            if (removingUser == null)
            {
                throw new InvalidOperationException(string.Format("User with Id = {0} doesn't exist", user.Id));
            }
            users.RemoveAll(u => u.Id == user.Id);
            users.RemoveAll(x => x.Equals(user));
        }

        //public IEnumerable<int> SearchForUser(Func<User, bool> predicate)
        //{
        //    if (!users.Any())
        //        return new int[0];
        //    return users.Where(predicate).Select(u => u.Id);
        //}

        public IEnumerable<int> SearchForUser(params Func<User, bool>[] predicates)
        {
            if (!users.Any())
                return new int[0];
            if (predicates.Count() == 0)
                return users.Select(u => u.Id);
            Func<User, bool> commonPredicate = predicates[0];
            if (predicates.Count() > 1)
                foreach (var predicate in predicates)
                    commonPredicate += predicate;
            return users.Where(commonPredicate).Select(u => u.Id);
        }

        
    }
}

