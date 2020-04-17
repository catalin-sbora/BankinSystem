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
        private readonly IPersistenceContext persistenceContext;
        private readonly ICardRepository cardRepository;              
        private readonly AccountsService accountsService;
        private readonly PaymentsService paymentsService;
        public CardServices(IPersistenceContext persistenceContext,                            
                            AccountsService accountsService,
                            PaymentsService paymentsService)
        {
            this.cardRepository = persistenceContext.CardRepository;
            
            this.accountsService = accountsService;                        
            this.paymentsService = paymentsService;
            this.persistenceContext = persistenceContext;
        }
        public Card GetCardByCardId(Guid CardId)
        {
            return cardRepository.GetById(CardId);
        }
        

        public void MakeOnlinePayment(string userId, Guid cardId, decimal amount, string destName, string destIBAN)
        {
            var cards = GetCardsForCustomer(userId);
            var selectedCard = cards.Where(card => card.Id == cardId)
                                    .Single();
            
            var transaction = paymentsService.CreateAccountPayment(userId, 
                selectedCard.BankAccount.Id, 
                amount, 
                destName, 
                destIBAN, 
                "Card Payment");
            
            var cardTransaction = CardTransaction.Create(transaction, CardTransactionType.Online);
            selectedCard.CardTransactions.Add(cardTransaction);
            
            cardRepository.Update(selectedCard);
            persistenceContext.SaveChanges();

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
       
        public IEnumerable<Card> GetCardsForAccount(string userId, Guid accountId)
        {            
            var account = accountsService.GetCustomerBankAccount(userId, accountId);            
            return cardRepository.GetByAccountId(accountId);                
        }

        public IEnumerable<Card> GetCardsForCustomer(string userId)
        {
            var bankAccounts = accountsService.GetCustomerBankAccounts(userId);
            var cards = new List<Card>();
            foreach(var bankAccount in bankAccounts)
            {
                cards.AddRange(cardRepository.GetByAccountId(bankAccount.Id));       
            }
            return cards;
        }

    }
}
