using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Entities;
using Riode.WebUI.Models.ViewModels;

namespace Riode.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private readonly RiodeDbContext _context;
        public ShopController(RiodeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ShopFilterViewModel viewModel = new();

            viewModel.Brands = _context.Brands
                .Where(b => b.DeletedByUserId == null)
                .ToList();

            viewModel.ProductColors = _context.ProductColors
                .Where(x => x.DeletedByUserId == null)
                .ToList();

            viewModel.ProductSizes = _context.ProductSizes
                .Where(x => x.DeletedByUserId == null)
                .ToList();

            viewModel.Categories = _context.Categories
                .Include(x => x.Children)
                .ThenInclude(x => x.Children)
                .Where(x => x.DeletedByUserId == null && x.ParentId == null)
                .ToList();

            viewModel.Products = _context.Products
                .Include(x => x.Images.Where(x => x.IsMain == true))
                .Include(x => x.Brand)
                .Where(x => x.DeletedByUserId == null)
                .ToList();

            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
           

            var product = _context.Products
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
