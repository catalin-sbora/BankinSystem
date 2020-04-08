using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using InternshipProject.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipProject.Controllers
{
  //  [Authorize]
    public class AccountsController : Controller
    {
        private readonly CustomerServices customerServices;
        private readonly UserManager<IdentityUser> userManager;

        public AccountsController(CustomerServices customerServices, UserManager<IdentityUser> userManager)
        {
            this.customerServices = customerServices;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            
            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                AccountsListViewModel viewModel = new AccountsListViewModel()
                {
                    BankAccounts = customer.BankAccounts,
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