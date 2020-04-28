using Microsoft.VisualStudio.TestTools.UnitTesting;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.UnitTests.AppLogic
{
    [TestClass]
    public class TransactionTests
    {
       
        [TestMethod]
        public void Create_Returns_CompleteObject_AfterCall()
        {
            //Arrange - define testing context
            
            var amount = 420;
            var guid = Guid.Parse("a9e01bc5-ccc5-41dc-948f-089028fa49f5");
            var externalIBAN = "ROIBAN123456799";
            var externalName = "test";
            var details = "details";
            var transaction = Transaction.Create(amount,guid,externalName,externalIBAN,details);
            

            //Act - execute code under test
          

            //Assert - evaluate results
            Assert.AreNotEqual(Guid.Empty, transaction.Id);
            Assert.AreEqual(amount, transaction.Amount);
            Assert.AreEqual(externalIBAN, transaction.ExternalIBAN);
            Assert.AreEqual(externalName, transaction.ExternalName);
            Assert.AreEqual(details, transaction.Details);
            Assert.AreEqual(TransactionStatus.Created,transaction.Status);
            Assert.IsInstanceOfType(transaction.Time, typeof(DateTime));
        }
    }
}
