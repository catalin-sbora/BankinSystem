using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using InternshipProject.ViewModels.Statistics;
using InternshipProject.ApplicationLogic.Services;

namespace InternshipProject.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly StatisticsServices statisticsService;
        private readonly CustomerService customerService;
        private readonly MetaDataService metaDataService;
        private readonly UserManager<IdentityUser> userManager;

        public StatisticsController(StatisticsServices statisticsServices, CustomerService customerServices, 
            MetaDataService metaDataServices, UserManager<IdentityUser> userManager)
        {
            this.statisticsService = statisticsServices;
            this.customerService = customerServices;
            this.metaDataService = metaDataServices;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {

            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerService.GetCustomer(userId);
                List<BankAccountStatisticsViewModel> statisticsViewModels = new List<BankAccountStatisticsViewModel>();

                foreach (var bankAccount in customer.BankAccounts)
                {
                    statisticsViewModels.Add(new BankAccountStatisticsViewModel
                    {
                        BankAccount = bankAccount,
                        MetaData = metaDataService.GetMetaDataForBankAccount(bankAccount.Id),
                        AllTimeBalanceHistory = statisticsService.BankAccountHistoryAllTime(bankAccount),
                        YearlyBalanceHistory = statisticsService.BankAccountHistoryYear(bankAccount),


                        //MonthlyBalanceHistory = statisticsService.BankAccountHistoryMonth(bankAccount),
                        //WeeklyBalanceHistory = statisticsService.BankAccountHistoryWeek(bankAccount),
                        //DailyBalanceHistory = statisticsService.BankAccountHistoryDay(bankAccount),
                    });
                }

                StatisticsViewModel viewModel = new StatisticsViewModel()
                {
                    BankAccounts = statisticsViewModels,
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
