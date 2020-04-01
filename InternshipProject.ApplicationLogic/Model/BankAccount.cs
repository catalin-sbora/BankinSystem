using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Card> Cards { get; set; }        

    }
}
