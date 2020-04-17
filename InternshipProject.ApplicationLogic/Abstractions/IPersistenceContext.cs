using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace InternshipProject.ApplicationLogic.Abstractions
{
    public interface IPersistenceContext: IDisposable
    {
        IBankAccountMetaDataRepository BankAccountMetaDataRepository { get; }
        ICardMetaDataRepository CardMetaDataRepository { get; }        
        ICustomerRepository CustomerRepository { get; }
        ICardRepository CardRepository { get; }           
        TransactionScope BeginTransaction();        
        void SaveChanges();
    }
}
