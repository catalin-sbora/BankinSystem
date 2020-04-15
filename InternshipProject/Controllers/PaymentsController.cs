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
        private readonly AccountsService accountService;
        private readonly PaymentsService paymentsService;

        public PaymentsController(UserManager<IdentityUser> userManager, 
            AccountsService accountsService, 
            PaymentsService paymentsService)
        {
            this.userManager = userManager;
            this.accountService = accountsService;
            this.paymentsService = paymentsService;
        }

        [HttpGet]
        public IActionResult Index([FromQuery]string searchString)
        {
            var userId = userManager.GetUserId(User);
            var customer = accountService.GetCustomer(userId);
            
            if (!String.IsNullOrEmpty(searchString))
            {
                try
                {
                    var paymentsList = paymentsService.GetFilteredPayments(userId, searchString);
                    var viewModel = new PaymentsViewModel
                    {
                        BanksAccounts = customer.BankAccounts,
                        CustomerName = $"{customer.FirstName} {customer.LastName}",
                        CustomerPhoneNo = customer.ContactDetails?.PhoneNo,
                        Transactions = paymentsList.OrderByDescending(payment => payment.Time)
                    };

                    return View(viewModel);
                }
                catch (Exception e)
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
                        Transactions = paymentsService.GetCustomerPayments(userId).OrderByDescending(payment => payment.Time)
                    };

                    return View(viewModel);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpGet]
        public IActionResult New()
        {
            var userId = userManager.GetUserId(User);
            var customer = accountService.GetCustomer(userId);
            var viewModel = new NewPaymentViewModel()
            {
                BanksAccount = customer.BankAccounts,
                PaymentStatus = NewPaymentStatus.NotInitiated
            };
            return PartialView("_NewPaymentPartial", viewModel);
        }

        [HttpPost]
        public IActionResult New([FromForm]NewPaymentViewModel paymentData)
        {
            NewPaymentViewModel viewModelResult = new NewPaymentViewModel()
            {
                PaymentStatus = NewPaymentStatus.Failed
            };

            if (!ModelState.IsValid ||
                paymentData == null ||
                paymentData.BankAccountId == null ||
                paymentData.Amount == null
                )
                return PartialView("_NewPaymentPartial", viewModelResult);

            ModelState.Clear();
            try
            {
                var userId = userManager.GetUserId(User);
                paymentsService.AddPayment(paymentData.BankAccountId,
                                           paymentData.Amount,
                                           paymentData.ExternalName,
                                           paymentData.ExternalIBAN);

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
            //return PartialView("_NewPaymentPartial", viewModelResult);
            return RedirectToAction("Index");
        }

        public IActionResult Details(string Id)
        {
            var payment = paymentsService.GetById(Id);
            return View(payment);
        }
    }
}

