using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.UnitTests.AppLogic
{
    [TestClass]
    public class ReceivedServiceTests
    {
        private Mock<IPersistenceContext> persistenceContextMock = new Mock<IPersistenceContext>();
        private Mock<ICustomerRepository> customerRepositoryMock = new Mock<ICustomerRepository>();
        private Mock<ITransactionRepository> transactionMock = new Mock<ITransactionRepository>();
        private Mock<ReceivedService> receivedServiceMock = new Mock<ReceivedService>();
        private ReceivedService receivedService;
        private CustomerService customerService;
        private AccountsService accountsService;
        private Guid userId  = Guid.NewGuid();
        private Customer testCustomer;
        private BankAccount baOne;
       
        [TestInitialize]
        public void InitializeTestData()
        {
            persistenceContextMock.Setup(pc => pc.CustomerRepository)
                                 .Returns(customerRepositoryMock.Object);
            customerService = new CustomerService(persistenceContextMock.Object);
            receivedService = new ReceivedService(persistenceContextMock.Object, customerService);
            testCustomer = Customer.Create(userId, "MyFirstName", "MyLastName", "8877474");
            baOne = BankAccount.Create("ROIBAN009399838334343344");
            testCustomer.BankAccounts.Add(baOne);
            var amount = 420;
            var externalIBAN = "ROIBAN123456799";
            var externalName = "test";
            var details = "details";
            baOne.CreateReceive(amount, externalName, externalIBAN, details);
            //baOne.CreatePayment(amount, externalName, externalIBAN, details);
            
           
        }
        [TestMethod]
        public void GetCustomerTransaction_Adds_Correct_Transaction()
        {

            //Arrange
           
            customerRepositoryMock.Setup(cr => cr.GetCustomerByUserId(userId))
                                  .Returns(testCustomer);
            //receivedServiceMock.Setup(cr => cr.GetCustomerTransaction(userId.ToString()))
                          //  .Returns(testCustomer.BankAccounts.ElementAt(0).Transactions);
            var result = receivedService.GetCustomerTransaction(userId.ToString());
            Assert.AreEqual(result.ElementAt(0), baOne.Transactions.AsEnumerable().ElementAt(0));

        }
    }
}
