using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InternshipProject.Models;
using InternshipProject.ApplicationLogic.Services;
using Serilog;

namespace InternshipProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AccountsService customerServices;
        private readonly StatisticsService statisticsServices;
        public HomeController(ILogger<HomeController> logger, 
            AccountsService customerServices, 
            StatisticsService statisticsServices)
        {
            _logger = logger;
            this.customerServices = customerServices;
            this.statisticsServices = statisticsServices;
        }
        
        public IActionResult Index()
        {
            
            _logger.LogError("******* Error message *****");
            _logger.LogWarning("****Warning Message *****");
            _logger.LogInformation("****Information Message *****");
            _logger.LogDebug("****** Debug Message *******");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogDebug("Error enocuntered TraceId: " + HttpContext.TraceIdentifier);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
