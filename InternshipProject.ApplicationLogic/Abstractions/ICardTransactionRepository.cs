using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Abstractions
{
    public interface ICardTransactionRepository:IRepository<CardTransaction>
    {
        IEnumerable<CardTransaction> GetCardTransactions(IEnumerable<Transaction> transactions);

    }
}
