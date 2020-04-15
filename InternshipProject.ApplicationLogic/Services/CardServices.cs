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
        public CardTransaction AddTransaction(decimal amount, string iban, Guid cardId )
        {
            // Guid guidCardId = Guid.Empty;
            //Guid.TryParse(cardId, out guidCardId);

            // var transaction = Transaction.Create(amount, iban, null, null);
            // transaction.BankAccountId = GetCardByCardId(cardId).BankAccount.Id;
            // var cardTransaction = CardTransaction.Create(transaction, CardTransactionType.Online );
            // transactionRepository.Add(transaction);
            // cardTransactionRepository.Add(cardTransaction);
            //// var cardSelected = cardRepository.GetById(cardId);
            // //cardSelected.CardTransactions.Add(cardTransaction);
            // return cardTransaction;
            return null;
        }
    }
}
