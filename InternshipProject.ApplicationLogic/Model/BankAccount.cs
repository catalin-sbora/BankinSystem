using InternshipProject.ApplicationLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    /*
     
         WARNING: Exceptions are not fully implemented yet
    */
    public class BankAccount: DataEntity
    {
        private List<Transaction> transactions = new List<Transaction>();              
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public virtual IReadOnlyCollection<Transaction> Transactions
        {
            get
            {
               return transactions.AsReadOnly();
            }
            private set
            {
                transactions = new List<Transaction>(value);
            }
        }              
        public DateTime LastTransactionDate { get; private set; }
        
        protected BankAccount()
        { 
            
        }

        private static bool IsValidIBAN(string IBAN)
        {
            if (IBAN.Length == 24)
                return true;


            return false;
        }
       
        public static BankAccount Create(string accountIBAN)
        {

            if (!IsValidIBAN(accountIBAN))
                throw new InvalidIBANAccountException(accountIBAN);

            return new BankAccount
                    {
                        Id = Guid.NewGuid(),
                        Balance = new decimal(0.0),                        
                        IBAN = accountIBAN                        
                    };
        }

        private void AddTransaction(Transaction transaction)
        {
            Transactions.Count();
            transactions.Add(transaction);
            LastTransactionDate = DateTime.UtcNow;
        }

        public Transaction CreatePayment(decimal amount, string destinationName, string destinationIBAN, string description)
        {
            if (amount <= 0)
                throw new Exception("");

            if (string.IsNullOrEmpty(destinationName))
                throw new Exception("");

            if (amount > Balance)
            {
                throw new NotEnoughFundsException(Balance, amount);
            }
            
            Transactions.Count();
            var transaction = Transaction.Create(-amount, destinationName, destinationIBAN, description);

            AddTransaction(transaction);
            
            Balance -= amount;

            return transaction;
        }   

        public void CreateReceive(decimal amount, string sourceName, string sourceIBAN, string description)
        {
            if (string.IsNullOrEmpty(sourceName) && string.IsNullOrEmpty(sourceIBAN))
                throw new Exception("");

            if (amount <= 0)
                throw new Exception("");

            var transaction = Transaction.Create(amount, sourceName, sourceIBAN, description);
            AddTransaction(transaction);
            Balance += amount;
        }

        public decimal GetAmountPaidInCurrentMonth()
        {
            var paidThisMonth = Transactions.Where(transaction =>
                                                            transaction.Time.Year == DateTime.UtcNow.Year &&
                                                            transaction.Time.Month == DateTime.UtcNow.Month &&
                                                            transaction.Amount < 0
                                                            )
                                                    .Sum(transaction => transaction.Amount);
            return paidThisMonth;

        }
    }
}
