using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Cards
{
    public enum CardTransactionTypeFilter {
        POS,
        ATM,
        Online,
        All
    }
    public class CardTransactionsListViewModel
    {
        public List<CardTransactionViewModel> CardTransactions { get; set; }
        public Guid BankAccountId { get; set; }
        public CardTransactionType? TransactionType { get; set; }
        public string SearchBy { get; set; }
        public Guid CardId { get; set; }
        public string SerialNumber { get; set; }
        public string OwnerName { get; set; }
        
    }
}
