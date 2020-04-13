
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
        private readonly AccountsService customerServices;

        public ReceivedController(UserManager<IdentityUser> userManager, AccountsService customerServices)
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
                var viewModel = new ReceivedListViewModel()
                {
                    //IsSelected = viewModel.IsSelected,
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    PhoneNo = customer.ContactDetails?.PhoneNo,
                    BankAccounts = customer.BankAccounts,
                    Transactions= customer.BankAccounts.ElementAt(0).Transactions,
                    

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