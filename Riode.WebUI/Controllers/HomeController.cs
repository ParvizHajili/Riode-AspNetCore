using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Models;
using Riode.WebUI.Models.DataContexts;
using System.Diagnostics;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly RiodeDbContext _context;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, RiodeDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Faqs()
        {
            return View();
        }
    }
}