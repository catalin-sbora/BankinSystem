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

        [HttpGet("{searchString?}")]
        public IActionResult Index([FromQuery]string searchString)
        {
            var userId = userManager.GetUserId(User);
            var customer = customerServices.GetCustomer(userId);

            if (!String.IsNullOrEmpty(searchString))
            {
                try
                {
                    var transactionList = transactionService.SearchedTransactions(searchString, userId);
                    var viewModel = new PaymentsViewModel
                    {
                        BanksAccounts = customer.BankAccounts,
                        CustomerName = $"{customer.FirstName} {customer.LastName}",
                        CustomerPhoneNo = customer.ContactDetails?.PhoneNo,
                        Transactions = transactionList.OrderByDescending(transaction => transaction.Time)
                    };

                    return View(viewModel);
                }
                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                try
                {
                    var viewModel = new PaymentsViewModel()
                    {
                        CustomerName = $"{customer.FirstName} {customer.LastName}",
                        CustomerPhoneNo = customer.ContactDetails?.PhoneNo,
                        BanksAccounts = customer.BankAccounts,
                        Transactions = customerServices.GetAllTransaction(customer).OrderByDescending(transaction => transaction.Time)
                    };
                    return View(viewModel);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
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
                return BadRequest(e.Message);
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
    }
}