using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.UserEntities;

namespace UserStorage.Tests
{
    [TestClass]
    public class UserEqualsTests
    {
        [TestMethod]
        public void Equals_TwoIdenticalPerson_ReturnTrue()
        {
            User firstUser = new User(0,"Name", "Surname", "", new DateTime(2000, 1, 1), Gender.Male, null);
            User secondUser = new User(0,"Name", "Surname", "", new DateTime(2000, 1, 1), Gender.Male, null);
            var result = firstUser.Equals(secondUser);
            //Console.WriteLine(firstUser.Id+" "+secondUser.Id);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_TwoDifferentPerson_ReturnFalse()
        {
            User firstUser = new User(0,"Name", "Surname", "", new DateTime(2000, 1, 1), Gender.Male, null);
            User thirdUser = new User(0,"OtherName", "Surname", "", new DateTime(2000, 1, 1), Gender.Male, null);
            var result = firstUser.Equals(thirdUser);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_PersonWithHisCopy_ReturnTrue()
        {
            User firstUser = new User(0,"Name", "Surname", "", new DateTime(2000, 1, 1), Gender.Male, null);
            User fourthUser = firstUser;
            var result = firstUser.Equals(fourthUser);
            Assert.IsTrue(result);
        }
    }
}
