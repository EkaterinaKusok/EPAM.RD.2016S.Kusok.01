using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Interfacies.Storages;
using UserStorage.Interfacies.UserEntities;
using UserStorage.UserStorage;

namespace UserStorage.Tests
{
    [TestClass]
    public class UserStorageTests
    {
        [TestMethod]
        public void SearchForUser_UserGenderIsFemale_ReturnOneUser()
        {
            var users = new List<User>()
            {
                new User(0, "Name", "Surname", "1", new DateTime(2000, 1, 1), Gender.Male, null),
                new User(0, "Name", "Surname", "2", new DateTime(2000, 1, 1), Gender.Male, null),
                new User(0, "OtherName", "OtherSurname", "3", new DateTime(2000, 1, 1), Gender.Female, null)
            };
            IUserStorage storage = new MemoryUserStorage();
            foreach (var user in users)
            {
                storage.Add(user);
            }

            var allUserIds = storage.SearchForUser().ToArray();
            var foundUserIds = storage.SearchForUser(u => u.Gender == Gender.Female).ToArray();
            Assert.AreEqual(1, foundUserIds.Length);
            Assert.AreEqual(allUserIds[2], foundUserIds[0]);
        }
    }
}
