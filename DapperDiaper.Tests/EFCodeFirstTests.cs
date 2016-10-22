using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using DapperDiaper.Models;

namespace DapperDiaper.Tests
{
    [TestClass]
    public class EFCodeFirstTests
    {
        private Fixture _fixture = new Fixture();

        [TestMethod]
        public void TestMethod1()
        {
            Contact contact = _fixture.Create<Contact>();
        }
    }
}
