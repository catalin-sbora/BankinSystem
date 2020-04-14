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
        public CardsController(CustomerService customerServices, UserManager<IdentityUser> userManager, CardServices cardService,TransactionService transactionService )
       
        {
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.cardService = cardService;
            this.customerServices = customerServices;
        }
        public IActionResult Index()
        {
            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                var bankAccounts = customerServices.GetCustomerBankAccounts(userId);

                List<Card> cards = new List<Card>();
                CompleteCardsViewModel cardList = new CompleteCardsViewModel();
                cardList.Cards = new List<CardWithColorViewModel>();
                foreach (var bankAccount in bankAccounts)
                {
                    cards.AddRange(customerServices.GetCardsByUserID(bankAccount.Id.ToString()));

                }


                foreach (var card in cards)
                {

                    CardWithColorViewModel temp = new CardWithColorViewModel();
                    temp.Card = card;
                    temp.CardColor = customerServices.GetCardColor(card.Id);
                    cardList.Cards.Add(temp);
                }



                return View(cardList);
            }
            catch(Exception e)
            {
                return BadRequest("Unable to process your request");
            }
            
        }
       
        
        public ActionResult CardPayments(Guid Id ,[FromForm] string? SearchBy , string Type)
        {
            string userId = userManager.GetUserId(User);
            try
            {
                var customer = customerServices.GetCustomer(userId);
                var bankAccounts = customerServices.GetCustomerBankAccounts(userId);
                var card = cardService.GetCardByCardId(Id);
                 BankAccount bankAccount = null;
                
                foreach (var bankAccountIt in bankAccounts)
                {
                    if (bankAccountIt.Id == card.BankAccount.Id)
                    {
                        bankAccount = bankAccountIt;
                    }
                }
                var transactions = transactionService.GetTransactionsFromBankAccount(bankAccount.Id);
                var cardTransactions = cardService.GetCardTransactions(transactions);
                List<CardTransactionViewModel> cardTransactionViewModel = new List<CardTransactionViewModel>();
                foreach (var transaction in cardTransactions)
                {
                    CardTransactionViewModel temp = new CardTransactionViewModel();
                   
                    temp.Amount = transaction.Transaction.Amount;
                    temp.Name = transaction.Transaction.ExternalName;
                    temp.TransactionType = transaction.TransactionType.ToString();
                    temp.DateTime = transaction.Transaction.Time;
                    cardTransactionViewModel.Add(temp);
                }
                
                CardTransactionsListViewModel cardTransactionsListViewModel = new CardTransactionsListViewModel();
                cardTransactionsListViewModel.CardTransactions = cardTransactionViewModel;
                cardTransactionsListViewModel.BankAccountId = bankAccount.Id;
                if(SearchBy != null || Type != null)
                {
                CardTransactionsListViewModel cardTransactionsList = new CardTransactionsListViewModel();
                    cardTransactionsList.CardTransactions = new List<CardTransactionViewModel>();
                    cardTransactionsList.BankAccountId = cardTransactionsListViewModel.BankAccountId;
                    if(SearchBy != null)
                    foreach(var transaction in cardTransactionsListViewModel.CardTransactions)
                    {
                        if(transaction.Name.Contains(SearchBy) || transaction.DateTime.ToString().Contains(SearchBy) || transaction.Amount.ToString().Contains(SearchBy))
                        {
                             cardTransactionsList.CardTransactions.Add(transaction);
                        }
                        
                    }
                    else if(Type != null)
                    {
                        foreach (var transaction in cardTransactionsListViewModel.CardTransactions)
                        {
                            if (transaction.TransactionType.ToString() == Type)
                            {
                                cardTransactionsList.CardTransactions.Add(transaction);
                            }

                        }
                    }
                    return View(cardTransactionsList);
                }
              
                return View(cardTransactionsListViewModel);
            }
            catch(Exception e)
            {
                return BadRequest("Unable to process your request");
                
            }
        }
        public IActionResult NewCardTransaction()
        {
            var userId = userManager.GetUserId(User);
            try
            {
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
            catch(Exception e)
            {
                return BadRequest("Unable to process your request");
            }
        }

        [HttpPost]
        public IActionResult CreateCardTransaction([FromForm]TransactionViewModel viewModel)
        {
            try
            {
               
                var transaction = Transaction.Create(viewModel.Amount, viewModel.ExternalName, viewModel.IBan, null);
                transaction.BankAccountId = cardService.GetCardByCardId(viewModel.CardId).BankAccount.Id;
                 var cardTransaction = CardTransaction.Create(transaction, CardTransactionType.Online);
                transactionService.Add(transaction);
                cardService.AddTransaction(cardTransaction);
                var id = viewModel.CardId;
                return RedirectToAction("CardPayments", new {Id=id , SearchBy = ""});
            }
            catch(Exception e)
            {
                return BadRequest("Unable to process your request");
            }
        }
    }
}