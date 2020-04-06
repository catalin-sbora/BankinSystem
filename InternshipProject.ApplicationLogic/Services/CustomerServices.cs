using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class CustomerServices
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerServices(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public Guid GetCustomerIdFromUserId(string userId)
        {
            var idToSearch = Guid.Parse(userId);
            var foundCustomer =  customerRepository?.GetCustomerByUserId(idToSearch);
            if (foundCustomer == null)
            {
                throw new CustomerNotFoundException(userId);
            }
            return foundCustomer.Id;

        }
        public IEnumerable<BankAccount> GetCustomerBankAccounts(Guid customerId)
        {
            var customer = customerRepository?.GetCustomerById(customerId);
            if (customer == null)
            {
                throw new CustomerNotFoundException(customerId);
            }
            
            return customer.BankAccounts
                            .AsEnumerable();           
        }
    }
}
