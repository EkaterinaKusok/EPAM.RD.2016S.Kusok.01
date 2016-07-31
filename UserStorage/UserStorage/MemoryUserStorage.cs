using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
using UserStorage.Interfacies.Generators;
using UserStorage.Interfacies.ServiceInfo;
using UserStorage.Interfacies.StateSavers;
using UserStorage.Interfacies.Storages;
using UserStorage.Interfacies.UserEntities;
using UserStorage.Interfacies.Validators;
using UserStorage.StateSaver;
using UserStorage.Validator;

namespace UserStorage.UserStorage
{
    [Serializable]
    public class MemoryUserStorage : IUserStorage
    {
        private List<User> users;

        private readonly IGenerator<int> idGenerator = new PrimeIdGenerator();
        private readonly IUserValidator validator = null;
        private readonly IStateSaver saver = new XmlStateSaver();

        public MemoryUserStorage()
        {
            users = new List<User>();
        }

        public MemoryUserStorage(IGenerator<int> generator, IUserValidator validator, IStateSaver saver)
        {
            this.idGenerator = generator;
            this.validator = validator;
            this.saver = saver;
            users = new List<User>();
        }

        public int Add(User user)
        {
            if ((object)user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            
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
            //if (this.users.Any(u => u.Id == user.Id))
            //{
            //    throw new InvalidOperationException($"User with id={user.Id} already exist");
            //}

            int newId = idGenerator.GenerateNewId();
            users.Add(new User()
            {
                Id = newId,
                FirstName = user.FirstName,
                LastName = user.FirstName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                PersonalId = user.PersonalId,
                VisaRecords = user.VisaRecords
            });
            return user.Id;
        }

        public void Delete(int id)
        {
            var user = users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException($"User with Id = {id} doesn't exist");
            }
            users.RemoveAll(u => u.Id == id);
        }

        public void Save()
        {
            var state = new StorageState()
            {
                Users = this.users,
                CurrentId = this.idGenerator.GetCurrentId()
            };
            this.saver.SaveState(state);
        }

        public void Load()
        {
            var state = this.saver.LoadState();
            this.users = state.Users.ToList();
            this.idGenerator.SetCurrentId(state.CurrentId);
        }

        //public void Delete(User user)
        //{
        //    var removingUser = users.SingleOrDefault(u => u.Equals(user));
        //    if (removingUser == null)
        //    {
        //        throw new InvalidOperationException(string.Format("User with Id = {0} doesn't exist", user.Id));
        //    }
        //    users.RemoveAll(u => u.Id == user.Id);
        //    users.RemoveAll(x => x.Equals(user));
        //}
        
        public IEnumerable<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            if (!users.Any())
                return new User[0];
            if (predicates.Count() == 0)
                return users.Select(u => u);
            Func<User, bool> commonPredicate = predicates[0];
            if (predicates.Count() > 1)
                foreach (var predicate in predicates)
                    commonPredicate += predicate;
            return users.Where(commonPredicate);
        }

        //public IEnumerable<User> GetAllUsers()
        //{
        //    return users.Select(u => u);
        //}

        //public void DeleteAllUsers()
        //{
        //    users.Clear();
        //}

        //public void SetCurrentId(int currentId)
        //{
        //    idGenerator.SetCurrentId(currentId);
        //}

    }
}