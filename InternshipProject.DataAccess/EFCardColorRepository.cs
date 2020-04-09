using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFCardColorRepository : BaseRepository<CardColor>, ICardColorRepository
    {
        public EFCardColorRepository(BankingDbContext dbContext):base(dbContext)
        {

        }
        public CardColor GetColor(Guid cardId)
        {
            var color = dbContext.CardColors.Where(cardColor => cardColor.CardId == cardId).FirstOrDefault();
            return color;
        }
    }
}
