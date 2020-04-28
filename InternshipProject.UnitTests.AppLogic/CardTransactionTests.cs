using Microsoft.VisualStudio.TestTools.UnitTesting;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.UnitTests.AppLogic
{
   
    [TestClass]
    public class CardTransactionTests
    {
        [TestMethod]
        public void TestCardTransaction()
        {
            
           
            var transaction = Transaction.Create(10, Guid.Parse("FAA596E7-B394-47D1-B7A2-47502FE2B400"), "name", "iban", "details");
            var cardtransaction = CardTransaction.Create(transaction, CardTransactionType.POS);
            Assert.AreNotEqual(Guid.Empty, cardtransaction.Id);
            Assert.AreEqual(transaction, cardtransaction.Transaction);
            Assert.AreEqual(CardTransactionType.POS, cardtransaction.TransactionType);
        }
    }
}
