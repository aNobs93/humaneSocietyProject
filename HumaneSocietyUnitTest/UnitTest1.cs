using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HumaneSociety;

namespace HumaneSocietyUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Animal animal = new Animal();
            //arrange
            string furryFriend = "bat";
            string expected;
            string actual;
            //act
            Query.AddAnimal();
            //assert

        }
    }
}
