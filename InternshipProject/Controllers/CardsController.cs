using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternshipProject.ApplicationLogic.Model;
using InternshipProject.ViewModels.Cards;
using Microsoft.AspNetCore.Mvc;

namespace InternshipProject.Controllers
{
    public class CardsController : Controller
    {
        public IActionResult Index()
        {
            CardListViewModel bankAccount = new CardListViewModel()
            {
                Cards = new List<Card> {
                new Card() {SerialNumber = "1111-1111-1111-111" , OwnerName= "Mihnea" , ExpiryDate= new DateTime(2024 , 11 , 5)},
                new Card() {SerialNumber = "2222-2222-2222-2222", OwnerName= "Mihnea " , ExpiryDate= new DateTime(2025,11,24) },
                new Card() {SerialNumber = "3333-3333-3333-3333", OwnerName= "Mihnea" , ExpiryDate= new DateTime(2023,9,20)}}
            };
             return View(bankAccount );
            
        }
        public ActionResult CardPayments()
        {
            return View();
        }
    }
}