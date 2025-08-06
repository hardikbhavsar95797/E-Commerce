using E_Commerce.Context;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class CategoryController(E_CommerceContext context) : Controller
    {
        private readonly E_CommerceContext _context = context;

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(x => !x.Is_Deleted).ToListAsync();
            return View(categories);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Category category = new()
            {
                Name = model.Name,
                Description = model.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Category category = new()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (category == null) return NotFound();

            category.Is_Deleted = true;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}