using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Accounts
{
    public class BankAccountViewModel
    {
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public BankAccount BankAccount { get; set; }
        public BankAccountMetaData MetaData { get; set; }
    }
}
