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
    public class CustomerServiceTests
    {
        private Mock<ICustomerRepository> customerRepositoryMock = new Mock<ICustomerRepository>();
        private Mock<IPersistenceContext> persistenceContextMock = new Mock<IPersistenceContext>();
        private CustomerService customerService;
        private Guid userId;
        private Customer testCustomer;
        private string firstName;
        private string lastName;
        private string socialId;
        [TestInitialize]
        public void Initialize_Data_Tests()
        {
            persistenceContextMock.Setup(pc => pc.CustomerRepository)
                                 .Returns(customerRepositoryMock.Object);
            customerService = new CustomerService(persistenceContextMock.Object);
            userId = Guid.NewGuid();
            firstName = "Toma";
            lastName = "Mihnea";
            socialId = "312312";
            testCustomer = Customer.Create(userId, firstName, lastName, socialId);

        }
        [TestMethod]
        public void GetCustomerFromUserId_Throws_Exception_When_Customer_Does_Not_Exist()
        {
            customerRepositoryMock.Setup(b => b.GetCustomerByUserId(userId)).Returns(testCustomer);
            Assert.ThrowsException<CustomerNotFoundException>(() =>
            {
                customerService.GetCustomerFromUserId(Guid.NewGuid().ToString());

            });
        }
        [TestMethod]
        public void GetCustomerFromUserId_Returns_Correct_Customer_Object()
        {
            customerRepositoryMock.Setup(b => b.GetCustomerByUserId(userId)).Returns(testCustomer);
            var customer = customerService.GetCustomerFromUserId(userId.ToString());
            Assert.IsNotNull(customer);
            Assert.AreEqual(userId, customer.UserId);
            Assert.AreEqual(firstName, customer.FirstName);
            Assert.AreEqual(lastName, customer.LastName);
            Assert.AreEqual(socialId, customer.SocialId);
        }
    }
}
