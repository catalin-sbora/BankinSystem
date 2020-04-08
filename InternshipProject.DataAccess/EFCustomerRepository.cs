using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFCustomerRepository : ICustomerRepository
    {
        private readonly BankingDbContext dbContext;
        public EFCustomerRepository(BankingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Customer Add(Customer customer)
        {
            var addedEntity = dbContext.Add(customer);
            dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public IEnumerable<Customer> FindByLastName(string lastName)
        {
            var customersList = dbContext.Customers
                                .Where(customer => 
                                            customer.LastName
                                            .ToLower()
                                            .Contains(lastName.ToLower()));

            return customersList.AsEnumerable();
        }

        public IEnumerable<Customer> GetAll()
        {
            var customers  = dbContext.Customers.AsEnumerable();
            return customers;
        }

        public Customer GetCustomerById(Guid customerId)
        {
            var foundCustomer = dbContext.Customers
                               .Where(customer => customer.Id == customerId)
                               .FirstOrDefault();
            return foundCustomer;

        }

        public Customer GetCustomerByUserId(Guid userId)
        {
            var foundCustomer = dbContext.Customers  
                               /* .Include(c => c.ContactDetails)
                                .Include(c => c.BankAccounts)
                                .Include(c => c.BankAccounts.Select(ba => ba.Transactions))*/
                                .Where(customer => customer.UserId == userId)
                                .FirstOrDefault();
            return foundCustomer;
        }
        public bool Remove(Guid customerId)
        {
            var customer = GetCustomerById(customerId);
            if (customer != null)
            { 
                dbContext.Customers
                        .Remove(customer);
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public Customer Update(Guid customerId, Customer customerDetails)
        {
            var customerEntity = dbContext.Update(customerDetails);
            dbContext.SaveChanges();
            return customerEntity.Entity;
        }
        
    }
}
