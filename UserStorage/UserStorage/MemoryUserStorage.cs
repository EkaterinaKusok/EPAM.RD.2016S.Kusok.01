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
        private readonly IGenerator<int> idGenerator = new PrimeIdGenerator();
        private readonly IUserValidator validator = null;
        private readonly IStateSaver saver = new XmlStateSaver();

        private List<User> users = new List<User>();

        public MemoryUserStorage()
        {
        }

        public MemoryUserStorage(IGenerator<int> generator, IUserValidator validator, IStateSaver saver)
        {
            this.idGenerator = generator;
            this.validator = validator;
            this.saver = saver;
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

                // if (!validator.PersonalIdIsValid(user.PersonalId))
                // {
                //    userIsValid = false;
                //    exceptionMessage += " personal ID;";
                // }
                // if (!validator.DateOfBirthIsValid(user.DateOfBirth))
                // {
                //    userIsValid = false;
                //    exceptionMessage += "date of birth;";
                // }
                if (!validator.VisaRecordsAreValid(user.VisaRecords))
                {
                    userIsValid = false;
                    exceptionMessage += " one of visas;";
                }

                if (!userIsValid)
                {
                    throw new UserValidationException(exceptionMessage);
                }
            }

            // if (this.users.Any(u => u.Id == user.Id))
            // {
            //    throw new InvalidOperationException($"User with id={user.Id} already exist");
            // }
            int newId = idGenerator.GenerateNewId();
            users.Add(new User()
            {
                Id = newId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                PersonalId = user.PersonalId,
                VisaRecords = user.VisaRecords
            });
            return newId;
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
        
        public IList<User> SearchForUser(params Func<User, bool>[] predicates)
        {
            if (!users.Any())
            {
                return new List<User>();
            }

            if (predicates.Count() == 0)
            {
                return users.Select(u => u).ToList();
            }

            Func<User, bool> commonPredicate = predicates[0];
            if (predicates.Count() > 1)
            {
                foreach (var predicate in predicates)
                {
                    commonPredicate += predicate;
                }
            }

            return users.Where(commonPredicate).ToList();
        }
    }
}