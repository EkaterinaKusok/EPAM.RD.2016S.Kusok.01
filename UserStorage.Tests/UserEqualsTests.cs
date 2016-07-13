using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Entities;

namespace UserStorage.Tests
{
    [TestClass]
    public class UserEqualsTests
    {
        User firstUser = new User(1,"Name","Surname", new DateTime(2000,1,1), Gender.Male, null);
        User secondUser = new User(2, "Name", "Surname", new DateTime(2000, 1, 1), Gender.Male, null);
        User thirdUser = new User(3, "OtherName", "Surname", new DateTime(2000, 1, 1), Gender.Male, null);

        [TestMethod]
        public void Equals_TwoIdenticalPerson_ReturnTrue()
        {
            var result = firstUser.Equals(secondUser);
            //Console.WriteLine(firstUser.Id+" "+secondUser.Id);
            Assert.IsTrue(result);
        }
    }
}
