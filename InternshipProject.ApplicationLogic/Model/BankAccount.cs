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
    public class BankAccount
    {
        private List<Transaction> transactions = new List<Transaction>();
        //private List<Card> cards = new List<Card>();
        public Guid Id { get; set; }
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
        public Transaction CreatePayment(decimal amount, string destinationName, string destinationIBAN, string description)
        {
            if (amount <= 0)
                throw new Exception("");

            if (string.IsNullOrEmpty(destinationName))
                throw new Exception("");


            var transaction = Transaction.Create(-amount, destinationName, destinationIBAN, description);
            transactions.Add(transaction);

            return transaction;
        }   

        public void CreateReceive(decimal amount, string sourceName, string sourceIBAN, string description)
        {
            if (string.IsNullOrEmpty(sourceName) && string.IsNullOrEmpty(sourceIBAN))
                throw new Exception("");

            if (amount <= 0)
                throw new Exception("");

            var transaction = Transaction.Create(amount, sourceName, sourceIBAN, description);
            transactions.Add(transaction);
        }

        /*public Card AttachNewCard(string serialNumber, string cvv, string ownerName)
        {
            var card = Card.Create(serialNumber, cvv, ownerName, this);                        
            return card;
        }*/

        /*public void DetachCard(Card cardToRemove)
        {
            var selectedCard = cards.Where(card => card.Id == cardToRemove.Id)
                  .SingleOrDefault();
            if (selectedCard == null)
                throw new Exception("NotFound");

            cards.RemoveAll(card => card.Id == selectedCard.Id);
        }*/

    }
}
