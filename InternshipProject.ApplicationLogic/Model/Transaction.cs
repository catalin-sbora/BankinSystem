using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace InternshipProject.ApplicationLogic.Model
{
    public enum TransactionStatus 
    {
        Created,
        Processing,
        Accepted,
        Rejected
    }
    public class Transaction: DataEntity
    {        
        public string ExternalIBAN { get; private set; }
        public string ExternalName { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Time { get; private set; }
        public string Details { get; private set; }        
        public TransactionStatus Status { get; private set; }
        public Guid BankAccountId { get; set; }

        public Transaction()
        { 
        }

        public static Transaction Create(decimal amount, string externalName, string externalIBAN, string details)
        {
            //validare iban - throw exception
            return new Transaction {
                Id = Guid.NewGuid(),
                ExternalIBAN = externalIBAN,
                ExternalName = externalName,
                Amount = amount,
                Time = DateTime.UtcNow,
                Details = details,
                Status = TransactionStatus.Created
            };
        }
    }
}
