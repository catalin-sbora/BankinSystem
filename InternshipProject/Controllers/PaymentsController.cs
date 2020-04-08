using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using InternshipProject.ViewModels.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipProject.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly CustomerService customerServices;

        public PaymentsController(UserManager<IdentityUser> userManager, CustomerService customerServices)
        {
            this.userManager = userManager;
            this.customerServices = customerServices;
        }

        public IActionResult IndexAsync()
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                var viewModel = new PaymentsViewModel()
                {
                    CustomerName = $"{customer.FirstName} {customer.LastName}",
                    CustomerPhoneNo = customer.ContactDetails?.PhoneNo,
                    BanksAccounts = customer.BankAccounts
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