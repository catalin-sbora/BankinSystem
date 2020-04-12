using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using InternshipProject.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipProject.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/{id?}")]
    public class AccountsController : Controller
    {
        private readonly CustomerService customerServices;
        private readonly UserManager<IdentityUser> userManager;
        private readonly MetaDataService metaDataService;

        public AccountsController(CustomerService customerServices, MetaDataService metaDataService, UserManager<IdentityUser> userManager)
        {
            this.customerServices = customerServices;
            this.userManager = userManager;
            this.metaDataService = metaDataService;
        }
        public IActionResult Index()
        {
            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
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
                var customer = customerServices.GetCustomer(userId);
                var bankAccount = customerServices.GetCustomerBankAccount(customer, id);
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
        [Route("/[controller]/{accountId}/[action]")]
        public IActionResult Transactions([FromRoute]Guid accountId, [FromQuery]string searchString)
        {
            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                var transactions = customer.GetFilteredAccountTransactions(accountId, searchString);
                return PartialView("_TransactionsPartial", transactions);
            }
            catch (Exception e)
            {
                return BadRequest("Unable to process your request");
            }

        }
        
        [HttpGet]
        [Route("/[controller]/{accountId}/[action]")]
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
                customerServices.CreateAccountPayment(userId,
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
            catch (Exception e)
            {
                viewModelResult.PaymentStatus = NewPaymentStatus.Failed;
            }
            return PartialView("_NewPaymentPartial", viewModelResult);///RedirectToAction("Details", "Accounts", new { id = paymentData.SourceAccount.ToString() });                 
        }

        [HttpGet]
        [Route("/[controller]/{accountId}/[action]")]
        public IActionResult AccountBalance([FromRoute]Guid accountId)
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var bankAccount = customerServices.GetCustomerBankAccount(userId, accountId);
                return PartialView("_AccountBalancePartial", bankAccount);
            }
            catch (Exception)
            {
                return PartialView("_AccountBalancePartial");
            }
        }

    }
}