using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Statistics
{
    public class BankAccountStatisticsViewModel
    {
        public BankAccount BankAccount { get; set; }
        public BankAccountMetaData MetaData { get; set; }
        public IEnumerable<decimal> BalanceHistory { get; set; }
        public IEnumerable<decimal> YearlyBalanceHistory { get; set; }
        //public IEnumerable<decimal> MonthlyBalanceHistory { get; set; }
        //public IEnumerable<decimal> WeeklyBalanceHistory { get; set; }
        //public IEnumerable<decimal> DailyBalanceHistory { get; set; }
    }
}
