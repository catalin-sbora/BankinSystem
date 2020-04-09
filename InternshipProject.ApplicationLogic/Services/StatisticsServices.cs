using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class StatisticsServices
    {

        private readonly ICustomerRepository customerRepository;
        public StatisticsServices(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
       

        public IEnumerable<BankAccount> GetCustomerBankAccounts(string userId)
        {
            Guid.TryParse(userId, out Guid idToSearch);
            var customer = customerRepository?.GetCustomerByUserId(idToSearch);

            if (customer == null)
            {
                throw new CustomerNotFoundException(userId);
            }

            return customer.BankAccounts
                            .AsEnumerable();
        }

        public IEnumerable<decimal> BankAccountHistoryAllTime(BankAccount bankAccount)
        {
            List<decimal> balanceOverTime = new List<decimal>();
            var currentBalance = bankAccount.Balance;
            var sortedTransactions = bankAccount.Transactions.OrderByDescending(param => param.Time);

            foreach(Transaction transaction in sortedTransactions)
            {
                currentBalance -= transaction.Amount;
                balanceOverTime.Add(currentBalance);
            }

            return balanceOverTime.AsEnumerable();
        }

        public IEnumerable<decimal> BankAccountHistoryYear(BankAccount bankAccount)
        {
            List<decimal> balanceOverTime = new List<decimal>();
            var currentBalance = bankAccount.Balance;
            var sortedTransactions = bankAccount.Transactions.Where(transaction => 
                                                                transaction.Time.Year >= DateTime.UtcNow.Year - 1)
                                                                .OrderByDescending(param => param.Time);

            foreach (Transaction transaction in sortedTransactions)
            {
                currentBalance -= transaction.Amount;
                balanceOverTime.Add(currentBalance);
            }

            return balanceOverTime.AsEnumerable();
        }
    }
}
