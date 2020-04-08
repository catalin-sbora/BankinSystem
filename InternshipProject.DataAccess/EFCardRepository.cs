using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    class EFCardRepository :BaseRepository<Card>, ICardRepository
    {
      
        public EFCardRepository(BankingDbContext dbContext):base(dbContext)
        {
           
        }
        public Card Add(Card card)
        {
            var addedEntity = dbContext.Add(card);
            dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public IEnumerable<Card> GetByAccount(Guid accountId)
        {
            var cardList = dbContext.Cards.Where(BankAccount => BankAccount.Id == accountId);
            return cardList;
        }

        public Card GetById(Guid cardId)
        {
            var card = dbContext.Cards.Where(Card => Card.Id == cardId).FirstOrDefault();
            return card;
        }
    }
}
