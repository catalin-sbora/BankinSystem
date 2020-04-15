using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using InternshipProject.ViewModels.Cards;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternshipProject.Controllers
{
    public class CardsController : Controller
    {
        private TransactionService transactionService;
        private UserManager<IdentityUser> userManager;
        private AccountsService customerServices;
        private CardServices cardService;
        private ILogger<CardsController> logger;
        public CardsController(AccountsService customerServices, 
                                UserManager<IdentityUser> userManager, 
                                CardServices cardService,
                                TransactionService transactionService,
                                ILogger<CardsController> logger)
       
        {
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.cardService = cardService;
            this.customerServices = customerServices;
            this.logger = logger;
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
                logger.LogDebug("Failed to retrieve cards list {@Exception}", e);
                logger.LogError("Failed to retrieve cards list {ExceptionMessage}", e);
                return BadRequest("Unable to process your request");
            }
            
        }
       
        
        public ActionResult CardPayments(Guid Id ,[FromForm] CardTransactionsListViewModel model , string OwnerName , string SerialNumber)
        {
            
            string userId = userManager.GetUserId(User);
            try
            {
                
               if(model == null)
                {
                    model = new CardTransactionsListViewModel();
                }
                var customer = customerServices.GetCustomer(userId);
                var bankAccounts = customerServices.GetCustomerBankAccounts(userId);
                var card = cardService.GetCardByCardId(Id);
                model.CardId = Id;
                 
                
                var cardTransactions = cardService.GetFilteredCardTransactions(card.Id ,model.SearchBy,model.TransactionType);
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
                
                //CardTransactionsListViewModel cardTransactionsListViewModel = new CardTransactionsListViewModel();
                model.CardTransactions = cardTransactionViewModel;
                model.OwnerName = OwnerName;
                model.SerialNumber = SerialNumber;
                //cardTransactionsListViewModel.BankAccountId = bankAccount.Id;
               
                   
                
              
                return View(model);
            }
            catch(Exception e)
            {
                return BadRequest("Unable to process your request");
                
            }
        }
        
       

        [HttpPost]
        public IActionResult CreateCardTransaction([FromForm]TransactionViewModel viewModel)
        {
            var model = new TransactionViewModel { PaymentStatus = NewPaymentStatus.Failed };
            if(!ModelState.IsValid ||
                viewModel.IBan == null ||
                viewModel.ExternalName == null ||
                viewModel.Amount == 0)
                return PartialView("NewPaymentPartial", model);
            ModelState.Clear();
            try
            {
                var sourceAccountId = cardService.GetCardByCardId(viewModel.CardId).BankAccount.Id;
                var transaction = Transaction.Create(viewModel.Amount, sourceAccountId, viewModel.ExternalName, viewModel.IBan, null);
                
                 var cardTransaction = CardTransaction.Create(transaction, CardTransactionType.Online);
                transactionService.AddTransaction(transaction);
                var getCardTransaction = cardService.AddTransaction(cardTransaction);
                card.CardTransactions.Add(getCardTransaction);
                cardService.AddCardTransaction(card);
                var id = viewModel.CardId;
                // return RedirectToAction("CardPayments", new {Id=id , SearchBy = ""});
                model.PaymentMessage = "Done";
                model.PaymentStatus = NewPaymentStatus.Created;
            }
            catch(Exception e)
            {
                return BadRequest("Unable to process your request");
            }
            return PartialView("_NewPaymentPartial", model);
        }
    }
}