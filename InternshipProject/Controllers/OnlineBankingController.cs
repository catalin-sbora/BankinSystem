using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipProject.Controllers
{
    [Authorize]
    public class OnlineBankingController : Controller
    {
        private readonly CustomerServices customerServices;
        private readonly UserManager<IdentityUser> userManager;

        public OnlineBankingController(CustomerServices customerServices, UserManager<IdentityUser> userManager)
        {

            this.userManager = userManager;
            this.customerServices = customerServices;
        }
        // GET: OnlineBanking
        public IActionResult Index()
        {

            var currentUser = userManager.GetUserId(User);
            var customerId = customerServices.GetCustomerIdFromUserId(currentUser);
            IEnumerable<BankAccount> bankAccounts = customerServices.GetCustomerBankAccounts(customerId);

            return View(bankAccounts);
        }

    }
}