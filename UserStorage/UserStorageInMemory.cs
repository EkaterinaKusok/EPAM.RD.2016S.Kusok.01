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

        public UserStorageInMemory(IGenerator<int> generator)
        {
            if (generator != null)
                this.idGenerator = generator;
            users = new List<User>();
        }

        public int Add(User user, IUserValidator validator = null)
        {
            if (validator != null)
            {
                string exceptionMessage = "Unvalid parameters:";
                bool userIsValid = true;
                if (!validator.FirstNameIsValid(user.FirstName))
                {
                    userIsValid = false;
                    exceptionMessage += " first name;";
                }
                if (!validator.LastNameIsValid(user.LastName))
                {
                    userIsValid = false;
                    exceptionMessage += " last name;";
                }
                //if (!validator.PersonalIdIsValid(user.PersonalId))
                //{
                //    userIsValid = false;
                //    exceptionMessage += " personal ID;";
                //}
                if (!validator.DateOfBirthIsValid(user.DateOfBirth))
                {
                    userIsValid = false;
                    exceptionMessage += "date of birth;";
                }
                if (!validator.VisaRecordsAreValid(user.VisaRecords))
                {
                    userIsValid = false;
                    exceptionMessage += " one of visas;";
                }
                if (!userIsValid)
                    throw new UserValidationException(exceptionMessage);
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

        public IEnumerable<int> AddUsers(IEnumerable<User> newUsers)
        {
            var userIds = new List<int>();
            foreach (var user in newUsers)
            {
                userIds.Add(this.Add(user));
            }
            return userIds;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return users.Select(u => u);
        }

        public void DeleteAllUsers()
        {
            users.Clear();
        }

        public void SetCurrentId(int currentId)
        {
            idGenerator.SetCurrentId(currentId);
        }
    }
}

