
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ViewModels.Accounts;
using InternshipProject.ViewModels.Received;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace InternshipProject.Controllers
{
    [Authorize]
    public class ReceivedController : Controller
    {
        public IActionResult Index()
        {
          
            // get the proper bank account for this but idk how
            ReceivedListViewModel viewModel = new ReceivedListViewModel()
            {

                CustomerName = "Alex",
                PhoneNo = "0281726472",
                
                BankAccounts = new List<BankAccount> {
                    new BankAccount{
                        IBAN = "ROSDasdasd", Balance =0,Transactions = new List<Transaction> 
                        {
                   new Transaction{ExternalIBAN = "asd",Amount =123 },
                   new Transaction{ExternalIBAN = "qwer",Amount=324 },
                   new Transaction{ExternalIBAN = "agd" }
                }
                } 
                }
                
            };
            return View(viewModel);
        }
    }
}