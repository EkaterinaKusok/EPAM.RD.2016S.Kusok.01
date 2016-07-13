using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserStorage.Tests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void GenerateNewId_IsFirstElement_ReturnOne()
        {
            var generator = new CustomIdGenerator();
            Assert.AreEqual(1, generator.GenerateNewId());
        }

        [TestMethod]
        public void GenerateNewId_IsSecondElement_ReturnTwo()
        {
            var generator = new CustomIdGenerator();
            generator.GenerateNewId();
            Assert.AreEqual(2, generator.GenerateNewId());
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void GenerateNewId_IsOverflow_ReturnAnException()
        {
            var generator = new CustomIdGenerator();
            for (int i = 0; i < int.MaxValue; i++)
            {
                generator.GenerateNewId();
            }
        }
    }
}
