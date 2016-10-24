using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using DapperDiaper.Models;

namespace DapperDiaper.Tests
{
    // Unit Test Naming Convention
    // [UnitOfWork_StateUnderTest_ExpectedBehavior]
    // http://stackoverflow.com/questions/155436/unit-test-naming-best-practices

    [TestClass]
    public class EFCodeFirstTests
    {
        private static Fixture _fixture = new Fixture();
        private static int _id;

        [TestMethod]
        public void EFCodeFirst_Insert_AssignIdentityToNewEntity()
        {
            // arrange
            var target = new ContactsDB();
            var contact = _fixture.Build<Contact>().Without(c => c.Id).With(c => c.FirstName, "walter").Create();

            // act
            target.Contacts.Add(contact);
            target.SaveChanges();

            // assert
            Assert.IsTrue(contact.Id > 0);
            _id = contact.Id;
        }

        [TestMethod]
        public void EFCodeFirst_Find_RetrieveExistingEntity()
        {
            // arrange
            var target = new ContactsDB();

            // act
            var contact = target.Contacts.Find(_id);

            // assert
            Assert.AreEqual("walter", contact.FirstName);
        }

        [TestMethod]
        public void EFCodeFirst_Modify_UpdateExistingEntity()
        {
            // arrange
            var target = new ContactsDB();
            var contact = target.Contacts.Find(_id);

            // act
            contact.FirstName = "ppap";
            target.SaveChanges();
            var contact2 = new ContactsDB().Contacts.Find(_id);

            // assert
            Assert.AreEqual("ppap", contact2.FirstName);
        }

        [TestMethod]
        public void EFCodeFirst_Delete_RemoveEntity()
        {
            // arrange
            var target = new ContactsDB();
            var contact = target.Contacts.Find(_id);

            // act
            target.Contacts.Remove(contact);
            target.SaveChanges();
            var contact2 = new ContactsDB().Contacts.Find(_id);

            // assert
            Assert.IsNull(contact2);
        }
    }
}
