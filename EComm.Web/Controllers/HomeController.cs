using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EComm.Web.Models;
using EComm.Data;
using Microsoft.Extensions.Logging;

namespace EComm.Web.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> log)
        {
            logger = log;
        }
        public IActionResult Index()
        {
            logger.LogDebug("Bruce was here!");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
