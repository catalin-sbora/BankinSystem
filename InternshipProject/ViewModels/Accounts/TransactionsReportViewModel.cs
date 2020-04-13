using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;

namespace InternshipProject.ViewModels.Accounts
{
    public class TransactionsReportViewModel
    {
        public string CustomerName { get; set; }
        public decimal Balance { get; set; }
        public string Account { get; set; }
        public string Currency { get; set; }

        public IEnumerable<Transaction> Transactions {get; set;}

    }
}
