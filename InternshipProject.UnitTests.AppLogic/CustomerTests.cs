using InternshipProject.ApplicationLogic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.UnitTests.AppLogic
{
    [TestClass]
   public class CustomerTests
    {
        [TestMethod]
        public void Search_BankAccount_For_Given_ID()
        {

            var bankAccount1 = BankAccount.Create("aaaaaaaaaaaaaaaaaaaaaaaa");
            var bankAccount2 = BankAccount.Create("bbbbbbbbbbbbbbbbbbbbbbbb");
            var bankAccount3 = BankAccount.Create("cccccccccccccccccccccccc");
            var bankAccountToFind = BankAccount.Create("aaaaaaaaaaaaaaaaaaaaaaaa");
            var customer = Customer.Create(Guid.NewGuid(), null, null, null);
            List<BankAccount> bankAccounts = new List<BankAccount>();
            customer.BankAccounts.Add(bankAccount1);
            customer.BankAccounts.Add(bankAccount2);
            customer.BankAccounts.Add(bankAccount3);

            BankAccount temp = customer.GetBankAccountByIBAN(bankAccountToFind.IBAN);
                
            
            Assert.AreEqual(temp.IBAN, bankAccountToFind.IBAN);
        }
        [TestMethod]
        public void Checks_If_Create_Returns_Correct_Object()
        {
            string firstName = "Mihnea";
            string lastName = "Holden";
            string socialId = "12312312";
            var customer = Customer.Create(Guid.NewGuid(), firstName, lastName, socialId);
            Assert.AreEqual(firstName, customer.FirstName);
            Assert.AreEqual(lastName, customer.LastName);
            Assert.AreEqual(socialId, customer.SocialId);
            Assert.AreNotEqual(Guid.Empty , customer.Id);

        }
    }
}
