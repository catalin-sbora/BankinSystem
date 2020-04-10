using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class EFCardTransactionRepository : BaseRepository<CardTransaction>, ICardTransactionRepository
    {
        public EFCardTransactionRepository(BankingDbContext dbContext) : base(dbContext)
        {

        }
    }
}
