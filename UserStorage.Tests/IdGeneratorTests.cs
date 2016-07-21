using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Generator;


namespace UserStorage.Tests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void GenerateNewId_IsFirstElement_ReturnOne()
        {
            var generator = new PrimeIdGenerator();
            Assert.AreEqual(1, generator.GenerateNewId());
        }

        [TestMethod]
        public void GenerateNewId_IsSecondElement_ReturnTwo()
        {
            var generator = new PrimeIdGenerator();
            generator.GenerateNewId();
            Assert.AreEqual(2, generator.GenerateNewId());
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void GenerateNewId_IsOverflow_ReturnAnException()
        {

            var generator = new PrimeIdGenerator();
            generator.GenerateNewId(Int32.MaxValue);
        }
    }
}
