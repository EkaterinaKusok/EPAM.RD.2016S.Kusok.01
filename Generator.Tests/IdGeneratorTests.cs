using System;
using Generator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Generator.Tests
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
        public void GenerateId_SetCurrentId19GetNextElement_Return23()
        {
            // act
            var generator = new PrimeIdGenerator();
            generator.SetCurrentId(18);
            generator.GenerateNewId();

            // assert
            Assert.AreEqual(23, generator.GetCurrentId());
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void GenerateNewId_IsOverflow_ReturnAnException()
        {
            var generator = new PrimeIdGenerator();
            generator.GenerateNewId(int.MaxValue);
        }
    }
}
