using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class ReceivedService
    {        
        private CustomerService customerService;

        public ReceivedService(CustomerService customerService) 
        {           
            this.customerService = customerService;
        }
       
        public IEnumerable<Transaction> GetCustomerTransaction(string userId)
        {
            var customer = customerService.GetCustomerFromUserId(userId);
            var receivedList = new List<Transaction>();
            foreach (var account in customer.BankAccounts)
            {
                foreach (var transaction in account.Transactions)
                {
                    if (transaction.Amount > 0)
                    {
                        receivedList.Add(transaction);
                    }
                }
            }
            return receivedList.AsEnumerable();
        }

    }
}
