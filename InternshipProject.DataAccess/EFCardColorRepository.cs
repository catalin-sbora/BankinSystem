using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFCardColorRepository : BaseRepository<CardMetaData>, ICardColorRepository
    {
        public EFCardColorRepository(BankingDbContext dbContext):base(dbContext)
        {

        }
        public CardMetaData GetColor(Guid cardId)
        {
            var color = dbContext.CardMetaData.Where(cardColor => cardColor.CardId == cardId).FirstOrDefault();
            return color;
        }
    }
}
