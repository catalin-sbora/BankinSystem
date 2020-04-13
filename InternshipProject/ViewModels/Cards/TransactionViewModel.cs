using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Cards
{
    public class TransactionViewModel
    {
        public string IBan { get; set; }
        public decimal Amount { get; set; }

        public Guid CardId { get; set; }
        public IEnumerable<Card> CardList {get; set;}
    }
}
