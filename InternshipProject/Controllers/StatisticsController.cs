using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using InternshipProject.ViewModels.Statistics;
using InternshipProject.ApplicationLogic.Services;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<StatisticsController> logger;

        public StatisticsController(StatisticsServices statisticsServices, 
                                    CustomerService customerServices, 
                                    MetaDataService metaDataServices, 
                                    UserManager<IdentityUser> userManager, 
                                    ILogger<StatisticsController> logger)
        {
            this.statisticsService = statisticsServices;
            this.customerService = customerServices;
            this.metaDataService = metaDataServices;
            this.userManager = userManager;
            this.logger = logger;
        }
        
        [HttpGet("{interval?}")]
        public IActionResult Index([FromQuery]string interval)
        {

            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerService.GetCustomerFromUserId(userId);
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

                logger.LogInformation("View Model created successfully");
                return View(viewModel);
            }
            catch (Exception e)
            {

                logger.LogError("Unable to retrieve data from user {ExceptionMessage}", e);
                logger.LogDebug("Unable to retrieve data from user {@Exception}", e);
                
                return BadRequest("Unable retrieve data for the current user");
            }
        }

        public IActionResult LineChart([FromQuery]string interval)
        {

            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerService.GetCustomerFromUserId(userId);
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
                            logger.LogDebug("Picked interval all time");
                            balanceHistory = new List<decimal>(statisticsService.BankAccountHistoryAllTime(bankAccount));
                            break;

                        case "year":
                            logger.LogDebug("Picked interval year");
                            balanceHistory = new List<decimal>(statisticsService.BankAccountHistoryYear(bankAccount));
                            break;

                        case "month":
                            logger.LogDebug("Picked interval month");
                            balanceHistory = new List<decimal>(statisticsService.BankAccountHistoryMonth(bankAccount));
                            break;

                        case "week":
                            logger.LogDebug("Picked interval week");
                            balanceHistory = new List<decimal>(statisticsService.BankAccountHistoryWeek(bankAccount));
                            break;

                        case "day":
                            logger.LogDebug("Picked interval day");
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

                logger.LogInformation($"{statisticsViewModels.Count} Partial View Models created");

                StatisticsViewModel viewModel = new StatisticsViewModel()
                {
                    BankAccounts = statisticsViewModels,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    PhoneNo = customer.ContactDetails?.PhoneNo,
                    TransactionIndexes = statisticsService.GetIndexListForTransactions(userId)
                };

                logger.LogDebug("View Model for displaying the chart created successfully");
                return PartialView("_LineChartPartial", viewModel);
            }
            catch (Exception e)
            {

                logger.LogError("Unable to retrieve data from user {ExceptionMessage}", e);
                logger.LogDebug("Unable to retrieve data from user {@Exception}", e);

                return BadRequest("Unable retrieve data for the current user");
            }
        }
    }
}
