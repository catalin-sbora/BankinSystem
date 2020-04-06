using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Statistics
{
    public class StatisticsViewModel
    {
        public IEnumerable<BankAccount> BankAccounts { get; set; }

        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
    }
}
