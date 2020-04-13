using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Cards
{
    public class CardTransactionsListViewModel
    {
        public List<CardTransactionViewModel> CardTransactions { get; set; }
        public Guid BankAccountId { get; set; }
        
    }
}
