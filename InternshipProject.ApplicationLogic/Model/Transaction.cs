using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    public enum TransactionStatus 
    {
        Created,
        Processing,
        Accepted,
        Rejected
    }
    public class Transaction
    {
        public Guid Id { get; set; }
        public string ExternalIBAN { get; set; }
        public string ExternalName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        public string Details { get; set; }        
        public TransactionStatus Status { get; set; }
    }
}
