using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
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
        public IEnumerable<CardTransaction> GetCardTransactions(IEnumerable<Transaction> transactions)
        {
           return cardTransactionRepository.GetCardTransactions(transactions);
        }
    }
}
