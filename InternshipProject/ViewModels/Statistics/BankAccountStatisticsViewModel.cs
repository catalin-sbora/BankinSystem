using InternshipProject.ApplicationLogic.Model;
using System.Collections.Generic;

namespace InternshipProject.ViewModels.Statistics
{
    public class BankAccountStatisticsViewModel
    {
        public BankAccount BankAccount { get; set; }
        public BankAccountMetaData MetaData { get; set; }
        public IEnumerable<decimal> BalanceHistory { get; set; }
    }
}
