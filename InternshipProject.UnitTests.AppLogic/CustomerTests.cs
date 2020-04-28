using InternshipProject.ApplicationLogic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InternshipProject.ApplicationLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.UnitTests.AppLogic
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]

        public void SetContactDetails_Sets_ContactDetailsCorrectly()
        {
            Guid userId = Guid.NewGuid();
            string firstName = "FirstName";
            string lastName = "LastName";
            string socialId = "111111111111";

            Customer customer = Customer.Create(userId, firstName, lastName, socialId);

            string address = "NewAddress";
            string city = "NewCity";
            string country = "NewCountry";
            string phoneNo = "NewPhoneNo";
            string altPhoneNo = "NewAltPhoneNo";
            string email = "NewEmail";

            customer.SetContactDetails(address, city, country, phoneNo, altPhoneNo, email);

            Assert.AreEqual(address, customer.ContactDetails.Address);
            Assert.AreEqual(city, customer.ContactDetails.City);
            Assert.AreEqual(country, customer.ContactDetails.Country);
            Assert.AreEqual(phoneNo, customer.ContactDetails.PhoneNo);
            Assert.AreEqual(altPhoneNo, customer.ContactDetails.AlternatePhoneNo);
            Assert.AreEqual(email, customer.ContactDetails.Email);
        }

        [TestMethod]
        public void GetAccount_Gets_RightAccount()
        {
            Guid userId = Guid.NewGuid();
            string firstName = "FirstName";
            string lastName = "LastName";
            string socialId = "111111111111";

            Customer customer = Customer.Create(userId, firstName, lastName, socialId);
            var bankAccount = BankAccount.Create("ROIBAN002994564533453245");
            var secondBankAccount = BankAccount.Create("ROIBAN001234567890123456");

            customer.BankAccounts.Add(bankAccount);
            customer.BankAccounts.Add(secondBankAccount);

            decimal initialBalance = 1000;
            bankAccount.Balance = initialBalance;

            var gottenAccount = customer.GetAccount(bankAccount.Id);

            Assert.IsNotNull(gottenAccount);
            Assert.IsInstanceOfType(gottenAccount, typeof(BankAccount));
            Assert.AreEqual(gottenAccount.Balance, bankAccount.Balance);
            Assert.AreEqual(gottenAccount.Currency, bankAccount.Currency);
            Assert.AreEqual(gottenAccount.IBAN, bankAccount.IBAN);
            Assert.AreEqual(gottenAccount.Id, bankAccount.Id);
            Assert.AreEqual(gottenAccount.Transactions.Count, bankAccount.Transactions.Count);
        }

        [TestMethod]
        public void GetAccount_Throws_AccountNotFoundException_When_AccountId_IsWrong()
        {

            Guid userId = Guid.NewGuid();
            string firstName = "FirstName";
            string lastName = "LastName";
            string socialId = "111111111111";

            Customer customer = Customer.Create(userId, firstName, lastName, socialId);

            var foreignBankAccount = BankAccount.Create("ROIBAN001234567890120000");

            Assert.ThrowsException<AccountNotFoundException>(
                () => {
                    var gottenAccount = customer.GetAccount(foreignBankAccount.Id);
                });
        }

        [TestMethod]
        public void MakePayment_Returns_Completed_Transaction()
        {
            Guid userId = Guid.NewGuid();
            string firstName = "FirstName";
            string lastName = "LastName";
            string socialId = "111111111111";

            Customer customer = Customer.Create(userId, firstName, lastName, socialId);

            var bankAccount = BankAccount.Create("ROIBAN001234567890120000");
            var externalBankAccount = BankAccount.Create("ROIBAN001234567890123456");
            
            customer.BankAccounts.Add(bankAccount);
            decimal initialBalance = 1000;
            bankAccount.Balance = initialBalance;

            var sourceAccountId = bankAccount.Id;
            decimal payAmount = 100;
            string destinationName = "DestinationName";
            string destinationIBAN = externalBankAccount.IBAN;
            string details = "Details";

            Transaction returnedTransaction = customer.MakePayment(sourceAccountId, payAmount, destinationName, destinationIBAN, details);

            Assert.IsNotNull(returnedTransaction);
            Assert.AreEqual(bankAccount.Balance, initialBalance - payAmount);
        }

        [TestMethod]
        public void NotifyTransaction_Throws_AccountNotFoundException_When_AccountNotFound()
        {

            Guid userId = Guid.NewGuid();
            string firstName = "FirstName";
            string lastName = "LastName";
            string socialId = "111111111111";

            Customer receiverCustomer = Customer.Create(userId, firstName, lastName, socialId);
            Customer senderCustomer = Customer.Create(userId, firstName, lastName, socialId);

            var bankAccount = BankAccount.Create("ROIBAN001234567890120000");
            var externalBankAccount = BankAccount.Create("ROIBAN001234567890123456");
            
            receiverCustomer.BankAccounts.Add(bankAccount);
            
            var amount = 100;
            externalBankAccount.Balance = amount;

            Transaction sendingTransaction = externalBankAccount.CreatePayment(amount, "Name", bankAccount.IBAN, "Details");

            Assert.ThrowsException<AccountNotFoundException>(
                () => {
                    receiverCustomer.NotifyTransaction(sendingTransaction, senderCustomer);
                });
        }
    }
}
