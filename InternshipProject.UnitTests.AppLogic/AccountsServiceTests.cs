using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.UnitTests.AppLogic
{
    [TestClass]
    public class AccountsServiceTests
    {
        private Mock<IPersistenceContext> persistenceContextMock = new Mock<IPersistenceContext>();
        private Mock<ICustomerRepository> customerRepositoryMock = new Mock<ICustomerRepository>();
        private CustomerService customerService;
        private AccountsService accountsService;

        //test data
        private Guid userId;
        private Customer testCustomer;
        private BankAccount baOne;
        private BankAccount baTwo;
        private BankAccount baThree;


        [TestInitialize]
        public void InitializeTestData()
        {

            persistenceContextMock.Setup( pc => pc.CustomerRepository)
                                  .Returns(customerRepositoryMock.Object);
            customerService = new CustomerService(persistenceContextMock.Object);
            accountsService = new AccountsService(persistenceContextMock.Object, customerService);

            userId = Guid.NewGuid();

            testCustomer = Customer.Create(userId, "MyFirstName", "MyLastName", "8877474");
            baOne = BankAccount.Create("ROIBAN009399838334343344");
            baTwo = BankAccount.Create("ROIBAN009399838334344444");
            baThree = BankAccount.Create("ROIBAN009399838334345544");

            testCustomer.BankAccounts.Add(baOne);
            testCustomer.BankAccounts.Add(baTwo);
            testCustomer.BankAccounts.Add(baThree);
        }
        [TestMethod]
        public void GetCustomerBankAccount_Throws_AccountNotFoundException_WhenAccountDoesNotExistForCustomer()
        {
            //Arrange            
            customerRepositoryMock.Setup(cr => cr.GetCustomerByUserId(userId))
                                  .Returns(testCustomer);

            //Assert
            Assert.ThrowsException<AccountNotFoundException>(()=> {
                //Act
                accountsService.GetCustomerBankAccount(userId.ToString(), Guid.NewGuid());
            });        
        
        }

        [TestMethod]
        public void GetCustomerBankAccount_Returns_CorrectBankAccount()
        {
            //Arrange
            customerRepositoryMock.Setup(cr => cr.GetCustomerByUserId(userId))
                                  .Returns(testCustomer);

            //Act
            var baResult = accountsService.GetCustomerBankAccount(userId.ToString(), baTwo.Id);

            //Assert
            Assert.IsNotNull(baResult);
            Assert.AreEqual(baTwo.Id, baResult.Id);
            Assert.AreEqual(baTwo.IBAN, baResult.IBAN);

        }

    }
}
