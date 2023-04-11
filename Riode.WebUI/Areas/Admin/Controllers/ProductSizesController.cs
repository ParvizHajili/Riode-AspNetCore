using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductSizesController : Controller
    {
        private readonly RiodeDbContext _context;

        public ProductSizesController(RiodeDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductSizes
        [Authorize(Policy = "admin.productsizes.index")]
        public async Task<IActionResult> Index()
        {
              return _context.Sizes != null ? 
                          View(await _context.Sizes.ToListAsync()) :
                          Problem("Entity set 'RiodeDbContext.Sizes'  is null.");
        }

        [Authorize(Policy = "admin.productsizes.details")]
        // GET: Admin/ProductSizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var productSize = await _context.Sizes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        [Authorize(Policy = "admin.productsizes.create")]
        // GET: Admin/ProductSizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductSizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "admin.productsizes.create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Abbr,Name,Description,Id,CreateByUserId,CreatedDate,DeletedByUserId,DeletedDate")] ProductSize productSize)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSize);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productSize);
        }

        // GET: Admin/ProductSizes/Edit/5
        [Authorize(Policy = "admin.productsizes.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var productSize = await _context.Sizes.FindAsync(id);
            if (productSize == null)
            {
                return NotFound();
            }
            return View(productSize);
        }

        // POST: Admin/ProductSizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "admin.productsizes.edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Abbr,Name,Description,Id,CreateByUserId,CreatedDate,DeletedByUserId,DeletedDate")] ProductSize productSize)
        {
            if (id != productSize.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSizeExists(productSize.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productSize);
        }

        [Authorize(Policy = "admin.productsizes.delete")]
        // GET: Admin/ProductSizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var productSize = await _context.Sizes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        [Authorize(Policy = "admin.productsizes.delete")]
        // POST: Admin/ProductSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sizes == null)
            {
                return Problem("Entity set 'RiodeDbContext.Sizes'  is null.");
            }
            var productSize = await _context.Sizes.FindAsync(id);
            if (productSize != null)
            {
                _context.Sizes.Remove(productSize);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSizeExists(int id)
        {
          return (_context.Sizes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
