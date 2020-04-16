using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class CardServices
    {
        private readonly ICardRepository cardRepository;
        private readonly ICardTransactionRepository cardTransactionRepository;
        private readonly ITransactionRepository transactionRepository;
        ICustomerRepository customerRepository;
        public CardServices(ICustomerRepository customer, ICardRepository cardRepository , ICardTransactionRepository cardTransactionRepository,ITransactionRepository transactionRepository)
        {
            this.cardRepository = cardRepository;
            this.cardTransactionRepository = cardTransactionRepository;
            this.transactionRepository = transactionRepository;
            customerRepository = customer;
        }
       public Card GetCardByCardId(Guid CardId)
        {
            return cardRepository.GetById(CardId);
        }
        public CardTransaction AddTransaction(CardTransaction cardTransaction)
        {
             cardTransactionRepository.Add(cardTransaction);
             return cardTransaction;
        }
        public IEnumerable<CardTransaction> GetAllCardTransactions(Guid cardId)
        {
            var card = GetCardByCardId(cardId);
            return card.CardTransactions;
           // return cardTransactionRepository.GetCardTransactions(transactions);
        }
        public IEnumerable<CardTransaction> GetFilteredCardTransactions(Guid cardId ,string searchBy, CardTransactionType? type )
        {
            var card = GetCardByCardId(cardId);
            var transactions = card.CardTransactions;
            if (type != null)

            {
             transactions = transactions.Where(cardTransaction=> cardTransaction.TransactionType == type.Value ).ToList();
            }
            if(!string.IsNullOrEmpty(searchBy))
            {
                searchBy = searchBy.ToLower();
                transactions = transactions.Where(transaction =>
                                      transaction.Transaction != null &&
                                    ((transaction.Transaction.ExternalName != null &&
                                    transaction.Transaction.ExternalName.ToLower().Contains(searchBy)) ||
                                    (transaction.Transaction.Time != null &&
                                    transaction.Transaction.Time.ToString().Contains(searchBy)) ||
                                    transaction.Transaction.Amount.ToString().Contains(searchBy))).ToList();
            }
            return transactions.AsEnumerable();
        }
            public void AddCardTransaction(Card card)
            {
            cardRepository.AddCardTransaction(card);
            }
    }
}
