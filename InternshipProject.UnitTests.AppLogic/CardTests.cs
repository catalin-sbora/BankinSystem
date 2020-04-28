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
        public void WithdrawFromATM_Returns_CardTransaction_AfterCall()
        {
            //Arrange
            var bankAccount = BankAccount.Create("ROIBAN002994564533453245");
            bankAccount.CreateReceive(20, "asd", "ROIBAN002994564533453245", "");
            var card = Card.Create("12312312", "333", "ASdasd asdasd", bankAccount);
            //Act

            var cardTransaction = card.WithdrawFromATM(10, "Atm");
            //Assert
            Assert.IsNotNull(cardTransaction.Transaction);
            Assert.AreEqual(CardTransactionType.ATM, cardTransaction.TransactionType);
        }

        [TestMethod]
        public void OnlinePayment_Returns_CardTransaction_AfterCall()
        {
            //Arrange
            var bankAccount = BankAccount.Create("ROIBAN002994564533453245");
            bankAccount.CreateReceive(20, "asd", "ROIBAN002994564533453245", "");

            var cvv = "333";
            var card = Card.Create("123123123", cvv, "ASdasd asdasd", bankAccount);

            //Act
            var cardTransaction = card.OnlinePayment(10, "Magazin", "asdads", cvv);

            //Assert
            Assert.AreEqual(card.CVV, cvv);
            Assert.IsNotNull(cardTransaction.Transaction);
            Assert.AreEqual(CardTransactionType.Online, cardTransaction.TransactionType);
        }
    }
   
}
