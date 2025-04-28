using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockaBear.Data;
using MockaBear.Models;
using System.Linq;

namespace MockaBear.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        public IActionResult Products(int page = 1, int pageSize = 10)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.DateAdded)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.Products.Count() / pageSize);

            ViewBag.Categories = _context.Categories.ToList();

            return View("~/Views/Admin/ProductsViews/Products.cshtml", products);
        }


        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(IFormFile ImageFile, string Name, string Description, string  Price, bool IsAvailable, int CategoryId)
        {

            if (!decimal.TryParse(Price, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal parsedPrice))
            {
                TempData["Error"] = "Invalid price format.";
                return RedirectToAction("Products");
            }


            if (ImageFile == null || ImageFile.Length == 0)
            {
                TempData["Error"] = "Image file is required.";
                return RedirectToAction("Products");
            }

            var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
            var imagePath = Path.Combine("wwwroot/images/products", imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            var product = new Product
            {
                Name = Name,
                Description = Description,
                Price = parsedPrice,
                IsAvailable = IsAvailable,
                CategoryId = CategoryId,
                Image = imageName
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            TempData["Success"] = "Product added successfully!";
            return RedirectToAction("Products");
        }


        [HttpPost("update")]
        public async Task<IActionResult> Update(int Id, IFormFile ImageFile, string Name, string Description, string Price, bool IsAvailable, int CategoryId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null)
            {
                return NotFound();
            }

            // Parse du prix avec culture en-US
            if (!decimal.TryParse(Price, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal parsedPrice))
            {
                TempData["Error"] = "Invalid price format.";
                return RedirectToAction("Products");
            }

            // Gérer le remplacement de l'image
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var oldImagePath = Path.Combine("wwwroot/images/products", product.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var imagePath = Path.Combine("wwwroot/images/products", imageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                product.Image = imageName;
            }

            // Mise à jour des champs
            product.Name = Name;
            product.Description = Description;
            product.Price = parsedPrice;
            product.IsAvailable = IsAvailable;
            product.CategoryId = CategoryId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction("Products");
        }


    }
}
