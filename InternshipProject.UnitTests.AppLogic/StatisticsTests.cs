using InternshipProject.ApplicationLogic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InternshipProject.ApplicationLogic.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.UnitTests.AppLogic
{
    [TestClass]
    public class StatisticsTests
    {
        [TestMethod]
        public void ProcessBalanceHistory_Processes_Correctly()
        {
            /// Arrange
            var bankAccount = BankAccount.Create("ROIBAN002994564533453245");
            var externalBankAccount = BankAccount.Create("ROIBAN001234567890123456");

            decimal initialBalance = 1000;
            bankAccount.Balance = initialBalance;

            decimal amount = 100;
            string externalName = "GenericName";
            string externalIBAN = externalBankAccount.IBAN;
            string details = "";
        
            while (bankAccount.Balance > amount)
            {
               bankAccount.CreatePayment(amount, externalName, externalIBAN, details);
            }

            /// Act
            
            IEnumerable<decimal> allTimeHistory = new StatisticsServices(null).BankAccountHistoryAllTime(bankAccount);

            /// Assert
            Assert.IsNotNull(allTimeHistory);
            
            decimal lastAmount = initialBalance;
            int numberOfTransactions = -1;
            foreach (var transactionAmount in allTimeHistory)
            {
                Assert.IsTrue(lastAmount >= transactionAmount + amount);
                lastAmount = transactionAmount;
                numberOfTransactions++;
            }

            Assert.AreEqual(numberOfTransactions, bankAccount.Transactions.Count);
            
        }
    }
}
