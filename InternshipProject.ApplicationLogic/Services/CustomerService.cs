using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class CustomerService
    {
        private ICustomerRepository customerRepository;
        private IPersistenceContext persistContext;
        public CustomerService(IPersistenceContext persistenceContext)
        {
            customerRepository = persistenceContext.CustomerRepository;
            persistContext = persistenceContext;
        }
        public Customer GetCustomerFromUserId(string userId)
        {
            Guid idToSearch = Guid.Parse(userId);            
            var customer = customerRepository?.GetCustomerByUserId(idToSearch);
            if (customer == null)
            {
                throw new CustomerNotFoundException(userId);
            }

            return customer;
        }

        public Customer RegisterNewCustomer(string userId, string firstName, string lastName, string socialId)
        {
           var customer = Customer.Create(Guid.Parse(userId), firstName, lastName, socialId);
           customer = customerRepository.Add(customer);
           persistContext.SaveChanges();
           return customer;

        }
    }
}
