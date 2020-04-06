using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ViewModels.Accounts;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ViewModels.Statistics;

namespace InternshipProject.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            StatisticsViewModel viewModel = new StatisticsViewModel()
            {

                CustomerName = "John Doe",

                PhoneNo = "07288377349",

                BankAccounts = new List<BankAccount> {
                    new BankAccount{ IBAN = "RO511452423", Balance = 134},
                    new BankAccount{ IBAN = "RO122451243", Balance = 200},
                    new BankAccount{ IBAN = "RO778452423", Balance = 621}
                }
            };

            return View(viewModel);
        }
    }
}
