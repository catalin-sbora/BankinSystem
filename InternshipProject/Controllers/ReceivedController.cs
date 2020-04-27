
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using InternshipProject.ViewModels.Accounts;
using InternshipProject.ViewModels.Received;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RazorPagesReporting;

namespace InternshipProject.Controllers
{
    [Authorize]
    public class ReceivedController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly CustomerService customerService;       
        private readonly ReceivedService receivedService;
        private readonly RazorPagesReportingEngine reportingEngine;
        private readonly ILogger<ReceivedController> logger;
        public ReceivedController(UserManager<IdentityUser> userManager,
            CustomerService customerService,              
            ReceivedService receivedService, 
            RazorPagesReportingEngine reportingEngine,
            ILogger<ReceivedController> logger)       
        {
            this.logger = logger;
            this.reportingEngine = reportingEngine;
            this.receivedService = receivedService;
            this.userManager = userManager;
            this.customerService = customerService;
            //this.transactionService = transactionService;
        }
      
        public IActionResult IndexAsync()
        {
            var userId = userManager.GetUserId(User);
            var customer = customerService.GetCustomerFromUserId(userId);
            try
            {                 
                
                var received = receivedService.GetCustomerTransaction(userId);
                var viewModel = new ReceivedListViewModel()
                {
                    //IsSelected = viewModel.IsSelected,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    PhoneNo = customer.ContactDetails?.PhoneNo,
                    BankAccounts = customer.BankAccounts,
                    Received = received.OrderByDescending(transaction => transaction.Time)
                };

                return View(viewModel);
            }
            catch (Exception e) 
            {
                logger.LogError("Failed to retrieve Received Transaction list{@ExceptionMessage}", e.Message);
                logger.LogDebug("Failed to retrieve Received Transaction list{@Exception}", e);
                
                return BadRequest("Unable to retrieve data");
            }
        }
        [HttpGet]
        public IActionResult AddReceived(string accountId)
        {
            var accountGuid = Guid.Empty;
            Guid.TryParse(accountId, out accountGuid);
            return PartialView("_ReceivedPartial", new NewPaymentViewModel { SourceAccount = accountGuid, PaymentStatus = NewPaymentStatus.NotInitiated });
        }


        [HttpPost]
        public IActionResult AddReceived([FromForm]NewPaymentViewModel paymentData)
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var customer = customerService.GetCustomerFromUserId(userId);
                var viewModel = new AddReceivedViewModel()
                {
                    
                    BankAccount = customer.BankAccounts
                };

                return View( viewModel);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to add the transaction{@ExceptionMessage}", e.Message);
                logger.LogDebug("Failed to add the transaction{@Exception}", e);
                
                return BadRequest("Bad Input");
            }
        }

        [HttpPost]
        public IActionResult Create(AddReceivedViewModel viewModel)
        {
            //transactionService.AddReceived(viewModel.Amount, viewModel.ExternalName, viewModel.ExternalIBAN, viewModel.BankAccountId);
            return RedirectToAction("Index");
        }

        
    }
}