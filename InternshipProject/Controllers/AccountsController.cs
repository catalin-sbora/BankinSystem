using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Services;
using InternshipProject.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RazorPagesReporting;

namespace InternshipProject.Controllers
{
    [Authorize]   
    public class AccountsController : Controller
    {
        private readonly AccountsService accountsService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly MetaDataService metaDataService;
        private readonly RazorPagesReportingEngine reportingEngine;
        public AccountsController(AccountsService customerServices, 
                                  MetaDataService metaDataService, 
                                  UserManager<IdentityUser> userManager,
                                  RazorPagesReportingEngine reportingEngine)
        {
            this.accountsService = customerServices;
            this.userManager = userManager;
            this.metaDataService = metaDataService;
            this.reportingEngine = reportingEngine;
        }
        public IActionResult Index()
        {
            string userId = userManager.GetUserId(User);
            try
            {
                var customer = accountsService.GetCustomer(userId);
                List<BankAccountViewModel> accountViewModels = new List<BankAccountViewModel>();

                foreach (var bankAccount in customer.BankAccounts)
                {
                    accountViewModels.Add(new BankAccountViewModel
                    {
                        BankAccount = bankAccount,
                        MetaData = metaDataService.GetMetaDataForBankAccount(bankAccount.Id)
                    });
                }
                AccountsListViewModel viewModel = new AccountsListViewModel()
                {
                    BankAccounts = accountViewModels,
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
        public IActionResult Details([FromRoute]Guid id)
        {
            string userId = userManager.GetUserId(User);
            try
            {
                var customer = accountsService.GetCustomer(userId);
                var bankAccount = accountsService.GetCustomerBankAccount(customer, id);
                var viewModel = new BankAccountViewModel
                {
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    CustomerContact = customer.ContactDetails?.PhoneNo,
                    BankAccount = bankAccount,
                    MetaData = metaDataService.GetMetaDataForBankAccount(bankAccount.Id)
                };
                return View(viewModel);
            }
            catch (Exception e)
            {
                return BadRequest("Unable to process your request");
            }
        }
        [HttpGet]
       
        public IActionResult Transactions([FromRoute]Guid accountId, [FromQuery]string searchString)
        {
            
            string userId = userManager.GetUserId(User);
            try
            {
                var customer = accountsService.GetCustomer(userId);
                var transactions = customer.GetFilteredAccountTransactions(accountId, searchString);
                var partialResult = PartialView("_TransactionsPartial", transactions);
                return partialResult;
            }
            catch (Exception e)
            {
                return BadRequest("Unable to process your request");
            }
        }

        
        public async Task<IActionResult> TransactionsReport([FromRoute]Guid accountId, [FromQuery]string searchString)
        {    
            
            string userId = userManager.GetUserId(User);
            try
            {
                var customer = accountsService.GetCustomer(userId);
                var transactions = customer.GetFilteredAccountTransactions(accountId, searchString);
                var account = customer.GetAccount(accountId);
                var reportViewModel = new TransactionsReportViewModel
                {
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    Account = account.IBAN,
                    Currency = account.Currency,
                    Transactions = transactions,
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

        [HttpGet]       
        public IActionResult NewPayment(string accountId)
        {
            var accountGuid = Guid.Empty;
            Guid.TryParse(accountId, out accountGuid);
            return PartialView("_NewPaymentPartial", new NewPaymentViewModel { SourceAccount = accountGuid, PaymentStatus = NewPaymentStatus.NotInitiated });
        }

        [HttpPost]
        public IActionResult NewPayment([FromForm]NewPaymentViewModel paymentData)
        {
            NewPaymentViewModel viewModelResult = new NewPaymentViewModel()
            {
                PaymentStatus = NewPaymentStatus.Failed
            };
            if (!ModelState.IsValid || 
                paymentData == null || 
                paymentData.SourceAccount == null ||
                paymentData.Amount == null
                )
                return PartialView("_NewPaymentPartial", viewModelResult);
            
            ModelState.Clear();
            try
            {
                var userId = userManager.GetUserId(User);
                accountsService.CreateAccountPayment(userId,
                                                      paymentData.SourceAccount.Value,
                                                      paymentData.Amount.Value,
                                                      paymentData.DestinationName,
                                                      paymentData.DestinationIBAN,
                                                      paymentData.Details);

                viewModelResult.PaymentMessage = "Done";
                viewModelResult.PaymentStatus = NewPaymentStatus.Created;
            }
            catch (NotEnoughFundsException)
            {
                viewModelResult.PaymentStatus = NewPaymentStatus.Failed;
                viewModelResult.PaymentMessage = "Not enough funds available";
            }
            catch (WrongCurrencyException)
            {
                viewModelResult.PaymentStatus = NewPaymentStatus.Failed;
                viewModelResult.PaymentMessage = "Payments not allowed for accounts with different currency";
            }
            catch (Exception e)
            {
                viewModelResult.PaymentStatus = NewPaymentStatus.Failed;
                viewModelResult.PaymentMessage = "Unexpected error occured";
            }
            return PartialView("_NewPaymentPartial", viewModelResult);///RedirectToAction("Details", "Accounts", new { id = paymentData.SourceAccount.ToString() });                 
        }

        [HttpGet]
        public IActionResult AccountBalance([FromRoute]Guid accountId)
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var bankAccount = accountsService.GetCustomerBankAccount(userId, accountId);
                return PartialView("_AccountBalancePartial", bankAccount);
            }
            catch (Exception)
            {
                return PartialView("_AccountBalancePartial");
            }
        }

   

    }
}