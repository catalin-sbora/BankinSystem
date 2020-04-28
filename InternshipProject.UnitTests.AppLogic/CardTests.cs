using InternshipProject.ApplicationLogic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace InternshipProject.UnitTests.AppLogic
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void Create_Returns_CompleteObject_AfterCall()
        {
            //Arrange - define testing context
            var bankAccount = BankAccount.Create("ROIBAN002994564533453245");
            var cardNumber = "35443534534345";
            var cardCVV = "234";
            var cardOwner = "Owner Name";

            //Act - execute code under test
            var card = Card.Create(cardNumber, cardCVV, cardOwner, bankAccount);

            //Assert - evaluate results
            Assert.AreNotEqual(Guid.Empty, card.Id);
            Assert.AreEqual(cardNumber, card.SerialNumber);
            Assert.AreEqual(cardCVV, card.CVV);
            Assert.IsNotNull(card.BankAccount);
            Assert.IsInstanceOfType(card.CreateDate, typeof(DateTime));
        }
           
        [TestMethod]
        public void Create_POS_Payment()
        {
            var amount = 50;
            BankAccount BankAccount = BankAccount.Create("aaaaaaaaaaaaaaaaaaaaaaaa");
            var destinationName = "Mihnea";
            var destinationAccount = "David";
            var transaction = Transaction.Create(amount, Guid.NewGuid(), destinationAccount, destinationAccount, null);
            var cardTransactionType = CardTransactionType.POS;
            var cardTransaction = CardTransaction.Create(transaction, cardTransactionType);
            Assert.AreEqual(transaction, cardTransaction.Transaction);
            Assert.AreEqual(cardTransactionType, CardTransactionType.POS);
        }
    }
   
}
