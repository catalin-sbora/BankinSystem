using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFCardMetaDataRepository : BaseRepository<CardMetaData>, ICardMetaDataRepository
    {
        public EFCardMetaDataRepository(BankingDbContext dbContext):base(dbContext)
        {

        }
        public CardMetaData GetMetaData(Guid cardId)
        {
            var color = dbContext.CardMetaData.Where(cardColor => cardColor.CardId == cardId).FirstOrDefault();
            return color;
        }
    }
}
