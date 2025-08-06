using E_Commerce.Context;
using E_Commerce.Helper;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace E_Commerce.Controllers
{
    [Authorize]
    public class HomeController(E_CommerceContext context, IWebHostEnvironment hostEnvironment) : Controller
    {
        private readonly E_CommerceContext _context = context;

        private readonly IWebHostEnvironment _hostEnvironment = hostEnvironment;

        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchQuery, int? categoryId, decimal? minPrice, decimal? maxPrice, int? minRating, int page = 1)
        {
            int pageSize = 6;
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductMedias)
                .Where(p => !p.Is_Deleted)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchQuery))
                query = query.Where(p => p.Name.Contains(searchQuery));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            //if (minRating.HasValue)
            //    query = query.Where(p => p.Rating >= minRating.Value); // assuming product has Rating

            int totalCount = query.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new ProductFilterViewModel
            {
                SearchQuery = searchQuery,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinRating = minRating,
                HasPreviousPage = page > 1,
                HasNextPage = page < totalPages,
                Categories = [.. _context.Categories],
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin,Seller")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => !c.Is_Deleted), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin,Seller")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model, List<IFormFile> productImages)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            Product product = new()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Quantity = model.Quantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (productImages != null && productImages.Count != 0)
            {
                string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, $"Uploads/Products/{product.Id}");          
                Directory.CreateDirectory(uploadPath);

                List<ProductMedia> productMedias = [];

                foreach (var image in productImages)
                {
                    if (image.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        var productMedia = new ProductMedia
                        {
                            ProductId = product.Id,
                            ImagePath = $"/Uploads/Products/{product.Id}/" + fileName
                        };

                        productMedias.Add(productMedia);
                    }
                }

                await _context.ProductMedias.AddRangeAsync(productMedias);
                await _context.SaveChangesAsync();
            }
            TempData["Success"] = "Product Created successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductMedias)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Is_Deleted);

            if (product == null)
                return NotFound();

            return View(product);
        }


        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.Where(x =>x.Id == id).FirstOrDefaultAsync();
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = "Admin,Seller")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            Product product = new()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Quantity = model.Quantity
            };

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Product Updated Successfully!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(x => x.ProductMedias).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (product == null) return NotFound();

            product.Is_Deleted = true;
            foreach (var media in product.ProductMedias)
                media.Is_Deleted = true;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            TempData["Error"] = "Product Deleted Successfully!";
            return RedirectToAction("Index");
        }
    }
}