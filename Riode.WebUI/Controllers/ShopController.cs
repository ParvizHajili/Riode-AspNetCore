using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            RiodeDbContext dbContext = new RiodeDbContext();
            List<Brand> brands = dbContext.Brands
                .Where(b => b.DeletedByUserId == null)
                .ToList();

            return View(brands);
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
