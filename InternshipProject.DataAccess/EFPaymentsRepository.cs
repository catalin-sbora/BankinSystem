using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFPaymentsRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public EFPaymentsRepository(BankingDbContext dbContext) : base(dbContext)
        { }

        public Transaction GetTransactionById(Guid transactionId)
        {
            return GetById(transactionId);
        }

        public Transaction NewTransaction(decimal amount, string externalName, string externalIBAN, Guid bankAccountId)
        {
            var selectedBankAccount = dbContext.BankAccounts.Where(bankAccount => bankAccount.Id == bankAccountId).SingleOrDefault();
            var transaction = selectedBankAccount.CreatePayment(amount, externalName, externalIBAN, null);
            Add(transaction);

            return transaction;
        }
        public Transaction NewReceived(decimal amount, string externalName, string externalIBAN, Guid bankAccountId)
        {
            var selectedBankAccount = dbContext.BankAccounts.Where(bankAccount => bankAccount.Id == bankAccountId).SingleOrDefault();
            var transaction = selectedBankAccount.CreateReceive(amount, externalName, externalIBAN, null);
            Add(transaction);

            return transaction;
        }
        public IEnumerable<Transaction> GetAllTransactionsForCustomer(Guid userId)
        {
            var customer = dbContext.Customers.Where(c => c.UserId == userId).SingleOrDefault();
            List<Transaction> transactionList = new List<Transaction>();

            foreach (var bankAccount in customer.BankAccounts)
            {
                transactionList.AddRange(bankAccount.Transactions);
            }

            return transactionList.AsEnumerable();
        }
        public IEnumerable<Transaction> GetReceived(Guid userId)
        {
            var customer = dbContext.Customers.Where(c => c.UserId == userId).SingleOrDefault();
            List<Transaction> transactionList = new List<Transaction>();

            foreach (var bankAccount in customer.BankAccounts)
            {
                for(int i=0;i<bankAccount.Transactions.Count;i++)
                {
                    if(bankAccount.Transactions.ElementAt(i).Amount>0)
                        transactionList.Add(bankAccount.Transactions.ElementAt(i));
                }
                
            }

            return transactionList.AsEnumerable();
        }
        public IEnumerable<Transaction> GetTransactionsFromBankAccount(Guid Id)
        {
            return dbContext.Transactions.Where(b => b.BankAccountId == Id);
        }
    }
}
