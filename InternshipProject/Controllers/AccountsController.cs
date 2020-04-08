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
    }
}