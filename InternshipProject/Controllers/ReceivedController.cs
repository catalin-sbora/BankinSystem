
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

        public ReceivedController(UserManager<IdentityUser> userManager, CustomerService customerServices)
        {
            this.userManager = userManager;
            this.customerServices = customerServices;
        }
        //[HttpPost]
     
        public IActionResult IndexAsync()
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                List<Transaction> received = new List<Transaction>() ;
                for (var i = 0; i < customer.BankAccounts.ElementAt(0).Transactions.Count(); i++)
                {
                    if (customer.BankAccounts.ElementAt(0).Transactions.ElementAt(i).Amount > 0)
                        received.Add(customer.BankAccounts.ElementAt(0).Transactions.ElementAt(i));
                }
                var viewModel = new ReceivedListViewModel()
                {
                    //IsSelected = viewModel.IsSelected,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    PhoneNo = customer.ContactDetails?.PhoneNo,
                    BankAccounts = customer.BankAccounts,

                    Transactions = received
            };
                return View(viewModel);
            }
            catch (Exception e)
            {
                return BadRequest("Unable to retrieve data");
            }
        }
   
    }
}