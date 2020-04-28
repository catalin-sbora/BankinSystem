using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ApplicationLogic.Services;
using InternshipProject.EFDataAccess;
using InternshipProject.ViewModels.Cards;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternshipProject.Controllers
{
    public class CardsController : Controller
    {
        private PaymentsService transactionService;
        private UserManager<IdentityUser> userManager;
        private CustomerService customerService;
        private AccountsService accountsService;
        private CardServices cardService;
        private MetaDataService metaDataService;
        private ILogger logger;
        public CardsController(CustomerService customerService,
                                AccountsService accountsService,
                                CardServices cardService,
                                PaymentsService transactionService,
                                MetaDataService metaDataService,
                                UserManager<IdentityUser> userManager,                                
                                ILogger<CardsController> logger)
       
        {
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.cardService = cardService;
            this.customerService = customerService;
            this.accountsService = accountsService;
            this.metaDataService = metaDataService;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            BankingDbContext context ;
            string userId = userManager.GetUserId(User);

            try
            {
                var customer = customerService.GetCustomerFromUserId(userId);
                var bankAccounts = accountsService.GetCustomerBankAccounts(userId);

                var cards = cardService.GetCardsForCustomer(userId);
                CompleteCardsViewModel cardList = new CompleteCardsViewModel();
                cardList.Cards = new List<CardWithColorViewModel>();
                
                foreach (var card in cards)
                {
                    CardWithColorViewModel temp = new CardWithColorViewModel();
                    temp.Card = card;
                    temp.CardColor = metaDataService.GetMetaDataForCard(card.Id);
                    cardList.Cards.Add(temp);
                }
                return View(cardList);
            }
            catch(Exception e)
            {
                logger.LogDebug("Failed to retrieve cards list {@Exception}", e);
                logger.LogError("Failed to retrieve cards list {ExceptionMessage}", e.Message);
                return BadRequest("Unable to process your request");
            }
            
        }
       
        
        public ActionResult CardPayments(Guid Id ,[FromForm] CardTransactionsListViewModel model )
        {

            
            string userId = userManager.GetUserId(User);
            try
            {
                
               if(model == null)
                {
                    model = new CardTransactionsListViewModel();
                }
                var customer = customerService.GetCustomerFromUserId(userId);
                var bankAccounts = accountsService.GetCustomerBankAccounts(userId);
                var card = cardService.GetCardByCardId(Id);
                model.OwnerName = card.OwnerName;
                model.SerialNumber = card.SerialNumber;
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
                
                
                model.CardTransactions = cardTransactionViewModel;                 
                
              
                return View(model);
            }
            catch(Exception e)
            {
                logger.LogError("Unable to retrieve card payments {ExceptionMessage}", e.Message);
                logger.LogDebug("Unable to retrieve card payments {@Exception}",e);
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
                var userId = userManager.GetUserId(User);
                cardService.MakeOnlinePayment(userId, 
                                                viewModel.CardId, 
                                                viewModel.Amount, 
                                                viewModel.ExternalName, 
                                                viewModel.IBan);
                
                model.PaymentMessage = "Done";
                model.PaymentStatus = NewPaymentStatus.Created;
            }
            catch(Exception e)
            {
                logger.LogError("Unable to execute payment {ExceptionMessage}", e.Message);
                logger.LogDebug("Unable to execute payment {@Exception}", e);
                return BadRequest("Unable to process your request");
            }
            return PartialView("_NewPaymentPartial", model);
        }
    }
}