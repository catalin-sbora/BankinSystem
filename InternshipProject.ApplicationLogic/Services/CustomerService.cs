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
        public CustomerService(IPersistenceContext persistenceContext)
        {
            customerRepository = persistenceContext.CustomerRepository;
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
    }
}
