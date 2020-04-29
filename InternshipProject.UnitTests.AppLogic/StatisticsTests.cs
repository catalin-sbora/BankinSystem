using InternshipProject.ApplicationLogic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InternshipProject.ApplicationLogic.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using InternshipProject.ApplicationLogic.Abstractions;

namespace InternshipProject.UnitTests.AppLogic
{
    [TestClass]
    public class StatisticsTests
    {
        private Mock<ICustomerRepository> customerRepositoryMock = new Mock<ICustomerRepository>();
        private Mock<IPersistenceContext> persistenceContextMock = new Mock<IPersistenceContext>();
        private StatisticsService statisticsService;

        private decimal amountPerTransaction;
        private decimal initialBalance;
        private string externalName;
        private string externalBankIBAN;
        private string bankAccountIBAN;
        private string details;
        private BankAccount[] bankAccounts;
        private int bankAccountAmount;

        private Customer testCustomer;        
        private Guid userId;
        private string firstName;
        private string lastName;
        private string socialId;

        [TestInitialize]
        public void InitializeTestData()
        {
            persistenceContextMock.Setup(pcontext => pcontext.CustomerRepository)
                                  .Returns(customerRepositoryMock.Object);
            statisticsService = new StatisticsService(persistenceContextMock.Object);

            userId = Guid.NewGuid();
            firstName = "FirstName";
            lastName = "LastName";
            socialId = "1111111111";

            testCustomer = Customer.Create(userId, firstName, lastName, socialId);

            initialBalance = 1000;
            externalName = "ExternalName";
            bankAccountIBAN = "ROIBAN00299456453345324";
            externalBankIBAN = "ROIBAN001234567890123456";
            details = "Details";

            bankAccountAmount = 3;

            bankAccounts = new BankAccount[bankAccountAmount];

            for (int i = 0; i < bankAccountAmount; i++)
            {
                amountPerTransaction = 100 * (i + 1);
                bankAccounts[i] = BankAccount.Create(bankAccountIBAN + i);
                testCustomer.BankAccounts.Add(bankAccounts[i]);
                bankAccounts[i].Balance = initialBalance;

                while (bankAccounts[i].Balance > amountPerTransaction)
                {
                    bankAccounts[i].CreatePayment(amountPerTransaction, externalName, externalBankIBAN, details);
                }
            }
        }

        [TestMethod]
        public void GetIndexListForTransaction_Gets_Correct_List()
        {
            customerRepositoryMock.Setup(cr => cr.GetCustomerByUserId(userId))
                             .Returns(testCustomer);

            IEnumerable<int> indexList = statisticsService.GetIndexListForTransactions(userId.ToString());

            Assert.IsNotNull(indexList);

            int indexCount = 0;
            foreach (int index in indexList)
            {
                indexCount++;
            }

            foreach(BankAccount bankAccount in bankAccounts)
            {
                Assert.IsTrue(bankAccount.Transactions.Count <= indexCount, 
                    $"Transactions count {bankAccount.Transactions.Count} is greater than {indexCount}");
            }
        }


        [TestMethod]
        public void ProcessBalanceHistory_Processes_Correctly()
        {
            /// Act
            IEnumerable<decimal> allTimeHistory = statisticsService.BankAccountHistoryAllTime(bankAccounts[0]);

            /// Assert
            Assert.IsNotNull(allTimeHistory);
            
            decimal lastAmount = initialBalance;
            int numberOfTransactions = -1;

            foreach (var transactionAmount in allTimeHistory)
            {
                Assert.IsTrue(lastAmount >= transactionAmount);
                lastAmount = transactionAmount;
                numberOfTransactions++;
            }

            Assert.AreEqual(numberOfTransactions, bankAccounts[0].Transactions.Count);
        }
    }
}
