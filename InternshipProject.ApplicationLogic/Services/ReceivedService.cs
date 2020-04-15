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
        private readonly ICustomerRepository customerRepository;
        private readonly ITransactionRepository transactionRepository;

        public ReceivedService(ICustomerRepository customerRepository, ITransactionRepository transactionRepository) 
        {
            this.transactionRepository = transactionRepository;
            this.customerRepository = customerRepository;
        }

        public Customer GetCustomer(string userId)
        {
            Guid idToSearch = Guid.Empty;
            Guid.TryParse(userId, out idToSearch);
            var customer = customerRepository?.GetCustomerByUserId(idToSearch);
            if (customer == null)
            {
                throw new CustomerNotFoundException(userId);
            }

            return customer;
        }
        public IEnumerable<Transaction> GetCustomerTransaction(string userId,Customer customer)
        {
            Guid guidUserId = Guid.Empty;
            Guid.TryParse(userId, out guidUserId);

            if (guidUserId == Guid.Empty)
            {
                throw new Exception("Wrong guid");
            }
            //foreach(var bankAccount in customer.BankAccounts)
            //{ 
            //  transactionList.AddRange(
            //    bankAccount.Transactions.Where(t=>t.Amount < 0)
            //);  
            //}
            //customer.Transactions.Where(t=>t.Amount < 0)
            var transactionList = transactionRepository.GetReceived(guidUserId)
                                                        .OrderByDescending(t => t.Time);
            var received = new List<Transaction>();
            for(int i=0;i< transactionList.Count();i++)
            {
                if (transactionList.ElementAt(i).Amount > 0)
                    received.Add(transactionList.ElementAt(i));
            }

            return received.AsEnumerable();

        }

    }
}
