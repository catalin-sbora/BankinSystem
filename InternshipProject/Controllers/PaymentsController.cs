using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ViewModels.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipProject.Controllers
{
    //[Authorize]
    public class PaymentsController : Controller
    {
        public IActionResult Index()
        {

            
            PaymentsViewModel viewModel = new PaymentsViewModel()
            {
                CustomerName = "John Doe",
                CustomerPhoneNo = "0723 972 110",
                BanksAccounts = new List<BankAccount>
                {
                   /* new BankAccount
                    {
                        IBAN = "ROSDasdasd",
                        Transactions = new List<Transaction>
                        {
                            new Transaction{Amount = 20, ExternalIBAN = "ROSDasdasd", Time = new DateTime(2019,04,03)},
                            new Transaction{Amount = 20, ExternalIBAN = "ROgdfgdfgd", Time = new DateTime(2020,11,23)},
                            new Transaction{Amount = 20, ExternalIBAN = "ROreterter", Time = new DateTime(2002,09,12)}
                        }
                    },
                    new BankAccount
                    {
                        IBAN = "ROSDvbcvb",
                        Transactions = new List<Transaction>
                        {
                            new Transaction{Amount = 3, ExternalIBAN = "ROrfeter", Time = new DateTime(2002,09,12)}
                        }
                    },
                    new BankAccount
                    {
                        IBAN = "ROSgfhfgh",
                        Transactions = new List<Transaction>
                        {
                            new Transaction{Amount = 20, ExternalIBAN = "ROgdfgdfgd", Time = new DateTime(2020,11,23)},
                            new Transaction{Amount = 20, ExternalIBAN = "ROreterter", Time = new DateTime(2002,09,12)}
                        }
                    }*/
                }
            };
            return View(viewModel);
        }
    }
}