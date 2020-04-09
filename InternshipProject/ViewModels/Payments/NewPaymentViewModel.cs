using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Payments
{
    public class NewPaymentViewModel
    {
        public string ExternalIBAN { get; set; }
        public string ExternalName { get; set; }
        public decimal Amount { get; set; }
        public string BankAccountId { get; set; }
        public IEnumerable<BankAccount> BanksAccount { get; set; }
    }
}
