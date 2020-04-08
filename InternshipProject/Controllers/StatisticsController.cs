using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ViewModels.Statistics;
using InternshipProject.ApplicationLogic.Services;

namespace InternshipProject.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly StatisticsServices statisticServices;
        private readonly CustomerService customerServices;
        private readonly UserManager<IdentityUser> userManager;

        public StatisticsController(StatisticsServices statisticServices, CustomerService customerServices, UserManager<IdentityUser> userManager)
        {
            this.statisticServices = statisticServices;
            this.customerServices = customerServices;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {

            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                StatisticsViewModel viewModel = new StatisticsViewModel()
                {
                    BalanceList = statisticServices.GetTotalBankAccountBalance(userId),
                    BankAccounts = customer.BankAccounts,
                    YearlyBalance = statisticServices.BankAccountBalanceYear(customer.BankAccounts.ElementAt(0)),
                    AllTimeBalance = statisticServices.BankAccountBalanceYear(customer.BankAccounts.ElementAt(0)),
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    PhoneNo = customer.ContactDetails?.PhoneNo
                };

                return View(viewModel);
            }
            catch (Exception e)
            {
                //log exception
                return BadRequest("Unable retrieve data for the current user");
            }
        }
    }
}
