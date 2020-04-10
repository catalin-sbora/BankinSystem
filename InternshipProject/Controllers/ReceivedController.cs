
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
namespace InternshipProject.Controllers
{
    [Authorize]
    public class ReceivedController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly CustomerService customerServices;
        private readonly TransactionService transactionService;
        public ReceivedController(UserManager<IdentityUser> userManager, CustomerService customerServices, TransactionService transactionService)
        {
            this.userManager = userManager;
            this.customerServices = customerServices;
            this.transactionService = transactionService;
        }
        //[HttpPost]
     
        public IActionResult IndexAsync()
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                List<Transaction> received = new List<Transaction>() ;
                foreach(var account in customer.BankAccounts)
                {
                    for(int i=0;i<account.Transactions.Count;i++)
                    foreach (var transaction in account.Transactions)
                        if (account.Transactions.ElementAt(i).Amount > 0)
                            received.Add(account.Transactions.ElementAt(i));
                }
                
                
                var viewModel = new ReceivedListViewModel()
                {
                    //IsSelected = viewModel.IsSelected,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    PhoneNo = customer.ContactDetails?.PhoneNo,
                    BankAccounts = customer.BankAccounts,
                    //Transactions = received
            };
                return View(viewModel);
            }
            catch (Exception e) 
            {
                return BadRequest("Unable to retrieve data");
            }
        }

        public IActionResult AddReceived()
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                var viewModel = new AddReceivedViewModel()
                {
                    
                    BankAccount = customer.BankAccounts
                };

                return View(viewModel);
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
        
    }
}