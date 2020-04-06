using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipProject.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            AccountsListViewModel viewModel = new AccountsListViewModel()
            { 
                BankAccounts = new List<BankAccount> {
                   /* new BankAccount{ IBAN = "ROSDasdasd" },
                    new BankAccount{ IBAN = "ROUUYIYIY"},
                    new BankAccount{ IBAN = "DEFgfgfgfg"}*/
                }
            };
            
            return View(viewModel);
        }
    }
}