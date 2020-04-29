using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternshipProject.ApplicationLogic.Services
{
    public class StatisticsService
    {

        private readonly ICustomerRepository customerRepository;
        
        public StatisticsService(IPersistenceContext persistenceContext)
        {
            customerRepository = persistenceContext.CustomerRepository;
        }

        private IEnumerable<decimal> ProcessBalanceHistory(decimal initialBalance, IEnumerable<Transaction> transactions)
        {
            List<decimal> balanceOverTime = new List<decimal>();
            var currentBalance = initialBalance;

            balanceOverTime.Add(initialBalance);
            foreach (Transaction transaction in transactions)
            {
                currentBalance -= transaction.Amount;
                balanceOverTime.Add(currentBalance);
            }

            return balanceOverTime.AsEnumerable().Reverse();
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

        private BankAccount GetBankAccountWithMostTransactions(string userId)
        {
            BankAccount maxTransactionsBankAccount = null;
            int maxTransactions = 0;

            foreach (BankAccount account in GetCustomerBankAccounts(userId))
            {
                int transactionCount = account.Transactions.Count();
                if (transactionCount > maxTransactions)
                {
                    maxTransactions = transactionCount;
                    maxTransactionsBankAccount = account;
                }
            }

            return maxTransactionsBankAccount;
        }

        public IEnumerable<int> GetIndexListForTransactions(string userId)
        {
            List<int> transactionList = new List<int>();

            BankAccount chosenBankAccount = GetBankAccountWithMostTransactions(userId);
            
            for (int i = 0; i <= chosenBankAccount.Transactions.Count(); i++)
            {
                transactionList.Add(i);
            }

            return transactionList.AsEnumerable();
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

        public IEnumerable<decimal> BankAccountHistoryDay(BankAccount bankAccount)
        {

            var sortedTransactions = FilterAccountTransactions(bankAccount,
                                                                transaction =>
                                                                transaction.Time.Year == DateTime.UtcNow.Year
                                                                &&
                                                                transaction.Time.Month == DateTime.UtcNow.Month
                                                                &&
                                                                transaction.Time.Day == DateTime.UtcNow.Day
                                                                );

            return ProcessBalanceHistory(bankAccount.Balance, sortedTransactions);
        }
    }
}
