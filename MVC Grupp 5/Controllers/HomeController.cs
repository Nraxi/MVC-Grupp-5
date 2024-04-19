using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Grupp_5.Models;
using System.Diagnostics;
using MVC_Grupp_5.Data;

namespace MVC_Grupp_5.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }


        //public IActionResult Privacy()
        //{
        //    return View();
        //}
        private readonly MVC_Grupp_5Context _context;

        public HomeController(MVC_Grupp_5Context context) // Injicera din DbContext i konstruktorn
        {
            _context = context;
        }

        public IActionResult AnnanFunc(string lol) {

         
           return Receipt(lol);
        }
        private IActionResult Receipt(string id)
        {
            ViewData["RegNr"] = id;
            return View("Receipt");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
