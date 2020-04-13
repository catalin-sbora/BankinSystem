using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Cards
{
    public class CardTransactionViewModel
    {
        public decimal Amount { get; set; }
        public string Name { get; set; }
       
        public string TransactionType { get; set; }
        public DateTime DateTime { get; set; }
    }
}
