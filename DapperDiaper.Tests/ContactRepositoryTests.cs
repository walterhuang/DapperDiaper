using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DapperDiaper.Tests
{
    [TestClass]
    public class ContactRepositoryTests
    {
        [TestMethod]
        public void ContactRepository_GetAll_ReturnResults()
        {
            // arrange
            var target = new ContactRepository();

            // act
            var list = target.GetAll();

            // assert
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void ContactRepository_Insert_AssignIdentityToNewEntity()
        {
            // arrange
            var target = new ContactRepository();

            // act
            var list = target.GetAll();

            // assert
            Assert.IsTrue(list.Count > 0);
        }
    }
}
