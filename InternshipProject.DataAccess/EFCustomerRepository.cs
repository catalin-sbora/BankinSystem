﻿using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFCustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
       
        public EFCustomerRepository(BankingDbContext dbContext):base(dbContext)
        {            
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

        public Customer UpdateCustomerDetails(Guid customerId, Customer customerDetails)
        {
            var customerEntity = dbContext.Update(customerDetails);
            dbContext.SaveChanges();
            return customerEntity.Entity;
        }
    }
}
