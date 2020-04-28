using InternshipProject.ApplicationLogic.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;


namespace InternshipProject.EFDataAccess
{
    public class EFPersistenceContext : IPersistenceContext
    {
        private readonly BankingDbContext dbContext;
        private TransactionScope currentTransactionScope = null;
        public EFPersistenceContext(BankingDbContext context)
        {
            this.dbContext = context;
            BankAccountMetaDataRepository = new EFBankAccountMetaDataRepository(context);
            CardMetaDataRepository = new EFCardMetaDataRepository(context);
            CustomerRepository = new EFCustomerRepository(context);
            CardRepository = new EFCardRepository(context);
            //CardTransactionRepository = new EFCardTransactionRepository(context);

        }
        public IBankAccountMetaDataRepository BankAccountMetaDataRepository 
        {
            get;
            private set;
        }     

        public ICustomerRepository CustomerRepository
        {
            get;
            private set;
        }
        public ICardRepository CardRepository 
        {
            get;
            private set;
        }       

        public ICardMetaDataRepository CardMetaDataRepository
        {
            get;
            private set;
        }

        public TransactionScope BeginTransaction()
        {
            if (currentTransactionScope != null)
            {
                throw new TransactionException("A transaction is already in progress for this context");
            }
            currentTransactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
            
            return currentTransactionScope;
        }      

        public void Dispose()
        {
            
            dbContext.Dispose();
        }

        public void SaveChanges()
        {         
            dbContext.SaveChanges();
            if (currentTransactionScope != null)
            {
                currentTransactionScope.Complete();
            }

            currentTransactionScope = null;
        }
    }
}
