using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Statistics
{
    public class StatisticsViewModel
    {
        public IEnumerable<BankAccountStatisticsViewModel> BankAccounts { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
    }
}
