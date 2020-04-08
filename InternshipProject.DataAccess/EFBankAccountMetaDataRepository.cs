using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFBankAccountMetaDataRepository : BaseRepository<BankAccountMetaData>, IBankAccountMetaDataRepository
    {
        public EFBankAccountMetaDataRepository(BankingDbContext context) : base(context)
        { 
        }
        public BankAccountMetaData GetMetadataForBankAccount(Guid bankAccountId)
        {
            return dbContext.BankAccountMetaDatas
                    .Where(metaData => metaData.BankAccountId == bankAccountId)
                    .FirstOrDefault();
        }
    }
}
