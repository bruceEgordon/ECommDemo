using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EComm.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/{statusCode:int}")]
        public IActionResult Index(int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            return View("Error");
        }
    }
}