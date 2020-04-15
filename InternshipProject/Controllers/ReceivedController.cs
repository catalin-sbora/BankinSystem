
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
using RazorPagesReporting;

namespace InternshipProject.Controllers
{
    [Authorize]
    public class ReceivedController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly AccountsService customerServices;

        private readonly TransactionService transactionService;
        private readonly ReceivedService receivedService;
        private readonly RazorPagesReportingEngine reportingEngine;
        public ReceivedController(UserManager<IdentityUser> userManager, AccountsService customerServices, TransactionService transactionService, ReceivedService receivedService, RazorPagesReportingEngine reportingEngine)       
        {
            this.reportingEngine = reportingEngine;
            this.receivedService = receivedService;
            this.userManager = userManager;
            this.customerServices = customerServices;
            this.transactionService = transactionService;
        }
      
        public IActionResult IndexAsync()
        {
            var userId = userManager.GetUserId(User);
            var customer = customerServices.GetCustomer(userId);
            try
            {                 
                
                var received = receivedService.GetCustomerTransaction(userId, customer);
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


        public async Task<IActionResult> TransactionsReport([FromRoute]Guid accountId, [FromQuery]string searchString)
        {

            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                var received = receivedService.GetCustomerTransaction(userId, customer); var account = customer.GetAccount(accountId);
                var reportViewModel = new TransactionsReportViewModel
                {
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    Account = account.IBAN,
                    Currency = account.Currency,
                    Transactions = received,
                    Balance = account.Balance

                };
                var fileName = $"TrRep_{DateTime.UtcNow.ToShortDateString()}_{account.IBAN}.pdf";
                fileName = fileName.Replace('/', '_');
                fileName = fileName.Replace('\\', '_');
                fileName = fileName.Replace(':', '_');

                return await reportingEngine.RenderViewAsPdf("Accounts/TransactionsReport", reportViewModel, fileName); //PartialView("_TransactionsPartial", transactions);
            }
            catch (Exception e)
            {
                return BadRequest("Unable to process your request");
            }
        }


        [HttpPost]
        public IActionResult AddReceived([FromForm]NewPaymentViewModel paymentData)
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                var viewModel = new AddReceivedViewModel()
                {
                    
                    BankAccount = customer.BankAccounts
                };

                return View( viewModel);
            }
            catch (Exception e)
            {
                return BadRequest("Bad Input");
            }
        }

        [HttpPost]
        public IActionResult Create(AddReceivedViewModel viewModel)
        {
            transactionService.AddReceived(viewModel.Amount, viewModel.ExternalName, viewModel.ExternalIBAN, viewModel.BankAccountId);
            return RedirectToAction("Index");
        }

        public IActionResult ChangeTabel(int option)
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                List<BankAccount> aux = new List<BankAccount>();
                aux.Add(customer.BankAccounts.ElementAt(option));
                var viewModel = new AddReceivedViewModel()
                {

                    BankAccount = aux
                };

                return View( viewModel);
            }
            catch (Exception e)
            {
                return BadRequest("Bad Input");
            }
        }
        
    }
}