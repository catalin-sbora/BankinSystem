using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using InternshipProject.ViewModels.Statistics;
using InternshipProject.ApplicationLogic.Services;

namespace InternshipProject.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/{id?}")]
    [Route("[controller]/{id?}")]
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
        
        [HttpGet("{interval?}")]
        public IActionResult Index([FromQuery]string interval)
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
                        MetaData = metaDataService.GetMetaDataForBankAccount(bankAccount.Id)
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

        public IActionResult LineChart([FromQuery]string interval)
        {

            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerService.GetCustomer(userId);
                List<BankAccountStatisticsViewModel> statisticsViewModels = new List<BankAccountStatisticsViewModel>();

                foreach (var bankAccount in customer.BankAccounts)
                {
                    var balanceHistory = new List<decimal>();
                    if (interval == null)
                        interval = "";
                    switch (interval.ToLower())
                    {
                        case "":
                        case "alltime":
                            balanceHistory = new List<decimal>(statisticsService.BankAccountHistoryAllTime(bankAccount));
                            break;

                        case "year":
                            balanceHistory = new List<decimal>(statisticsService.BankAccountHistoryYear(bankAccount));
                            break;

                        case "month":
                            balanceHistory = new List<decimal>(statisticsService.BankAccountHistoryMonth(bankAccount));
                            break;

                        case "week":
                            balanceHistory = new List<decimal>(statisticsService.BankAccountHistoryWeek(bankAccount));
                            break;

                        case "day":
                            balanceHistory = new List<decimal>(statisticsService.BankAccountHistoryDay(bankAccount));
                            break;
                    }

                    statisticsViewModels.Add(new BankAccountStatisticsViewModel
                    {
                        BankAccount = bankAccount,
                        MetaData = metaDataService.GetMetaDataForBankAccount(bankAccount.Id),
                        BalanceHistory = balanceHistory,
                    });
                }

                StatisticsViewModel viewModel = new StatisticsViewModel()
                {
                    BankAccounts = statisticsViewModels,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    PhoneNo = customer.ContactDetails?.PhoneNo,
                    TransactionIndexes = statisticsService.GetIndexListForTransactions(userId)
                };

                return PartialView("_LineChartPartial", viewModel);
            }
            catch (Exception e)
            {
                //log exception
                return BadRequest("Unable retrieve data for the current user");
            }
        }

    }
}
