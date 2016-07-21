﻿using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.UserEntities;
using UserStorage.Generator;
using UserStorage.UserStorage;
using UserStorage.Validator;

namespace UserStorage
{
    [Serializable]
    public class MemoryUserStorage : IUserStorage
    {
        private List<User> users;

        private readonly IGenerator<int> idGenerator = new PrimeIdGenerator();

        public MemoryUserStorage(IGenerator<int> generator = null)
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