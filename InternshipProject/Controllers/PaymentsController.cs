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
        private readonly TransactionService transactionService;

        public PaymentsController(UserManager<IdentityUser> userManager, CustomerService customerServices, TransactionService transactionService)
        {
            this.userManager = userManager;
            this.customerServices = customerServices;
            this.transactionService = transactionService;
        }

        public IActionResult Index()
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

        public IActionResult New()
        {
            var userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                var viewModel = new NewPaymentViewModel()
                {
                    BanksAccount = customer.BankAccounts
                };

                return View(viewModel);
            }
            catch(Exception e)
            {
                return BadRequest("No");
            }
        }

        [HttpPost]
        public IActionResult Create(NewPaymentViewModel viewModel)
        {
            transactionService.Add(viewModel.Amount, viewModel.ExternalName, viewModel.ExternalIBAN, viewModel.BankAccountId);
            return RedirectToAction("Index");
        }

        public IActionResult Details(string Id)
        {
            var transaction = transactionService.GetById(Id);
            return View(transaction);
        }

        public IActionResult SearchPayments(string searchString)
        {
            IEnumerable<Transaction> transactionList = new List<Transaction>();
            if (!String.IsNullOrEmpty(searchString))
            {
                var userId = userManager.GetUserId(User);
                transactionList = transactionService.SearchedTransactionsByAmount(searchString, userId);
            }

            return View(transactionList);
        }

    }
}