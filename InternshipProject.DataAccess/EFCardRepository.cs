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
        public IEnumerable<Card> GetByUserId(Guid userId)
        {
            var cardList = dbContext.Cards.Where(user => user.BankAccount.Id == userId);
            return cardList;
        }    
        public void AddCardTransaction(Card card)
        {
            dbContext.Cards.Update(card);
            dbContext.SaveChanges();
        }
        
        
    }
}
