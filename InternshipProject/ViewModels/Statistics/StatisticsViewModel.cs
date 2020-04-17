using System.Collections.Generic;

namespace InternshipProject.ViewModels.Statistics
{
    public class StatisticsViewModel
    {
        public IEnumerable<BankAccountStatisticsViewModel> BankAccounts { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
        public IEnumerable<int> TransactionIndexes { get; set; }
    }
}
