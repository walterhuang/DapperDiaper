using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DapperDiaper.Models;
using Ploeh.AutoFixture;

namespace DapperDiaper.Tests
{
    [TestClass]
    public class ContactRepositoryTests
    {
        private static Fixture _fixture = new Fixture();
        private static int _id;

        [TestMethod]
        public void ContactRepository_Insert_AssignIdentityToNewEntity()
        {
            // arrange
            var target = new ContactRepository();
            var contact = _fixture.Build<Contact>().Without(c => c.Id).With(c => c.FirstName, "walter").Create();

            // act
            target.Add(contact);

            // assert
            Assert.IsTrue(contact.Id > 0);
            _id = contact.Id;
        }

        [TestMethod]
        public void ContactRepository_Find_RetrieveExistingEntity()
        {
            // arrange
            var target = new ContactRepository();

            // act
            var contact = target.Find(_id);

            // assert
            Assert.IsNotNull(contact);
            Assert.AreEqual("walter", contact.FirstName);
        }

        [TestMethod]
        public void ContactRepository_Modify_UpdateExistingEntity()
        {
            // arrange
            var target = new ContactRepository();
            var contact = target.Find(_id);

            // act
            contact.FirstName = "ppap";
            target.Update(contact);
            var contact2 = new ContactRepository().Find(_id);

            // assert
            Assert.AreEqual("ppap", contact2.FirstName);
        }

        [TestMethod]
        public void ContactRepository_Delete_RemoveEntity()
        {
            // arrange
            var target = new ContactRepository();

            // act
            target.Remove(_id);
            var contact = new ContactRepository().Find(_id);

            // assert
            Assert.IsNull(contact);
        }
    }
}
