using System;
using System.Collections.Generic;
using InternshipProject.ApplicationLogic.Model;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Received
{
    public class AddReceivedViewModel
    {
        public string ExternalIBAN { get; set; }
        public string ExternalName { get; set; }
        public decimal Amount { get; set; }
        public string BankAccountId { get; set; }
        public IEnumerable<BankAccount> BankAccount { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}