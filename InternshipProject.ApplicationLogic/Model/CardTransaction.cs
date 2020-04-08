using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    public enum CardTransactionType
    { 
        POS,
        ATM,
        Online
    }
    public class CardTransaction:DataEntity
    {
       
        public virtual Transaction Transaction{ get; private set; }
        public CardTransactionType TransactionType { get; private set; }

        protected CardTransaction()
        { 
        }

        public static CardTransaction Create(Transaction transaction, CardTransactionType transactionType)
        {
            return new CardTransaction { 
                Id = Guid.NewGuid(),
                Transaction = transaction,
                TransactionType = transactionType
            };
        }
    }
}
