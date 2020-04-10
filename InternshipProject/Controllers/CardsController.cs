using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using InternshipProject.ViewModels.Cards;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipProject.Controllers
{
    public class CardsController : Controller
    {
        private TransactionService transactionService;
        private UserManager<IdentityUser> userManager;
        private CustomerService customerServices;
        private CardServices cardService;
        public CardsController(CustomerService customerServices, UserManager<IdentityUser> userManager, CardServices cardService,TransactionService transactionService)
       
        {
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.cardService = cardService;
            this.customerServices = customerServices;
        }
        public IActionResult Index()
        {
            string userId = userManager.GetUserId(User);
            var customer = customerServices.GetCustomer(userId);
            var bankAccounts = customerServices.GetCustomerBankAccounts(userId);
            
            List<Card> cards = new List<Card>();
            CompleteCardsViewModel cardList = new CompleteCardsViewModel();
            cardList.Cards = new List<CardWithColorViewModel>();
            foreach(var bankAccount in bankAccounts)
            {
                cards.AddRange( customerServices.GetCardsByUserID(bankAccount.Id.ToString()));
               
            }
            
          
            foreach(var card in cards)
            {
              
                CardWithColorViewModel temp = new CardWithColorViewModel();
                temp.Card = card;
                temp.CardColor = customerServices.GetCardColor(card.Id);
                cardList.Cards.Add(temp);
            }
            
        

             return View( cardList);
            
        }
        public ActionResult Search()
        {
            
            return View();
        }
        public ActionResult CardPayments(Guid id)
        {
            string userId = userManager.GetUserId(User);
            var customer = customerServices.GetCustomer(userId);
            var bankAccounts = customerServices.GetCustomerBankAccounts(userId);
            var card = cardService.GetCardByCardId(id);
            BankAccount bankAccount = null;
            foreach(var bankAccountIt in bankAccounts)
            {
                if(bankAccountIt.Id == card.BankAccount.Id)
                {
                    bankAccount = bankAccountIt;
                }
            }
            List<CardTransactionViewModel> cardTransactionViewModel = new List<CardTransactionViewModel>();
            var transactions = transactionService.GetTransactionsFromBankAccount(bankAccount.Id);
            foreach(var transaction in transactions)
            {
                CardTransactionViewModel temp = new CardTransactionViewModel();
                temp.Amount = transaction.Amount;
                temp.Name = transaction.ExternalName;
                temp.TransactionType = "Online";
                temp.DateTime = transaction.Time;
                cardTransactionViewModel.Add(temp);
            }

            CardTransactionsListViewModel cardTransactionsListViewModel = new CardTransactionsListViewModel();
            cardTransactionsListViewModel.CardTransactions = cardTransactionViewModel;
            return View(cardTransactionsListViewModel);
        }
        public IActionResult NewCardTransaction()
        {
            var userId = userManager.GetUserId(User);
            var customer = customerServices.GetCustomer(userId);
            var bankAccounts = customerServices.GetCustomerBankAccounts(userId);
            List<Card> cards = new List<Card>();
            foreach (var bankAccount in bankAccounts)
            {
                cards.AddRange(customerServices.GetCardsByUserID(bankAccount.Id.ToString()));

            }
          
            TransactionViewModel transactionViewModel = new TransactionViewModel()
            {
                CardList = cards
            };

            return View(transactionViewModel);
        }

        [HttpPost]
        public IActionResult CreateCardTransaction(TransactionViewModel viewModel)
        {
            cardService.AddTransaction(viewModel.Amount, viewModel.IBan, viewModel.CardId);
            return RedirectToAction("Index");
        }
    }
}