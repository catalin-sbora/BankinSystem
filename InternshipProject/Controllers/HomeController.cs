using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InternshipProject.Models;
using InternshipProject.ApplicationLogic.Services;

namespace InternshipProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomerService customerServices;
        private readonly StatisticsServices statisticsServices;
        public HomeController(ILogger<HomeController> logger, CustomerService customerServices, StatisticsServices statisticsServices)
        {
            _logger = logger;
            this.customerServices = customerServices;
            this.statisticsServices = statisticsServices;
        }
        
        public IActionResult Index()
        {
           
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
