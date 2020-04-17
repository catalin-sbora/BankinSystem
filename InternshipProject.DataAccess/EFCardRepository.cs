using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFCardRepository :BaseRepository<Card>, ICardRepository
    {
      
        public EFCardRepository(BankingDbContext dbContext):base(dbContext)
        {
           
        }
        public IEnumerable<Card> GetByAccountId(Guid accountId)
        {
            var cardList = dbContext.Cards
                                    .Where(card => card.BankAccount.Id == accountId);
            return cardList;
        }    
        public void AddCardTransaction(Card card)
        {
            dbContext.Cards.Update(card);
            dbContext.SaveChanges();
        }
        
        
    }
}
