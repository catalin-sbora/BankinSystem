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
        private UserManager<IdentityUser> userManager;
        private CustomerService customerServices;
        public CardsController(CustomerService customerServices, UserManager<IdentityUser> userManager)
       
        {
            this.userManager = userManager;
            
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
        public ActionResult CardPayments()
        {
            return View();
        }
    }
}