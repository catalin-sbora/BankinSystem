using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Payments
{
    public enum NewPaymentStatus
    {
        NotInitiated = 0,
        Created,
        Failed
    }
    public class NewPaymentViewModel
    {
        public string ExternalIBAN { get; set; }
        public string ExternalName { get; set; }
        public decimal Amount { get; set; }
        public Guid? BankAccountId { get; set; }
        public IEnumerable<BankAccount> BanksAccount { get; set; }
        public NewPaymentStatus PaymentStatus { get; set; }
        public string PaymentMessage { get; set; }
    }
}
