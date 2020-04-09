using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private IEnumerable<decimal> ProcessBalanceHistory(decimal initialBalance, IEnumerable<Transaction> transactions)
        {
            List<decimal> balanceOverTime = new List<decimal>();
            var currentBalance = initialBalance;
            foreach (Transaction transaction in transactions)
            {
                currentBalance -= transaction.Amount;
                balanceOverTime.Add(currentBalance);
            }
            return balanceOverTime.AsEnumerable();
        }

        private IEnumerable<Transaction> FilterAccountTransactions(BankAccount account, Func<Transaction, bool> condition)
        {
            return account.Transactions.Where(condition)
                                       .OrderByDescending(t => t.Time);
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
            
            var sortedTransactions = FilterAccountTransactions(bankAccount, t => true);
            return ProcessBalanceHistory(bankAccount.Balance, sortedTransactions);
        }

        public IEnumerable<decimal> BankAccountHistoryYear(BankAccount bankAccount)
        {

            var sortedTransactions = FilterAccountTransactions(bankAccount,
                                                                transaction =>
                                                                DateTime.UtcNow
                                                                        .Subtract(transaction.Time)
                                                                        .Days <= 365);

            return ProcessBalanceHistory(bankAccount.Balance, sortedTransactions);
        }

        public IEnumerable<decimal> BankAccountHistoryMonth(BankAccount bankAccount)
        {

            var sortedTransactions = bankAccount.Transactions.Where(transaction =>
                                                                transaction.Time.Year == DateTime.UtcNow.Year
                                                                &&
                                                                transaction.Time.Month == DateTime.UtcNow.Month);

            return ProcessBalanceHistory(bankAccount.Balance, sortedTransactions);
        }

        public IEnumerable<decimal> BankAccountHistoryWeek(BankAccount bankAccount)
        {

            var sortedTransactions = FilterAccountTransactions(bankAccount,
                                                                transaction =>
                                                                DateTime.UtcNow
                                                                        .Subtract(transaction.Time)
                                                                        .Days <= 7);

            return ProcessBalanceHistory(bankAccount.Balance, sortedTransactions);
        }
    }
}
