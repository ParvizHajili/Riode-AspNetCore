using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Entities;
using Riode.WebUI.Models.ViewModels;

namespace Riode.WebUI.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            RiodeDbContext dbContext = new RiodeDbContext();
            ShopFilterViewModel viewModel = new();

            viewModel.Brands = dbContext.Brands
                .Where(b => b.DeletedByUserId == null)
                .ToList();

            viewModel.ProductColors = dbContext.ProductColors
                .Where(x => x.DeletedByUserId == null)
                .ToList();

            viewModel.ProductSizes = dbContext.ProductSizes
                .Where(x => x.DeletedByUserId == null)
                .ToList();

            viewModel.Categories = dbContext.Categories
                .Include(x => x.Children)
                .ThenInclude(x => x.Children)
                .Where(x => x.DeletedByUserId == null && x.ParentId == null)
                .ToList();

            viewModel.Products = dbContext.Products
                .Include(x => x.Images.Where(x => x.IsMain == true))
                .Include(x => x.Brand)
                .Where(x => x.DeletedByUserId == null)
                .ToList();

            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            RiodeDbContext dbContext = new RiodeDbContext();

            var product = dbContext.Products
                .Include(x=>x.Brand)
                .Include(x => x.Images)
                .FirstOrDefault(x => x.Id == id && x.DeletedByUserId == null);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
