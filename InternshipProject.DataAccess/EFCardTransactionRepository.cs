using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFCardTransactionRepository : BaseRepository<CardTransaction>, ICardTransactionRepository
    {
        public EFCardTransactionRepository(BankingDbContext dbContext) : base(dbContext)
        {

        }

        public IEnumerable<CardTransaction> GetCardTransactions(IEnumerable<Transaction> transactions)
        {
            //  return dbContext.CardTransactions.Where(temp => temp.CardId == cardId);
            var cardTransactions = new List<CardTransaction>();
            foreach(var transaction in transactions)
            {
                var cardTransaction =  (dbContext.CardTransactions.Where(temp => temp.Transaction.Id == transaction.Id).FirstOrDefault());
                if (cardTransaction != null)
                {
                    cardTransactions.Add(cardTransaction);
                }
            }
            return cardTransactions.AsEnumerable();
        }
    }
}
