using InternshipProject.ApplicationLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    public class Card: DataEntity
    {
        private List<CardTransaction> cardTransactions = new List<CardTransaction>();
        
        public string SerialNumber { get; private set; }
        public string CVV { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public string OwnerName { get; private set; }
        public virtual BankAccount BankAccount { get; private set; }
       
        public virtual IReadOnlyCollection<CardTransaction> CardTransactions
        {
            get 
            {
                return cardTransactions.AsReadOnly();
            }
            set 
            {
                cardTransactions = new List<CardTransaction>(value);
            }
        }

        protected Card()
        { 
        }
        public static Card Create(string serialNumber, string cvv, string ownerName, BankAccount account)
        {
            if (string.IsNullOrEmpty(serialNumber))
                throw new ArgumentException("");

            if (string.IsNullOrEmpty(cvv))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(ownerName))
                throw new ArgumentException();

            if (account == null)
                throw new ArgumentNullException("account");

            var card = new Card
            {
                Id = Guid.NewGuid(),
                CVV = cvv,
                SerialNumber=serialNumber,
                CreateDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(4),
                OwnerName = ownerName,
                BankAccount = account
            };
            return card;
        }

        public CardTransaction MakePOSPayment(decimal amount, string destinationName, string destinationAccount)
        {
           var transaction = BankAccount.CreatePayment(amount, destinationName, destinationAccount, "POS Payment");
           var cardTransaction = CardTransaction.Create(transaction, CardTransactionType.POS);
           cardTransactions.Add(cardTransaction);
           return cardTransaction; 
        }

        public CardTransaction WithdrawFromATM(decimal amount, string atmIdentifier)
        {
            var transaction = BankAccount.CreatePayment(amount, atmIdentifier, "", "ATM Withdrawal");
            var cardTransaction = CardTransaction.Create(transaction, CardTransactionType.ATM);
            cardTransactions.Add(cardTransaction);
            return cardTransaction;
        }

        public CardTransaction OnlinePayment(decimal amount, string destinationName, string destinationAccount, string cvv)
        {
            if (!CVV.Equals(cvv))
                throw new CVVMismatchException(CVV, cvv);

            var transaction = BankAccount.CreatePayment(amount, destinationName, destinationAccount, "Online Payment");
            var cardTransaction = CardTransaction.Create(transaction, CardTransactionType.Online);
            cardTransactions.Add(cardTransaction);
            return cardTransaction;
        }
    }
}
